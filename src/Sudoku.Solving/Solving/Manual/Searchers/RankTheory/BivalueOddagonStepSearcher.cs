﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Bi-value Oddagon</b> step searcher.
/// The step searcher will include the following techniques:
/// <list type="bullet">
/// <!--<item>Bi-value Oddagon Type 1</item>-->
/// <item>Bi-value Oddagon Type 2</item>
/// <item>Bi-value Oddagon Type 3</item>
/// <!--<item>Bi-value Oddagon Type 4</item>-->
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class BivalueOddagonStepSearcher : IBivalueOddagonStepSearcher
{
	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		if (BivalueMap.Count < 4)
		{
			return null;
		}

		var resultAccumulator = new List<BivalueOddagonStep>();
		var loops = new List<(Cells, IEnumerable<LinkViewNode>)>();
		var tempLoop = new List<int>(14);
		var loopMap = Cells.Empty;

		// Now iterate on each bi-value cells as the start cell to get all possible unique loops,
		// making it the start point to execute the recursion.
		IOrderedEnumerable<BivalueOddagonStep> resultList = default!;
		foreach (int cell in BivalueMap)
		{
			short mask = grid.GetCandidates(cell);
			int d1 = TrailingZeroCount(mask), d2 = mask.GetNextSet(d1);

			loopMap.Clear();
			loops.Clear();
			tempLoop.Clear();

			IUniqueLoopOrBivalueOddagonStepSearcher.SearchForPossibleLoopPatterns(
				grid, d1, d2, cell, (Region)255, 0, 2, ref loopMap,
				tempLoop, () => isValid(ref loopMap), loops
			);

			if (loops.Count == 0)
			{
				continue;
			}

			short comparer = (short)(1 << d1 | 1 << d2);
			foreach (var (currentLoop, links) in loops)
			{
				var extraCellsMap = currentLoop - BivalueMap;
				switch (extraCellsMap.Count)
				{
					case 0:
					{
						throw new InvalidOperationException("The current grid has no solution.");
					}
					case 1:
					{
						if (CheckType1(resultAccumulator, grid, d1, d2, currentLoop, links, extraCellsMap, onlyFindOne) is { } step1)
						{
							return step1;
						}

						break;
					}
					default:
					{
						// Type 2, 3.
						// Here use default label to ensure the order of
						// the handling will be 1->2->3.
						if (CheckType2(resultAccumulator, grid, d1, d2, currentLoop, links, extraCellsMap, comparer, onlyFindOne) is { } step2)
						{
							return step2;
						}

						if (extraCellsMap.Count == 2)
						{
							if (CheckType3(resultAccumulator, grid, d1, d2, currentLoop, links, extraCellsMap, comparer, onlyFindOne) is { } step3)
							{
								return step3;
							}
						}

						break;
					}
				}
			}

			if (resultAccumulator.Count == 0)
			{
				continue;
			}

			resultList =
				from step in IDistinctableStep<BivalueOddagonStep>.Distinct(resultAccumulator)
				orderby step.Loop.Count, step.TechniqueCode
				select step;

			if (onlyFindOne)
			{
				return resultList.First();
			}

			accumulator.AddRange(resultList);
		}

		return null;


		static bool isValid(ref Cells cells)
		{
			foreach (int region in cells.Regions)
			{
				if ((cells & RegionMaps[region]).Count >= 3)
				{
					return false;
				}
			}

			return true;
		}
	}

	private Step? CheckType1(
		ICollection<BivalueOddagonStep> accumulator, in Grid grid, int d1, int d2, in Cells loop,
		IEnumerable<LinkViewNode> links, in Cells extraCellsMap, bool onlyFindOne)
	{
		int extraCell = extraCellsMap[0];
		var conclusions = new List<Conclusion>(2);
		if (grid.Exists(extraCell, d1) is true)
		{
			conclusions.Add(new(ConclusionType.Elimination, extraCell, d1));
		}
		if (grid.Exists(extraCell, d2) is true)
		{
			conclusions.Add(new(ConclusionType.Elimination, extraCell, d2));
		}
		if (conclusions.Count == 0)
		{
			goto ReturnNull;
		}

		var candidateOffsets = new List<CandidateViewNode>(30);
		foreach (int cell in loop - extraCell)
		{
			candidateOffsets.Add(new(0, cell * 9 + d1));
			candidateOffsets.Add(new(0, cell * 9 + d2));
		}

		var step = new BivalueOddagonType1Step(
			ImmutableArray.CreateRange(conclusions),
			ImmutableArray.Create(View.Empty + candidateOffsets + links),
			loop,
			d1,
			d2,
			extraCell
		);

		if (onlyFindOne)
		{
			return step;
		}

		accumulator.Add(step);

	ReturnNull:
		return null;
	}

	private Step? CheckType2(
		ICollection<BivalueOddagonStep> accumulator, in Grid grid, int d1, int d2, in Cells loop,
		IEnumerable<LinkViewNode> links, in Cells extraCellsMap, short comparer, bool onlyFindOne)
	{
		short mask = (short)(grid.GetDigitsUnion(extraCellsMap) & ~comparer);
		if (!IsPow2(mask))
		{
			goto ReturnNull;
		}

		int extraDigit = TrailingZeroCount(mask);
		if (extraCellsMap % CandMaps[extraDigit] is not { Count: not 0 } elimMap)
		{
			goto ReturnNull;
		}

		var candidateOffsets = new List<CandidateViewNode>();
		foreach (int cell in loop)
		{
			foreach (int digit in grid.GetCandidates(cell))
			{
				candidateOffsets.Add(new(digit == extraDigit ? 1 : 0, cell * 9 + digit));
			}
		}

		var step = new BivalueOddagonType2Step(
			ImmutableArray.Create(Conclusion.ToConclusions(elimMap, extraDigit, ConclusionType.Elimination)),
			ImmutableArray.Create(View.Empty + candidateOffsets + links),
			loop,
			d1,
			d2,
			extraDigit
		);

		if (onlyFindOne)
		{
			return step;
		}

		accumulator.Add(step);

	ReturnNull:
		return null;
	}

	private Step? CheckType3(
		ICollection<BivalueOddagonStep> accumulator, in Grid grid, int d1, int d2, in Cells loop,
		IEnumerable<LinkViewNode> links, in Cells extraCellsMap, short comparer, bool onlyFindOne)
	{
		bool notSatisfiedType3 = false;
		foreach (int cell in extraCellsMap)
		{
			short mask = grid.GetCandidates(cell);
			if ((mask & comparer) == 0 || mask == comparer)
			{
				notSatisfiedType3 = true;
				break;
			}
		}

		if (!extraCellsMap.InOneRegion || notSatisfiedType3)
		{
			goto ReturnNull;
		}

		short m = grid.GetDigitsUnion(extraCellsMap);
		if ((m & comparer) != comparer)
		{
			goto ReturnNull;
		}

		short otherDigitsMask = (short)(m & ~comparer);
		foreach (int region in extraCellsMap.CoveredRegions)
		{
			if (((ValueMaps[d1] | ValueMaps[d2]) & RegionMaps[region]) is not [])
			{
				goto ReturnNull;
			}

			var otherCells = (RegionMaps[region] & EmptyMap) - loop;
			for (int size = PopCount((uint)otherDigitsMask) - 1, count = otherCells.Count; size < count; size++)
			{
				foreach (var cells in otherCells & size)
				{
					short mask = grid.GetDigitsUnion(cells);
					if (PopCount((uint)mask) != size + 1 || (mask & otherDigitsMask) != otherDigitsMask)
					{
						continue;
					}

					if ((RegionMaps[region] & EmptyMap) - cells - loop is not { Count: not 0 } elimMap)
					{
						continue;
					}

					var conclusions = new List<Conclusion>(16);
					foreach (int digit in mask)
					{
						foreach (int cell in elimMap & CandMaps[digit])
						{
							conclusions.Add(new(ConclusionType.Elimination, cell, digit));
						}
					}
					if (conclusions.Count == 0)
					{
						continue;
					}

					var candidateOffsets = new List<CandidateViewNode>();
					foreach (int cell in loop)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							candidateOffsets.Add(
								new(
									(otherDigitsMask >> digit & 1) != 0 ? 1 : 0,
									cell * 9 + digit
								)
							);
						}
					}
					foreach (int cell in cells)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							candidateOffsets.Add(new(1, cell * 9 + digit));
						}
					}

					var step = new BivalueOddagonType3Step(
						ImmutableArray.CreateRange(conclusions),
						ImmutableArray.Create(View.Empty + candidateOffsets + new RegionViewNode(0, region) + links),
						loop,
						d1,
						d2,
						cells,
						mask
					);

					if (onlyFindOne)
					{
						return step;
					}

					accumulator.Add(step);
				}
			}
		}

	ReturnNull:
		return null;
	}
}
