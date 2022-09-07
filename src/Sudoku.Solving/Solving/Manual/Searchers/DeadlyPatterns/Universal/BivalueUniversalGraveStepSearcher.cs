﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Bivalue Universal Grave</b> step searcher.
/// The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>Bivalue Universal Grave Type 1</item>
/// <item>Bivalue Universal Grave Type 2</item>
/// <item>Bivalue Universal Grave Type 3</item>
/// <item>Bivalue Universal Grave Type 4</item>
/// <item>Bivalue Universal Grave + n</item>
/// <item>Bivalue Universal Grave XZ</item>
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class BivalueUniversalGraveStepSearcher : IBivalueUniversalGraveStepSearcher
{
	/// <inheritdoc/>
	public bool SearchExtendedTypes { get; set; }


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		if (!IBivalueUniversalGraveStepSearcher.FindTrueCandidates(grid, out var trueCandidates))
		{
			return null;
		}

		switch (trueCandidates)
		{
			case []:
			{
				return null;
			}
			case [var trueCandidate]:
			{
				// BUG + 1 found.
				var step = new BivalueUniversalGraveType1Step(
					ImmutableArray.Create(new Conclusion(ConclusionType.Assignment, trueCandidate)),
					ImmutableArray.Create(View.Empty + new CandidateViewNode(0, trueCandidate))
				);
				if (onlyFindOne)
				{
					return step;
				}

				accumulator.Add(step);

				break;
			}
			default:
			{
				if (CheckSingleDigit(trueCandidates))
				{
					if (CheckType2(accumulator, trueCandidates, onlyFindOne) is { } type2Step)
					{
						return type2Step;
					}
				}
				else
				{
					if (SearchExtendedTypes)
					{
						if (CheckMultiple(accumulator, grid, trueCandidates, onlyFindOne) is { } typeMultipleStep)
						{
							return typeMultipleStep;
						}
						if (CheckXz(accumulator, grid, trueCandidates, onlyFindOne) is { } typeXzStep)
						{
							return typeXzStep;
						}
					}

					if (CheckType3Naked(accumulator, grid, trueCandidates, onlyFindOne) is { } type3Step)
					{
						return type3Step;
					}
					if (CheckType4(accumulator, grid, trueCandidates, onlyFindOne) is { } type4Step)
					{
						return type4Step;
					}
				}

				break;
			}
		}

		return null;
	}

	private static Step? CheckType2(
		ICollection<Step> accumulator, IReadOnlyList<int> trueCandidates, bool onlyFindOne)
	{
		var cells = (stackalloc int[trueCandidates.Count]);
		int i = 0;
		foreach (int candidate in trueCandidates)
		{
			cells[i++] = candidate / 9;
		}
		if (!new Cells(cells) is not { Count: not 0 } map)
		{
			return null;
		}

		int digit = trueCandidates[0] % 9;
		if ((map & CandMaps[digit]) is not { Count: not 0 } elimMap)
		{
			return null;
		}

		var conclusions = new List<Conclusion>(elimMap.Count);
		foreach (int cell in elimMap)
		{
			conclusions.Add(new(ConclusionType.Elimination, cell, digit));
		}

		var candidateOffsets = new List<CandidateViewNode>(trueCandidates.Count);
		foreach (int candidate in trueCandidates)
		{
			candidateOffsets.Add(new(0, candidate));
		}

		// BUG type 2.
		var step = new BivalueUniversalGraveType2Step(
			ImmutableArray.CreateRange(conclusions),
			ImmutableArray.Create(View.Empty + candidateOffsets),
			digit,
			new(cells)
		);
		if (onlyFindOne)
		{
			return step;
		}

		accumulator.Add(step);

		return null;
	}

	private static Step? CheckType3Naked(
		ICollection<Step> accumulator, in Grid grid, IReadOnlyList<int> trueCandidates, bool onlyFindOne)
	{
		// Check whether all true candidates lie in a same region.
		var map = new Cells(from c in trueCandidates group c by c / 9 into z select z.Key);
		if (!map.InOneRegion)
		{
			return null;
		}

		// Get the digit mask.
		short digitsMask = 0;
		foreach (int candidate in trueCandidates)
		{
			digitsMask |= (short)(1 << candidate % 9);
		}

		// Iterate on each region that the true candidates lying on.
		foreach (int region in map.CoveredRegions)
		{
			var regionMap = RegionMaps[region];
			if ((regionMap & EmptyMap) - map is not { Count: not 0 } otherCellsMap)
			{
				continue;
			}

			// Iterate on each size.
			for (int size = 1, length = otherCellsMap.Count; size < length; size++)
			{
				foreach (var cells in otherCellsMap & size)
				{
					short mask = (short)(digitsMask | grid.GetDigitsUnion(cells));
					if (PopCount((uint)mask) != size + 1)
					{
						continue;
					}

					if (((regionMap - cells - map) & EmptyMap) is not { Count: not 0 } elimMap)
					{
						continue;
					}

					var conclusions = new List<Conclusion>();
					foreach (int cell in elimMap)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							if ((mask >> digit & 1) != 0)
							{
								conclusions.Add(new(ConclusionType.Elimination, cell, digit));
							}
						}
					}
					if (conclusions.Count == 0)
					{
						continue;
					}

					var candidateOffsets = new List<CandidateViewNode>();
					foreach (int cand in trueCandidates)
					{
						candidateOffsets.Add(new(0, cand));
					}
					foreach (int cell in cells)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							candidateOffsets.Add(new(1, cell * 9 + digit));
						}
					}

					var step = new BivalueUniversalGraveType3Step(
						ImmutableArray.CreateRange(conclusions),
						ImmutableArray.Create(View.Empty + candidateOffsets + new RegionViewNode(0, region)),
						trueCandidates,
						digitsMask,
						cells,
						true
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

	private static Step? CheckType4(
		ICollection<Step> accumulator, in Grid grid, IReadOnlyList<int> trueCandidates, bool onlyFindOne)
	{
		// Conjugate pairs should lie in two cells.
		var candsGroupByCell = from candidate in trueCandidates group candidate by candidate / 9;
		if (candsGroupByCell.Take(3).Count() != 2)
		{
			return null;
		}

		// Check two cell has same region.
		var cells = new List<int>();
		foreach (var candGroupByCell in candsGroupByCell)
		{
			cells.Add(candGroupByCell.Key);
		}

		int regions = new Cells(cells).CoveredRegions;
		if (regions != 0)
		{
			return null;
		}

		// Check for each region.
		foreach (int region in regions)
		{
			// Add up all digits.
			var digits = new HashSet<int>();
			foreach (var candGroupByCell in candsGroupByCell)
			{
				foreach (int cand in candGroupByCell)
				{
					digits.Add(cand % 9);
				}
			}

			// Check whether exists a conjugate pair in this region.
			for (int conjuagtePairDigit = 0; conjuagtePairDigit < 9; conjuagtePairDigit++)
			{
				// Check whether forms a conjugate pair.
				short mask = (RegionMaps[region] & CandMaps[conjuagtePairDigit]) / region;
				if (PopCount((uint)mask) != 2)
				{
					continue;
				}

				// Check whether the conjugate pair lies in current two cells.
				int first = TrailingZeroCount(mask), second = mask.GetNextSet(first);
				int c1 = RegionCells[region][first], c2 = RegionCells[region][second];
				if (c1 != cells[0] || c2 != cells[1])
				{
					continue;
				}

				// Check whether all digits contain that digit.
				if (digits.Contains(conjuagtePairDigit))
				{
					continue;
				}

				// BUG type 4 found.
				// Now add up all eliminations.
				var conclusions = new List<Conclusion>();
				foreach (var candGroupByCell in candsGroupByCell)
				{
					int cell = candGroupByCell.Key;
					short digitMask = 0;
					foreach (int cand in candGroupByCell)
					{
						digitMask |= (short)(1 << cand % 9);
					}

					// Bitwise not.
					foreach (int d in digitMask = (short)(~digitMask & Grid.MaxCandidatesMask))
					{
						if (conjuagtePairDigit == d || grid.Exists(cell, d) is not true)
						{
							continue;
						}

						conclusions.Add(new(ConclusionType.Elimination, cell, d));
					}
				}

				// Check eliminations.
				if (conclusions.Count == 0)
				{
					continue;
				}

				var candidateOffsets = new CandidateViewNode[trueCandidates.Count + 2];
				int i = 0;
				foreach (int candidate in trueCandidates)
				{
					candidateOffsets[i++] = new(0, candidate);
				}
				candidateOffsets[^2] = new(1, c1 * 9 + conjuagtePairDigit);
				candidateOffsets[^1] = new(1, c2 * 9 + conjuagtePairDigit);

				// BUG type 4.
				short digitsMask = 0;
				foreach (int digit in digits)
				{
					digitsMask |= (short)(1 << digit);
				}

				var step = new BivalueUniversalGraveType4Step(
					ImmutableArray.CreateRange(conclusions),
					ImmutableArray.Create(View.Empty + candidateOffsets + new RegionViewNode(0, region)),
					digitsMask,
					new(cells),
					new(c1, c2, conjuagtePairDigit)
				);
				if (onlyFindOne)
				{
					return step;
				}

				accumulator.Add(step);
			}
		}

		return null;
	}

	private static Step? CheckMultiple(
		ICollection<Step> accumulator, in Grid grid, IReadOnlyList<int> trueCandidates, bool onlyFindOne)
	{
		if (trueCandidates.Count > 18)
		{
			return null;
		}

		if (!new Candidates(trueCandidates) is not { Count: var mapCount and not 0 } map)
		{
			return null;
		}

		// BUG + n found.
		// Check eliminations.
		var conclusions = new List<Conclusion>(mapCount);
		foreach (int candidate in map)
		{
			if (grid.Exists(candidate / 9, candidate % 9) is true)
			{
				conclusions.Add(new(ConclusionType.Elimination, candidate));
			}
		}
		if (conclusions.Count == 0)
		{
			return null;
		}

		var candidateOffsets = new CandidateViewNode[trueCandidates.Count];
		int i = 0;
		foreach (int candidate in trueCandidates)
		{
			candidateOffsets[i++] = new(0, candidate);
		}

		// BUG + n.
		var step = new BivalueUniversalGraveMultipleStep(
			ImmutableArray.CreateRange(conclusions),
			ImmutableArray.Create(View.Empty + candidateOffsets),
			trueCandidates
		);
		if (onlyFindOne)
		{
			return step;
		}

		accumulator.Add(step);

		return null;
	}

	private static Step? CheckXz(
		ICollection<Step> accumulator, in Grid grid, IReadOnlyList<int> trueCandidates, bool onlyFindOne)
	{
		if (trueCandidates.Count > 2)
		{
			return null;
		}

		int cand1 = trueCandidates[0], cand2 = trueCandidates[1];
		int c1 = cand1 / 9, c2 = cand2 / 9, d1 = cand1 % 9, d2 = cand2 % 9;
		short mask = (short)(1 << d1 | 1 << d2);
		foreach (int cell in (PeerMaps[c1] ^ PeerMaps[c2]) & BivalueMap)
		{
			if (grid.GetCandidates(cell) != mask)
			{
				continue;
			}

			// BUG-XZ found.
			var conclusions = new List<Conclusion>();
			bool condition = (Cells.Empty + c1 + cell).InOneRegion;
			int anotherCell = condition ? c2 : c1;
			int anotherDigit = condition ? d2 : d1;
			foreach (int peer in !(Cells.Empty + cell + anotherCell))
			{
				if (grid.Exists(peer, anotherDigit) is true)
				{
					conclusions.Add(new(ConclusionType.Elimination, peer, anotherDigit));
				}
			}
			if (conclusions.Count == 0)
			{
				continue;
			}

			var candidateOffsets = new CandidateViewNode[trueCandidates.Count];
			for (int i = 0, count = trueCandidates.Count; i < count; i++)
			{
				candidateOffsets[i] = new(0, trueCandidates[i]);
			}

			var step = new BivalueUniversalGraveXzStep(
				ImmutableArray.CreateRange(conclusions),
				ImmutableArray.Create(View.Empty + new CellViewNode(0, cell) + candidateOffsets),
				mask,
				Cells.Empty + c1 + c2,
				cell
			);
			if (onlyFindOne)
			{
				return step;
			}

			accumulator.Add(step);
		}

		return null;
	}

	/// <summary>
	/// Check whether all candidates in the list has same digit value.
	/// </summary>
	/// <param name="list">The list of all true candidates.</param>
	/// <returns>A <see cref="bool"/> indicating that.</returns>
	private static bool CheckSingleDigit(IReadOnlyList<int> list)
	{
		int i = 0;
		Unsafe.SkipInit(out int comparer);
		foreach (int cand in list)
		{
			if (i++ == 0)
			{
				comparer = cand % 9;
				continue;
			}

			if (comparer != cand % 9)
			{
				return false;
			}
		}

		return true;
	}
}
