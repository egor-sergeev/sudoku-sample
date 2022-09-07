﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Qiu's Deadly Pattern</b> step searcher.
/// The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>Qiu's Deadly Pattern Type 1</item>
/// <item>Qiu's Deadly Pattern Type 2</item>
/// <item>Qiu's Deadly Pattern Type 3</item>
/// <item>Qiu's Deadly Pattern Type 4</item>
/// <item>Qiu's Deadly Pattern Locked Type</item>
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class QiuDeadlyPatternStepSearcher : IQiuDeadlyPatternStepSearcher
{
	/// <summary>
	/// All different patterns.
	/// </summary>
	private static readonly QiuDeadlyPattern[] Patterns = new QiuDeadlyPattern[QiuDeadlyPatternTemplatesCount];


	/// <include file='../../global-doc-comments.xml' path='g/static-constructor' />
	static QiuDeadlyPatternStepSearcher()
	{
		int[,] BaseLineIterator =
		{
			{ 9, 10 }, { 9, 11 }, { 10, 11 }, { 12, 13 }, { 12, 14 }, { 13, 14 },
			{ 15, 16 }, { 15, 17 }, { 16, 17 }, { 18, 19 }, { 18, 20 }, { 19, 20 },
			{ 21, 22 }, { 21, 23 }, { 22, 23 }, { 24, 25 }, { 24, 26 }, { 25, 26 }
		};
		int[,] StartCells =
		{
			{ 0, 1 }, { 0, 2 }, { 1, 2 }, { 3, 4 }, { 3, 5 }, { 4, 5 },
			{ 6, 7 }, { 6, 8 }, { 7, 8 }, { 0, 9 }, { 0, 18 }, { 9, 18 },
			{ 27, 36 }, { 27, 45 }, { 36, 45 }, { 54, 63 }, { 54, 72 }, { 63, 72 }
		};

		for (
			int i = 0, n = 0, length = BaseLineIterator.Length, iterationLengthOuter = length >> 1;
			i < iterationLengthOuter;
			i++
		)
		{
			bool isRow = i < length >> 2;
			var baseLineMap = RegionMaps[BaseLineIterator[i, 0]] | RegionMaps[BaseLineIterator[i, 1]];
			for (int j = isRow ? 0 : 9, z = 0, iterationLengthInner = length >> 2; z < iterationLengthInner; j++, z++)
			{
				int c1 = StartCells[j, 0], c2 = StartCells[j, 1];
				for (int k = 0; k < 9; k++, c1 += isRow ? 9 : 1, c2 += isRow ? 9 : 1)
				{
					var pairMap = Cells.Empty + c1 + c2;
					if ((baseLineMap & pairMap) is not [])
					{
						continue;
					}

					var tempMapBlock =
						RegionMaps[c1.ToRegionIndex(Region.Block)]
							| RegionMaps[c2.ToRegionIndex(Region.Block)];
					if ((baseLineMap & tempMapBlock) is not [])
					{
						continue;
					}

					var tempMapLine =
						RegionMaps[c1.ToRegionIndex(isRow ? Region.Column : Region.Row)]
							| RegionMaps[c2.ToRegionIndex(isRow ? Region.Column : Region.Row)];
					var squareMap = baseLineMap & tempMapLine;
					Patterns[n++] = new(squareMap, baseLineMap - squareMap, pairMap);
				}
			}
		}
	}


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		for (int i = 0, length = Patterns.Length; i < length; i++)
		{
			bool isRow = i < length >> 1;
			if (
				Patterns[i] is not
				{
					Pair: [var pairFirst, var pairSecond] pair,
					Square: var square,
					BaseLine: var baseLine
				} pattern
			)
			{
				continue;
			}

			// To check whether both two pair cells are empty.
			if (!EmptyMap.Contains(pairFirst) || !EmptyMap.Contains(pairSecond))
			{
				continue;
			}

			// Step 1: To determine whether the distinction degree of base line is 1.
			short appearedDigitsMask = 0, distinctionMask = 0;
			int appearedParts = 0;
			for (int j = 0, region = isRow ? 18 : 9; j < 9; j++, region++)
			{
				var regionMap = RegionMaps[region];
				if ((baseLine & regionMap) is { Count: not 0 } tempMap)
				{
					f(grid, tempMap, ref appearedDigitsMask, ref distinctionMask, ref appearedParts);
				}
				else if ((square & regionMap) is { Count: not 0 } squareMap)
				{
					// Don't forget to record the square cells.
					f(grid, squareMap, ref appearedDigitsMask, ref distinctionMask, ref appearedParts);
				}

				static void f(
					in Grid grid, in Cells map, ref short appearedDigitsMask,
					ref short distinctionMask, ref int appearedParts)
				{
					bool flag = false;
					int[] offsets = map.ToArray();
					int c1 = offsets[0], c2 = offsets[1];
					if (!EmptyMap.Contains(c1))
					{
						int d1 = grid[c1];
						distinctionMask ^= (short)(1 << d1);
						appearedDigitsMask |= (short)(1 << d1);

						flag = true;
					}
					if (!EmptyMap.Contains(c2))
					{
						int d2 = grid[c2];
						distinctionMask ^= (short)(1 << d2);
						appearedDigitsMask |= (short)(1 << d2);

						flag = true;
					}

					appearedParts += flag ? 1 : 0;
				}
			}

			if (!IsPow2(distinctionMask) || appearedParts != PopCount((uint)appearedDigitsMask))
			{
				continue;
			}

			short pairMask = grid.GetDigitsUnion(pair);

			// Iterate on each combination.
			for (int size = 2, count = PopCount((uint)pairMask); size < count; size++)
			{
				foreach (int[] digits in pairMask.GetAllSets().GetSubsets(size))
				{
					// Step 2: To determine whether the digits in pair cells
					// will only appears in square cells.
					var tempMap = Cells.Empty;
					foreach (int digit in digits)
					{
						tempMap |= CandMaps[digit];
					}
					var appearingMap = tempMap & square;
					if (appearingMap.Count != 4)
					{
						continue;
					}

					bool flag = false;
					foreach (int digit in digits)
					{
						if ((square & CandMaps[digit]) is [])
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}

					short comparer = 0;
					foreach (int digit in digits)
					{
						comparer |= (short)(1 << digit);
					}
					short otherDigitsMask = (short)(pairMask & ~comparer);
					if (appearingMap == (tempMap & RegionMaps[TrailingZeroCount(square.BlockMask)]))
					{
						// Qdp forms.
						// Now check each type.
						if (CheckType1(accumulator, grid, isRow, pair, square, baseLine, pattern, comparer, otherDigitsMask, onlyFindOne) is { } type1Step)
						{
							return type1Step;
						}
						if (CheckType2(accumulator, grid, isRow, pair, square, baseLine, pattern, comparer, otherDigitsMask, onlyFindOne) is { } type2Step)
						{
							return type2Step;
						}
						if (CheckType3(accumulator, grid, isRow, pair, square, baseLine, pattern, comparer, otherDigitsMask, onlyFindOne) is { } type3Step)
						{
							return type3Step;
						}
					}
				}
			}

			if (CheckType4(accumulator, isRow, pair, square, baseLine, pattern, pairMask, onlyFindOne) is { } type4Step)
			{
				return type4Step;
			}
			if (CheckLockedType(accumulator, grid, isRow, pair, square, baseLine, pattern, pairMask, onlyFindOne) is { } typeLockedStep)
			{
				return typeLockedStep;
			}
		}

		return null;
	}

	private static Step? CheckType1(
		ICollection<Step> accumulator, in Grid grid, bool isRow, in Cells pair,
		in Cells square, in Cells baseLine, in QiuDeadlyPattern pattern,
		short comparer, short otherDigitsMask, bool onlyFindOne)
	{
		if (!IsPow2(otherDigitsMask))
		{
			return null;
		}

		int extraDigit = TrailingZeroCount(otherDigitsMask);
		var map = pair & CandMaps[extraDigit];
		if (map is not [var elimCell])
		{
			return null;
		}

		short mask = (short)(grid.GetCandidates(elimCell) & ~(1 << extraDigit));
		if (mask == 0)
		{
			return null;
		}

		var conclusions = new List<Conclusion>();
		foreach (int digit in mask)
		{
			conclusions.Add(new(ConclusionType.Elimination, elimCell, digit));
		}

		var cellsMap = square | pair;
		var cellOffsets = new CellViewNode[cellsMap.Count];
		int i = 0;
		foreach (int cell in cellsMap)
		{
			cellOffsets[i++] = new(0, cell);
		}

		var candidateOffsets = new List<CandidateViewNode>();
		foreach (int digit in comparer)
		{
			foreach (int cell in square & CandMaps[digit])
			{
				candidateOffsets.Add(new(1, cell * 9 + digit));
			}
		}
		int anotherCellInPair = (pair - map)[0];
		foreach (int digit in grid.GetCandidates(anotherCellInPair))
		{
			candidateOffsets.Add(new(0, anotherCellInPair * 9 + digit));
		}

		short lineMask = isRow ? baseLine.RowMask : baseLine.ColumnMask;
		var step = new QiuDeadlyPatternType1Step(
			ImmutableArray.CreateRange(conclusions),
			ImmutableArray.Create(
				View.Empty
					+ cellOffsets
					+ candidateOffsets
					+ from pos in lineMask.GetAllSets() select new RegionViewNode(0, pos + (isRow ? 9 : 18))
			),
			pattern,
			elimCell * 9 + extraDigit
		);
		if (onlyFindOne)
		{
			return step;
		}

		accumulator.Add(step);

		return null;
	}

	private static Step? CheckType2(
		ICollection<Step> accumulator, in Grid grid, bool isRow,
		in Cells pair, in Cells square, in Cells baseLine, in QiuDeadlyPattern pattern,
		short comparer, short otherDigitsMask, bool onlyFindOne)
	{
		if (!IsPow2(otherDigitsMask))
		{
			return null;
		}

		int extraDigit = TrailingZeroCount(otherDigitsMask);
		var map = pair & CandMaps[extraDigit];
		if ((!map & CandMaps[extraDigit]) is not { Count: not 0 } elimMap)
		{
			return null;
		}

		var conclusions = new List<Conclusion>();
		foreach (int cell in elimMap)
		{
			conclusions.Add(new(ConclusionType.Elimination, cell, extraDigit));
		}

		var cellsMap = square | pair;
		var cellOffsets = new CellViewNode[cellsMap.Count];
		int i = 0;
		foreach (int cell in cellsMap)
		{
			cellOffsets[i++] = new(0, cell);
		}
		var candidateOffsets = new List<CandidateViewNode>();
		foreach (int digit in comparer)
		{
			foreach (int cell in square & CandMaps[digit])
			{
				candidateOffsets.Add(new(1, cell * 9 + digit));
			}
		}
		foreach (int cell in pair)
		{
			foreach (int digit in grid.GetCandidates(cell))
			{
				candidateOffsets.Add(new(digit == extraDigit ? 1 : 0, cell * 9 + digit));
			}
		}

		short lineMask = isRow ? baseLine.RowMask : baseLine.ColumnMask;
		var step = new QiuDeadlyPatternType2Step(
			ImmutableArray.CreateRange(conclusions),
			ImmutableArray.Create(
				View.Empty
					+ cellOffsets
					+ candidateOffsets
					+ from pos in lineMask.GetAllSets() select new RegionViewNode(0, pos + (isRow ? 9 : 18))
			),
			pattern,
			extraDigit
		);
		if (onlyFindOne)
		{
			return step;
		}

		accumulator.Add(step);

		return null;
	}

	private static Step? CheckType3(
		ICollection<Step> accumulator, in Grid grid, bool isRow,
		in Cells pair, in Cells square, in Cells baseLine, in QiuDeadlyPattern pattern,
		short comparer, short otherDigitsMask, bool onlyFindOne)
	{
		foreach (int region in pair.CoveredRegions)
		{
			var allCellsMap = (RegionMaps[region] & EmptyMap) - pair;
			for (
				int size = PopCount((uint)otherDigitsMask) - 1, length = allCellsMap.Count;
				size < length;
				size++
			)
			{
				foreach (var cells in allCellsMap & size)
				{
					short mask = grid.GetDigitsUnion(cells);
					if ((mask & comparer) != comparer || PopCount((uint)mask) != size + 1)
					{
						continue;
					}

					var conclusions = new List<Conclusion>();
					foreach (int digit in mask)
					{
						foreach (int cell in allCellsMap - cells & CandMaps[digit])
						{
							conclusions.Add(new(ConclusionType.Elimination, cell, digit));
						}
					}
					if (conclusions.Count == 0)
					{
						continue;
					}

					var cellsMap = square | pair;
					var cellOffsets = new CellViewNode[cellsMap.Count];
					int i = 0;
					foreach (int cell in cellsMap)
					{
						cellOffsets[i++] = new(0, cell);
					}
					var candidateOffsets = new List<CandidateViewNode>();
					foreach (int digit in comparer)
					{
						foreach (int cell in square & CandMaps[digit])
						{
							candidateOffsets.Add(new(1, cell * 9 + digit));
						}
					}
					foreach (int cell in pair)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							candidateOffsets.Add(
								new((otherDigitsMask >> digit & 1) != 0 ? 1 : 0, cell * 9 + digit));
						}
					}
					foreach (int cell in cells)
					{
						foreach (int digit in grid.GetCandidates(cell))
						{
							candidateOffsets.Add(new(1, cell * 9 + digit));
						}
					}

					short lineMask = isRow ? baseLine.RowMask : baseLine.ColumnMask;
					var step = new QiuDeadlyPatternType3Step(
						ImmutableArray.CreateRange(conclusions),
						ImmutableArray.Create(
							View.Empty
								+ cellOffsets
								+ candidateOffsets
								+ from pos in lineMask.GetAllSets() select new RegionViewNode(0, pos + (isRow ? 9 : 18))
						),
						pattern,
						mask,
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
		ICollection<Step> accumulator, bool isRow, in Cells pair, in Cells square, in Cells baseLine,
		in QiuDeadlyPattern pattern, short comparer, bool onlyFindOne)
	{
		foreach (int region in pair.CoveredRegions)
		{
			foreach (int digit in comparer)
			{
				if ((CandMaps[digit] & RegionMaps[region]) != pair)
				{
					continue;
				}

				short otherDigitsMask = (short)(comparer & ~(1 << digit));
				bool flag = false;
				foreach (int d in otherDigitsMask)
				{
					if ((ValueMaps[d] & RegionMaps[region]) is not []
						|| (RegionMaps[region] & CandMaps[d]) != square)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}

				int elimDigit = TrailingZeroCount(comparer & ~(1 << digit));
				var elimMap = pair & CandMaps[elimDigit];
				if (elimMap is [])
				{
					continue;
				}

				var conclusions = new List<Conclusion>();
				foreach (int cell in elimMap)
				{
					conclusions.Add(new(ConclusionType.Elimination, cell, elimDigit));
				}

				var cellsMap = square | pair;
				var cellOffsets = new CellViewNode[cellsMap.Count];
				int i = 0;
				foreach (int cell in cellsMap)
				{
					cellOffsets[i++] = new(0, cell);
				}
				var candidateOffsets = new List<CandidateViewNode>();
				foreach (int d in comparer)
				{
					foreach (int cell in square & CandMaps[d])
					{
						candidateOffsets.Add(new(1, cell * 9 + d));
					}
				}
				foreach (int cell in pair)
				{
					candidateOffsets.Add(new(1, cell * 9 + digit));
				}

				short lineMask = isRow ? baseLine.RowMask : baseLine.ColumnMask;
				var step = new QiuDeadlyPatternType4Step(
					ImmutableArray.CreateRange(conclusions),
					ImmutableArray.Create(
						View.Empty
							+ cellOffsets
							+ candidateOffsets
							+ from pos in lineMask.GetAllSets() select new RegionViewNode(0, pos + (isRow ? 9 : 18))
					),
					pattern,
					new(pair, digit)
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

	private static Step? CheckLockedType(
		ICollection<Step> accumulator, in Grid grid, bool isRow,
		in Cells pair, in Cells square, in Cells baseLine, in QiuDeadlyPattern pattern,
		short comparer, bool onlyFindOne)
	{
		// Firstly, we should check the cells in the block that the square cells lying on.
		int block = TrailingZeroCount(square.BlockMask);
		var otherCellsMap = (RegionMaps[block] & EmptyMap) - square;
		var tempMap = Cells.Empty;
		var pairDigits = comparer.GetAllSets();

		bool flag = false;
		foreach (int digit in pairDigits)
		{
			if ((ValueMaps[digit] & RegionMaps[block]) is not [])
			{
				flag = true;
				break;
			}

			tempMap |= CandMaps[digit];
		}
		if (flag)
		{
			return null;
		}

		otherCellsMap &= tempMap;
		if (otherCellsMap is { Count: 0 or > 5 })
		{
			return null;
		}

		// May be in one region or span two regions.
		// Now we check for this case.
		var candidates = new List<int>();
		foreach (int cell in otherCellsMap)
		{
			foreach (int digit in pairDigits)
			{
				if (grid.Exists(cell, digit) is true)
				{
					candidates.Add(cell * 9 + digit);
				}
			}
		}

		if (!new Candidates(candidates) is not { Count: not 0 } elimMap)
		{
			return null;
		}

		var conclusions = new List<Conclusion>();
		foreach (int candidate in elimMap)
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

		var cellsMap = square | pair;
		var cellOffsets = new CellViewNode[cellsMap.Count];
		int i = 0;
		foreach (int cell in cellsMap)
		{
			cellOffsets[i++] = new(0, cell);
		}
		var candidateOffsets = new List<CandidateViewNode>();
		foreach (int d in comparer)
		{
			foreach (int cell in square & CandMaps[d])
			{
				candidateOffsets.Add(new(1, cell * 9 + d));
			}
		}
		foreach (int cell in pair)
		{
			foreach (int digit in grid.GetCandidates(cell))
			{
				candidateOffsets.Add(new(0, cell * 9 + digit));
			}
		}
		foreach (int candidate in candidates)
		{
			candidateOffsets.Add(new(2, candidate));
		}

		short lineMask = isRow ? baseLine.RowMask : baseLine.ColumnMask;
		var step = new QiuDeadlyPatternLockedTypeStep(
			ImmutableArray.CreateRange(conclusions),
			ImmutableArray.Create(
				View.Empty
					+ cellOffsets
					+ candidateOffsets
					+ from pos in lineMask.GetAllSets() select new RegionViewNode(0, pos + (isRow ? 9 : 18))
			),
			pattern,
			candidates
		);
		if (onlyFindOne)
		{
			return step;
		}

		accumulator.Add(step);

		return null;
	}
}
