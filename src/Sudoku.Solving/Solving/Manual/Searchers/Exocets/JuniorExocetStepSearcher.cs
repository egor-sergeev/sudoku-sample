﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Provides with a <b>Junior Exocet</b> step searcher.
/// The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>Junior Exocet</item>
/// </list>
/// </summary>
[StepSearcher]
public sealed unsafe partial class JuniorExocetStepSearcher : IJuniorExocetStepSearcher
{
	/// <inheritdoc/>
	public bool CheckAdvanced { get; set; }


	/// <inheritdoc/>
	public Step? GetAll(ICollection<Step> accumulator, in Grid grid, bool onlyFindOne)
	{
		// TODO: Re-implement JE.
		return null;
	}
}
