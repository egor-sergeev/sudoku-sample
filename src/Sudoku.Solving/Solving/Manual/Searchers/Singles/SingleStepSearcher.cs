﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Single</b> step searcher. The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>Full House</item>
/// <item>Last Digit</item>
/// <item>Hidden Single</item>
/// <item>Naked Single</item>
/// </list>
/// </summary>
[StepSearcher]
[StepSearcherOptions(IsDirect = true, IsOptionsFixed = true)]
public sealed unsafe partial class SingleStepSearcher : ISingleStepSearcher
{
	/// <inheritdoc/>
	public bool EnableFullHouse { get; set; }

	/// <inheritdoc/>
	public bool EnableLastDigit { get; set; }

	/// <inheritdoc/>
	public bool HiddenSinglesInBlockFirst { get; set; }

	/// <inheritdoc/>
	public bool ShowDirectLines { get; set; }


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		if (!EnableFullHouse)
		{
			goto CheckHiddenSingle;
		}

		for (int region = 0; region < 27; region++)
		{
			int count = 0, resultCell = -1;
			bool flag = true;
			foreach (int cell in RegionMaps[region])
			{
				if (grid.GetStatus(cell) == CellStatus.Empty)
				{
					resultCell = cell;
					if (++count > 1)
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag || count == 0)
			{
				continue;
			}

			int digit = TrailingZeroCount(grid.GetCandidates(resultCell));
			var step = new FullHouseStep(
				ImmutableArray.Create(new Conclusion(ConclusionType.Assignment, resultCell, digit)),
				ImmutableArray.Create(
					View.Empty
						+ new CandidateViewNode(0, resultCell * 9 + digit)
						+ new RegionViewNode(0, region)
				),
				resultCell,
				digit
			);

			if (onlyFindOne)
			{
				return step;
			}

			accumulator.Add(step);
		}
	CheckHiddenSingle:
		if (HiddenSinglesInBlockFirst)
		{
			// If block first, we'll extract all blocks and iterate on them firstly.
			for (int region = 0; region < 9; region++)
			{
				for (int digit = 0; digit < 9; digit++)
				{
					if (g(grid, digit, region) is not { } step)
					{
						continue;
					}

					if (onlyFindOne)
					{
						return step;
					}

					accumulator.Add(step);
				}
			}

			// Then secondly rows and columns.
			for (int region = 9; region < 27; region++)
			{
				for (int digit = 0; digit < 9; digit++)
				{
					if (g(grid, digit, region) is not { } step)
					{
						continue;
					}

					if (onlyFindOne)
					{
						return step;
					}

					accumulator.Add(step);
				}
			}
		}
		else
		{
			// We'll directly iterate on each region.
			// Theoretically, this iteration should be faster than above one, but in practice,
			// we may found hidden singles in block much more times than in row or column.
			for (int digit = 0; digit < 9; digit++)
			{
				for (int region = 0; region < 27; region++)
				{
					if (g(grid, digit, region) is not { } step)
					{
						continue;
					}

					if (onlyFindOne)
					{
						return step;
					}

					accumulator.Add(step);
				}
			}
		}

		for (int cell = 0; cell < 81; cell++)
		{
			if (grid.GetStatus(cell) != CellStatus.Empty)
			{
				continue;
			}

			short mask = grid.GetCandidates(cell);
			if (!IsPow2(mask))
			{
				continue;
			}

			int digit = TrailingZeroCount(mask);
			List<CrosshatchViewNode>? directLines = null;
			if (ShowDirectLines)
			{
				directLines = new(6);
				for (int i = 0; i < 9; i++)
				{
					if (digit != i)
					{
						bool flag = false;
						foreach (int peerCell in PeerMaps[cell])
						{
							if (grid[peerCell] == i)
							{
								directLines.Add(new(0, Cells.Empty + peerCell, Cells.Empty, digit));
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
				}
			}

			var step = new NakedSingleStep(
				ImmutableArray.Create(new Conclusion(ConclusionType.Assignment, cell, digit)),
				ImmutableArray.Create(View.Empty + new CandidateViewNode(0, cell * 9 + digit) + directLines),
				cell,
				digit
			);

			if (onlyFindOne)
			{
				return step;
			}

			accumulator.Add(step);
		}

		return null;


		Step? g(in Grid grid, int digit, int region)
		{
			// The main idea of hidden single is to search for a digit can only appear once in a region,
			// so we should check all possibilities in a region to found whether the region exists a digit
			// that only appears once indeed.
			int count = 0, resultCell = -1;
			bool flag = true;
			foreach (int cell in RegionMaps[region])
			{
				if (grid.Exists(cell, digit) is true)
				{
					resultCell = cell;
					if (++count > 1)
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag || count == 0)
			{
				// The digit has been filled into the region, or the digit
				// appears more than once, which means the digit is invalid case for hidden single.
				// Just skip it.
				return null;
			}

			// Now here the digit is a hidden single. We should gather the information
			// (painting or text information) on the step in order to display onto the UI.
			bool enableAndIsLastDigit = false;
			var cellOffsets = new List<CellViewNode>();
			if (EnableLastDigit)
			{
				// Sum up the number of appearing in the grid of 'digit'.
				int digitCount = 0;
				for (int i = 0; i < 81; i++)
				{
					if (grid[i] == digit)
					{
						digitCount++;
						cellOffsets.Add(new(0, i));
					}
				}

				enableAndIsLastDigit = digitCount == 8;
			}

			// Get direct lines.
			List<CrosshatchViewNode>? directLines = null;
			if (!enableAndIsLastDigit && ShowDirectLines)
			{
				directLines = new(6);

				// Step 1: Get all source cells that makes the result cell
				// can't be filled with the result digit.
				Cells crosshatchingCells = Cells.Empty, tempMap = Cells.Empty;
				foreach (int cell in RegionCells[region])
				{
					if (cell != resultCell && grid.GetStatus(cell) == CellStatus.Empty)
					{
						tempMap.Add(cell);
					}
				}
				foreach (int cell in tempMap)
				{
					foreach (int peerCell in PeerMaps[cell])
					{
						if (cell != resultCell && grid[peerCell] == digit)
						{
							crosshatchingCells.Add(peerCell);
						}
					}
				}

				// Step 2: Get all removed cells in this region.
				foreach (int cell in crosshatchingCells)
				{
					if ((PeerMaps[cell] & tempMap) is { Count: not 0 } removableCells)
					{
						directLines.Add(new(0, Cells.Empty + cell, removableCells, digit));
						tempMap -= removableCells;
					}
				}
			}

			return new HiddenSingleStep(
				ImmutableArray.Create(new Conclusion(ConclusionType.Assignment, resultCell, digit)),
				ImmutableArray.Create(
					View.Empty
						+ (enableAndIsLastDigit ? cellOffsets : null)
						+ new CandidateViewNode(0, resultCell * 9 + digit)
						+ (enableAndIsLastDigit ? null : new RegionViewNode(0, region))
						+ directLines
				),
				resultCell,
				digit,
				region,
				enableAndIsLastDigit
			);
		}
	}
}
