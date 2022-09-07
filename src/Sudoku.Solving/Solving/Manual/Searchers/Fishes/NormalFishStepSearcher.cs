﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Normal Fish</b> step searcher. The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>X-Wing</item>
/// <item>Swordfish</item>
/// <item>Jellyfish</item>
/// <item>Finned X-Wing</item>
/// <item>Finned Swordfish</item>
/// <item>Finned Jellyfish</item>
/// <item>Sashimi X-Wing</item>
/// <item>Sashimi Swordfish</item>
/// <item>Sashimi Jellyfish</item>
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class NormalFishStepSearcher : INormalFishStepSearcher
{
	/// <inheritdoc/>
	/// <remarks>
	/// I hide this member on purpose because 4 is the maximum size of subsets found in practice.
	/// </remarks>
	int IFishStepSearcher.MaxSize { get; set; } = 4;


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		int** r = stackalloc int*[9], c = stackalloc int*[9];
		Unsafe.InitBlock(r, 0, (uint)sizeof(int*) * 9);
		Unsafe.InitBlock(c, 0, (uint)sizeof(int*) * 9);

		for (int digit = 0; digit < 9; digit++)
		{
			if (ValueMaps[digit].Count > 5)
			{
				continue;
			}

			// Gather.
			for (int region = 9; region < 27; region++)
			{
				if ((RegionMaps[region] & CandMaps[digit]) is not [])
				{
#pragma warning disable CA2014
					if (region < 18)
					{
						if (r[digit] == null)
						{
							int* ptr = stackalloc int[10];
							Unsafe.InitBlock(ptr, 0, 10 * sizeof(int));

							r[digit] = ptr;
						}

						r[digit][++r[digit][0]] = region;
					}
					else
					{
						if (c[digit] == null)
						{
							int* ptr = stackalloc int[10];
							Unsafe.InitBlock(ptr, 0, 10 * sizeof(int));

							c[digit] = ptr;
						}

						c[digit][++c[digit][0]] = region;
					}
#pragma warning restore CA2014
				}
			}
		}

		for (int size = 2; size <= ((IFishStepSearcher)this).MaxSize; size++)
		{
			if (GetAll(accumulator, grid, size, r, c, false, true, onlyFindOne) is { } finlessRowFish)
			{
				return finlessRowFish;
			}

			if (GetAll(accumulator, grid, size, r, c, false, false, onlyFindOne) is { } finlessColumnFish)
			{
				return finlessColumnFish;
			}

			if (GetAll(accumulator, grid, size, r, c, true, true, onlyFindOne) is { } finnedRowFish)
			{
				return finnedRowFish;
			}

			if (GetAll(accumulator, grid, size, r, c, true, false, onlyFindOne) is { } finnedColumnFish)
			{
				return finnedColumnFish;
			}
		}

		return null;
	}

	/// <summary>
	/// Get all possible normal fishes.
	/// </summary>
	/// <param name="accumulator">The accumulator.</param>
	/// <param name="grid">The grid.</param>
	/// <param name="size">The size.</param>
	/// <param name="r">The possible row table to iterate.</param>
	/// <param name="c">The possible column table to iterate.</param>
	/// <param name="withFin">Indicates whether the searcher will check for the existence of fins.</param>
	/// <param name="searchRow">
	/// Indicates whether the searcher searches for fishes in the direction of rows.
	/// </param>
	/// <param name="onlyFindOne">Indicates whether the method only searches for one step.</param>
	/// <returns>The first found step.</returns>
	private unsafe Step? GetAll(
		ICollection<Step> accumulator, in Grid grid, int size, int** r, int** c,
		bool withFin, bool searchRow, bool onlyFindOne)
	{
		// Iterate on each digit.
		for (int digit = 0; digit < 9; digit++)
		{
			// Check the validity of the distribution for the current digit.
			int* pBase = searchRow ? r[digit] : c[digit], pCover = searchRow ? c[digit] : r[digit];
			if (pBase == null || pBase[0] <= size)
			{
				continue;
			}

			// Iterate on the base set combination.
			foreach (int[] bs in PointerMarshal.GetArrayFromStart(pBase, 10, 1, true).GetSubsets(size))
			{
				// 'baseLine' is the map that contains all base set cells.
				var baseLine = size switch
				{
					2 => CandMaps[digit] & (RegionMaps[bs[0]] | RegionMaps[bs[1]]),
					3 => CandMaps[digit] & (RegionMaps[bs[0]] | RegionMaps[bs[1]] | RegionMaps[bs[2]]),
					4 => CandMaps[digit] & (
						RegionMaps[bs[0]] | RegionMaps[bs[1]] | RegionMaps[bs[2]] | RegionMaps[bs[3]]
					),
					_ => throw new NotSupportedException("The specified size isn't supported.")
				};

				// Iterate on the cover set combination.
				foreach (int[] cs in PointerMarshal.GetArrayFromStart(pCover, 10, 1, true).GetSubsets(size))
				{
					// 'coverLine' is the map that contains all cover set cells.
					var coverLine = size switch
					{
						2 => CandMaps[digit] & (RegionMaps[cs[0]] | RegionMaps[cs[1]]),
						3 => CandMaps[digit] & (RegionMaps[cs[0]] | RegionMaps[cs[1]] | RegionMaps[cs[2]]),
						4 => CandMaps[digit] & (
							RegionMaps[cs[0]] | RegionMaps[cs[1]] | RegionMaps[cs[2]] | RegionMaps[cs[3]]
						),
						_ => throw new NotSupportedException("The specified size isn't supported.")
					};

					// Now check the fins and the elimination cells.
					Cells elimMap, fins = Cells.Empty;
					if (!withFin)
					{
						// If the current searcher doesn't check fins, we'll just get the pure check:
						// 1. Base set contain more cells than cover sets.
						// 2. Elimination cells set isn't empty.
						if (baseLine > coverLine || (elimMap = coverLine - baseLine) is [])
						{
							continue;
						}
					}
					else // Should check fins.
					{
						// All fins should be in the same block.
						fins = baseLine - coverLine;
						short blockMask = fins.BlockMask;
						if (fins is [] || !IsPow2(blockMask))
						{
							continue;
						}

						// Cover set shouldn't overlap with the block of all fins lying in.
						int finBlock = TrailingZeroCount(blockMask);
						if ((coverLine & RegionMaps[finBlock]) is [])
						{
							continue;
						}

						// Don't intersect.
						if ((RegionMaps[finBlock] & coverLine - baseLine) is [])
						{
							continue;
						}

						// Finally, get the elimination cells.
						elimMap = coverLine - baseLine & RegionMaps[finBlock];
					}

					// Gather the conclusions and candidates or regions to be highlighted.
					var candidateOffsets = new List<CandidateViewNode>();
					var regionOffsets = new List<RegionViewNode>();
					foreach (int cell in withFin ? baseLine - fins : baseLine)
					{
						candidateOffsets.Add(new(0, cell * 9 + digit));
					}
					if (withFin)
					{
						foreach (int cell in fins)
						{
							candidateOffsets.Add(new(1, cell * 9 + digit));
						}
					}
					foreach (int baseSet in bs)
					{
						regionOffsets.Add(new(0, baseSet));
					}
					foreach (int coverSet in cs)
					{
						regionOffsets.Add(new(2, coverSet));
					}

					// Gather the result.
					var step = new NormalFishStep(
						ImmutableArray.Create(
							Conclusion.ToConclusions(elimMap, digit, ConclusionType.Elimination)
						),
						ImmutableArray.Create(
							View.Empty + candidateOffsets + regionOffsets,
							GetDirectView(grid, digit, bs, cs, fins, searchRow)
						),
						digit,
						new RegionCollection(bs).Mask,
						new RegionCollection(cs).Mask,
						fins,
						IFishStepSearcher.IsSashimi(bs, fins, digit)
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

	/// <summary>
	/// Get the direct fish view with the specified grid and the base sets.
	/// </summary>
	/// <param name="grid">The grid.</param>
	/// <param name="digit">The digit.</param>
	/// <param name="baseSets">The base sets.</param>
	/// <param name="coverSets">The cover sets.</param>
	/// <param name="fins">
	/// The cells of the fin in the current fish.
	/// </param>
	/// <param name="searchRow">Indicates whether the current searcher searches row.</param>
	/// <returns>The view.</returns>
	private static View GetDirectView(
		in Grid grid, int digit, int[] baseSets, int[] coverSets, in Cells fins, bool searchRow)
	{
		// Get the highlight cells (necessary).
		var cellOffsets = new List<CellViewNode>();
		var candidateOffsets = fins is [] ? null : new List<CandidateViewNode>();
		foreach (int baseSet in baseSets)
		{
			foreach (int cell in RegionMaps[baseSet])
			{
				switch (grid.Exists(cell, digit))
				{
					case true when fins.Contains(cell):
					{
						cellOffsets.Add(new(1, cell));
						break;
					}
					case false:
					case null:
					{
						bool flag = false;
						foreach (int c in ValueMaps[digit])
						{
							if (RegionMaps[c.ToRegionIndex(searchRow ? Region.Column : Region.Row)].Contains(cell))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}

						Cells baseMap = Cells.Empty, coverMap = Cells.Empty;
						foreach (int b in baseSets)
						{
							baseMap |= RegionMaps[b];
						}
						foreach (int c in coverSets)
						{
							coverMap |= RegionMaps[c];
						}
						baseMap &= coverMap;
						if (baseMap.Contains(cell))
						{
							continue;
						}

						cellOffsets.Add(new(0, cell));
						break;
					}
				}
			}
		}

		foreach (int cell in ValueMaps[digit])
		{
			cellOffsets.Add(new(2, cell));
		}
		foreach (int cell in fins)
		{
			candidateOffsets!.Add(new(1, cell * 9 + digit));
		}

		return View.Empty + cellOffsets + candidateOffsets;
	}
}
