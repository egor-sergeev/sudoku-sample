﻿namespace Sudoku.Solving.Manual;

/// <summary>
/// Indicates a reason why the searcher is disabled.
/// </summary>
[Flags]
public enum DisabledReason : short
{
	/// <summary>
	/// Indicates the searcher is normal one and isn't disabled.
	/// </summary>
	None = 0,

	/// <summary>
	/// Indicates the searcher searches for last resorts, which don't need to show.
	/// </summary>
	LastResort = 1,

	/// <summary>
	/// Indicates the searcher has bugs while searching.
	/// </summary>
	HasBugs = 2,

	/// <summary>
	/// Indicates the searcher runs so slowly that the author himself can't stand to use it.
	/// </summary>
	TooSlow = 4,

	/// <summary>
	/// Indicates the searcher has not been implemented, or has been deprecated.
	/// </summary>
	DeprecatedOrNotImplemented = 8
}
