﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Bi-value Oddagon</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Loop">Indicates the loop used.</param>
/// <param name="Digit1">Indicates the first digit.</param>
/// <param name="Digit2">Indicates the second digit.</param>
public abstract record BivalueOddagonStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	in Cells Loop,
	int Digit1,
	int Digit2
) : RankTheoryStep(Conclusions, Views), IDistinctableStep<BivalueOddagonStep>, ILoopLikeStep, IStepWithRank
{
	/// <inheritdoc/>
	public bool? IsNice => null;

	/// <inheritdoc/>
	public override decimal Difficulty => 5.0M;

	/// <inheritdoc/>
	public int Rank => -1;

	/// <inheritdoc/>
	public sealed override TechniqueGroup TechniqueGroup => TechniqueGroup.BivalueOddagon;


	/// <inheritdoc/>
	public static bool Equals(BivalueOddagonStep left, BivalueOddagonStep right) =>
		left.GetType() == right.GetType()
		&& (1 << left.Digit1 | 1 << left.Digit2) == (1 << right.Digit1 | 1 << right.Digit2)
		&& left.Loop == right.Loop;
}
