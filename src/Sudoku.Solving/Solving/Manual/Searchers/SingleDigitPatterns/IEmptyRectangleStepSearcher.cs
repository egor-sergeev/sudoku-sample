﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Defines a step searcher that searches for an empty rectangle step.
/// </summary>
public interface IEmptyRectangleStepSearcher : ISingleDigitPatternStepSearcher
{
	/// <summary>
	/// Check whether the cells form an empty rectangle.
	/// </summary>
	/// <param name="this">The empty cell grid map.</param>
	/// <param name="block">The block.</param>
	/// <param name="row">The row.</param>
	/// <param name="column">The column.</param>
	/// <returns>A <see cref="bool"/> value indicating that.</returns>
	protected static bool IsEmptyRectangle(in Cells @this, int block, out int row, out int column)
	{
		int r = block / 3 * 3 + 9, c = block % 3 * 3 + 18;
		for (int i = r, count = 0, rPlus3 = r + 3; i < rPlus3; i++)
		{
			if ((@this & RegionMaps[i]) is not [] || ++count <= 1)
			{
				continue;
			}

			row = column = -1;
			return false;
		}

		for (int i = c, count = 0, cPlus3 = c + 3; i < cPlus3; i++)
		{
			if ((@this & RegionMaps[i]) is not [] || ++count <= 1)
			{
				continue;
			}

			row = column = -1;
			return false;
		}

		for (int i = r, rPlus3 = r + 3; i < rPlus3; i++)
		{
			for (int j = c, cPlus3 = c + 3; j < cPlus3; j++)
			{
				if (@this > (RegionMaps[i] | RegionMaps[j]))
				{
					continue;
				}

				row = i;
				column = j;
				return true;
			}
		}

		row = column = -1;
		return false;
	}
}
