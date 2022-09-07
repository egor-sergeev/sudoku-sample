﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Subset</b> step searcher. The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>Locked Pair</item>
/// <item>Locked Triple</item>
/// <item>Naked Pair</item>
/// <item>Naked Triple</item>
/// <item>Naked Quadruple</item>
/// <item>Naked Pair (+)</item>
/// <item>Naked Triple (+)</item>
/// <item>Naked Quadruple (+)</item>
/// <item>Hidden Pair</item>
/// <item>Hidden Triple</item>
/// <item>Hidden Quadruple</item>
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class SubsetStepSearcher : ISubsetStepSearcher
{
	/// <inheritdoc/>
	/// <remarks>
	/// I hide this member on purpose because 4 is the maximum size of subsets found in practice.
	/// </remarks>
	int ISubsetStepSearcher.MaxSize { get; set; } = 4;


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		for (int size = 2; size <= ((ISubsetStepSearcher)this).MaxSize; size++)
		{
			// Naked subsets.
			for (int region = 0; region < 27; region++)
			{
				if ((RegionMaps[region] & EmptyMap) is not { Count: >= 2 } currentEmptyMap)
				{
					continue;
				}

				// Iterate on each combination.
				foreach (var cells in currentEmptyMap & size)
				{
					short mask = grid.GetDigitsUnion(cells);
					if (PopCount((uint)mask) != size)
					{
						continue;
					}

					// Naked subset found. Now check eliminations.
					short flagMask = 0;
					var conclusions = new List<Conclusion>(18);
					foreach (int digit in mask)
					{
						var map = cells % CandMaps[digit];
						flagMask |= (short)(map.InOneRegion ? 0 : 1 << digit);

						foreach (int cell in map)
						{
							conclusions.Add(new(ConclusionType.Elimination, cell, digit));
						}
					}
					if (conclusions.Count == 0)
					{
						continue;
					}

					var candidateOffsets = new List<CandidateViewNode>(16);
					foreach (int cell in cells)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							candidateOffsets.Add(new(0, cell * 9 + digit));
						}
					}

					bool? isLocked = flagMask == mask ? true : flagMask != 0 ? false : null;
					var step = new NakedSubsetStep(
						ImmutableArray.CreateRange(conclusions),
						ImmutableArray.Create(View.Empty + candidateOffsets + new RegionViewNode(0, region)),
						region,
						cells,
						mask,
						isLocked
					);

					if (onlyFindOne)
					{
						return step;
					}

					accumulator.Add(step);
				}
			}

			// Hidden subsets.
			for (int region = 0; region < 27; region++)
			{
				var traversingMap = RegionMaps[region] - EmptyMap;
				if (traversingMap.Count >= 8)
				{
					// No available digit (Or hidden single).
					continue;
				}

				short mask = Grid.MaxCandidatesMask;
				foreach (int cell in traversingMap)
				{
					mask &= (short)~(1 << grid[cell]);
				}
				foreach (int[] digits in mask.GetAllSets().GetSubsets(size))
				{
					short tempMask = mask;
					short digitsMask = 0;
					var map = Cells.Empty;
					foreach (int digit in digits)
					{
						tempMask &= (short)~(1 << digit);
						digitsMask |= (short)(1 << digit);
						map |= RegionMaps[region] & CandMaps[digit];
					}
					if (map.Count != size)
					{
						continue;
					}

					// Gather eliminations.
					var conclusions = new List<Conclusion>();
					foreach (int digit in tempMask)
					{
						foreach (int cell in map & CandMaps[digit])
						{
							conclusions.Add(new(ConclusionType.Elimination, cell, digit));
						}
					}
					if (conclusions.Count == 0)
					{
						continue;
					}

					// Gather highlight candidates.
					var candidateOffsets = new List<CandidateViewNode>();
					foreach (int digit in digits)
					{
						foreach (int cell in map & CandMaps[digit])
						{
							candidateOffsets.Add(new(0, cell * 9 + digit));
						}
					}

					var step = new HiddenSubsetStep(
						ImmutableArray.CreateRange(conclusions),
						ImmutableArray.Create(View.Empty + candidateOffsets + new RegionViewNode(0, region)),
						region,
						map,
						digitsMask
					);

					if (onlyFindOne)
					{
						return step;
					}

					accumulator.Add(step);
				}
			}
		}

		return null;
	}
}
