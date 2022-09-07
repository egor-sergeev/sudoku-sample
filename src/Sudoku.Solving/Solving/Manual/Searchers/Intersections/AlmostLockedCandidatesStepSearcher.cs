﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with an <b>Almost Locked Candidates</b> step searcher.
/// The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>Almost Locked Pair</item>
/// <item>Almost Locked Triple</item>
/// <item>Almost Locked Quadruple</item>
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class AlmostLockedCandidatesStepSearcher : IAlmostLockedCandidatesStepSearcher
{
	/// <inheritdoc/>
	public bool CheckAlmostLockedQuadruple { get; set; }

	/// <inheritdoc/>
	bool IAlmostLockedCandidatesStepSearcher.CheckForValues { get; set; } = false;


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		for (int size = 2, maxSize = CheckAlmostLockedQuadruple ? 4 : 3; size <= maxSize; size++)
		{
			foreach (var ((baseSet, coverSet), (a, b, c, _)) in IntersectionMaps)
			{
				if ((c & EmptyMap) is not [])
				{
					if (GetAll(accumulator, grid, size, baseSet, coverSet, a, b, c, onlyFindOne) is { } step1)
					{
						return step1;
					}
					if (GetAll(accumulator, grid, size, coverSet, baseSet, b, a, c, onlyFindOne) is { } step2)
					{
						return step2;
					}
				}
			}
		}

		return null;
	}

	/// <summary>
	/// Process the calculation.
	/// </summary>
	/// <param name="result">The result.</param>
	/// <param name="grid">The grid.</param>
	/// <param name="size">The size.</param>
	/// <param name="baseSet">The base set.</param>
	/// <param name="coverSet">The cover set.</param>
	/// <param name="a">The left grid map.</param>
	/// <param name="b">The right grid map.</param>
	/// <param name="c">The intersection.</param>
	/// <param name="onlyFindOne">Indicates whether the searcher only searching for one step is okay.</param>
	/// <remarks>
	/// <para>
	/// The diagrams:
	/// <code><![CDATA[
	/// ALP:
	/// abx aby | ab
	/// abz     |
	///
	/// ALT:
	/// abcw abcx | abc abc
	/// abcy abcz |
	/// ]]></code>
	/// </para>
	/// <para>Algorithm:</para>
	/// <para>
	/// If the cell <c>ab</c> (in ALP) or <c>abc</c> (in ALT) is filled with the digit <c>p</c>,
	/// then the cells <c>abx</c> and <c>aby</c> (in ALP) and <c>abcw</c> and <c>abcx</c> (in ALT) can't
	/// fill the digit <c>p</c>. Therefore the digit <c>p</c> can only be filled into the left-side block.
	/// </para>
	/// <para>
	/// If the block only contains those cells that can contain the digit <c>p</c>, the ALP or ALT will be formed,
	/// and the elimination is <c>z</c> (in ALP) and <c>y</c> and <c>z</c> (in ALT).
	/// </para>
	/// </remarks>
	private static Step? GetAll(
		ICollection<Step> result, in Grid grid, int size, int baseSet, int coverSet,
		in Cells a, in Cells b, in Cells c, bool onlyFindOne)
	{
		// Iterate on each cell combination.
		foreach (var cells in a & EmptyMap & size - 1)
		{
			// Gather the mask. The cell combination must contain the specified number of digits.
			short mask = grid.GetDigitsUnion(cells);
			if (PopCount((uint)mask) != size)
			{
				continue;
			}

			// Check whether overlapped.
			bool isOverlapped = false;
			foreach (int digit in mask)
			{
				if ((ValueMaps[digit] & RegionMaps[coverSet]) is not [])
				{
					isOverlapped = true;
					break;
				}
			}
			if (isOverlapped)
			{
				continue;
			}

			// Then check whether the another region (left-side block in those diagrams)
			// forms an AHS (i.e. those digits must appear in the specified cells).
			short ahsMask = 0;
			foreach (int digit in mask)
			{
				ahsMask |= (RegionMaps[coverSet] & CandMaps[digit] & b) / coverSet;
			}
			if (PopCount((uint)ahsMask) != size - 1)
			{
				continue;
			}

			// Gather the AHS cells.
			var ahsCells = Cells.Empty;
			foreach (int pos in ahsMask)
			{
				ahsCells.Add(RegionCells[coverSet][pos]);
			}

			// Gather all eliminations.
			var conclusions = new List<Conclusion>();
			foreach (int aCell in a)
			{
				if (cells.Contains(aCell))
				{
					continue;
				}

				foreach (int digit in mask & grid.GetCandidates(aCell))
				{
					conclusions.Add(new(ConclusionType.Elimination, aCell, digit));
				}
			}
			foreach (int digit in Grid.MaxCandidatesMask & ~mask)
			{
				foreach (int ahsCell in ahsCells & CandMaps[digit])
				{
					conclusions.Add(new(ConclusionType.Elimination, ahsCell, digit));
				}
			}

			// Check whether any eliminations exists.
			if (conclusions.Count == 0)
			{
				continue;
			}

			// Gather highlight candidates.
			var candidateOffsets = new List<CandidateViewNode>();
			foreach (int digit in mask)
			{
				foreach (int cell in cells & CandMaps[digit])
				{
					candidateOffsets.Add(new(0, cell * 9 + digit));
				}
			}
			foreach (int cell in c)
			{
				foreach (int digit in mask & grid.GetCandidates(cell))
				{
					candidateOffsets.Add(new(1, cell * 9 + digit));
				}
			}
			foreach (int cell in ahsCells)
			{
				foreach (int digit in mask & grid.GetCandidates(cell))
				{
					candidateOffsets.Add(new(0, cell * 9 + digit));
				}
			}

			var map = (cells | ahsCells) - EmptyMap;
			var valueCells = new List<CellViewNode>(map.Count);
			foreach (int cell in map)
			{
				valueCells.Add(new(0, cell));
			}

			bool hasValueCell = valueCells.Count != 0;
			var step = new AlmostLockedCandidatesStep(
				ImmutableArray.CreateRange(conclusions),
				ImmutableArray.Create(
					View.Empty
						+ (hasValueCell ? valueCells : null)
						+ candidateOffsets
						+ new RegionViewNode[] { new(0, baseSet), new(2, coverSet) }
				),
				mask,
				cells,
				ahsCells,
				hasValueCell
			);

			if (onlyFindOne)
			{
				return step;
			}

			result.Add(step);
		}

		return null;
	}
}
