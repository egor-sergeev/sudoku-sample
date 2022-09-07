﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Qiu's Deadly Pattern Type 4</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Pattern"><inheritdoc/></param>
/// <param name="ConjugatePair">Indicates the conjugate pair used.</param>
public sealed record QiuDeadlyPatternType4Step(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	in QiuDeadlyPattern Pattern,
	in ConjugatePair ConjugatePair
) : QiuDeadlyPatternStep(Conclusions, Views, Pattern)
{
	/// <inheritdoc/>
	public override decimal Difficulty => base.Difficulty + .2M;

	/// <inheritdoc/>
	public override int Type => 4;

	[FormatItem]
	internal string ConjStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ConjugatePair.ToString();
	}
}
