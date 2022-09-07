﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Regular Wing</b> step searcher.
/// The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>XY-Wing</item>
/// <item>XYZ-Wing</item>
/// <item>WXYZ-Wing</item>
/// <item>VWXYZ-Wing</item>
/// <item>UVWXYZ-Wing</item>
/// <item>TUVWXYZ-Wing</item>
/// <item>STUVWXYZ-Wing</item>
/// <item>RSTUVWXYZ-Wing</item>
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class RegularWingStepSearcher : IRegularWingStepSearcher
{
	/// <summary>
	/// The inner field of the property <see cref="MaxSize"/>.
	/// </summary>
	/// <seealso cref="MaxSize"/>
	private int _maxSize;


	/// <inheritdoc/>
	public int MaxSize
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _maxSize;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => _maxSize = value > 9 ? throw new ArgumentOutOfRangeException(nameof(value)) : value;
	}


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		// Iterate on the size.
		// Note that the greatest size is determined by two factors: the size that you specified
		// and the number of bi-value cells in the grid.
		for (int size = 3, count = Min(MaxSize, BivalueMap.Count); size <= count; size++)
		{
			// Iterate on each pivot cell.
			foreach (int pivot in EmptyMap)
			{
				short mask = grid.GetCandidates(pivot);
				int candsCount = PopCount((uint)mask);
				if (candsCount != size && candsCount != size - 1)
				{
					// Candidates are not enough.
					continue;
				}

				var map = PeerMaps[pivot] & BivalueMap;
				if (map.Count < size - 1)
				{
					// Bivalue cells are not enough.
					continue;
				}

				// Iterate on each cell combination.
				foreach (int[] cells in map & size - 1)
				{
					// Check duplicate.
					// If two cells contain same candidates, the wing can't be formed.
					bool flag = false;
					for (int i = 0, length = cells.Length, outerLength = length - 1; i < outerLength; i++)
					{
						for (int j = i + 1; j < length; j++)
						{
							if (grid.GetMask(cells[i]) == grid.GetMask(cells[j]))
							{
								flag = true;
								goto CheckWhetherTwoCellsContainSameCandidateKind;
							}
						}
					}

				CheckWhetherTwoCellsContainSameCandidateKind:
					if (flag)
					{
						continue;
					}

					short union = mask, inter = (short)(Grid.MaxCandidatesMask & mask);
					foreach (int cell in cells)
					{
						short m = grid.GetCandidates(cell);
						union |= m;
						inter &= m;
					}

					if (PopCount((uint)union) != size || inter != 0 && !IsPow2(inter))
					{
						continue;
					}

					// Get the Z digit (The digit to be removed).
					bool isIncomplete = inter == 0;
					short interWithoutPivot = (short)(union & ~grid.GetCandidates(pivot));
					short maskToCheck = isIncomplete ? interWithoutPivot : inter;
					if (!IsPow2(maskToCheck))
					{
						continue;
					}

					// The pattern should be "az, bz, cz, dz, ... , abcd(z)".
					int zDigit = TrailingZeroCount(maskToCheck);
					var petals = new Cells(cells);
					if ((petals + pivot & CandMaps[zDigit]).Count != (isIncomplete ? size - 1 : size))
					{
						continue;
					}

					// Check elimination map.
					var elimMap = !petals;
					if (!isIncomplete)
					{
						elimMap &= PeerMaps[pivot];
					}
					elimMap &= CandMaps[zDigit];
					if (elimMap is [])
					{
						continue;
					}

					// Gather highlight candidates.
					var candidateOffsets = new List<CandidateViewNode>(6);
					foreach (int cell in cells)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							candidateOffsets.Add(new(digit == zDigit ? 1 : 0, cell * 9 + digit));
						}
					}
					foreach (int digit in grid.GetCandidates(pivot))
					{
						candidateOffsets.Add(new(digit == zDigit ? 1 : 0, pivot * 9 + digit));
					}

					var step = new RegularWingStep(
						ImmutableArray.Create(
							Conclusion.ToConclusions(elimMap, zDigit, ConclusionType.Elimination)
						),
						ImmutableArray.Create(View.Empty + candidateOffsets),
						pivot,
						PopCount((uint)mask),
						union,
						petals
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
