﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Bi-value Oddagon Type 2</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Loop"><inheritdoc/></param>
/// <param name="Digit1"><inheritdoc/></param>
/// <param name="Digit2"><inheritdoc/></param>
/// <param name="ExtraDigit">Indicates the extra digit.</param>
public sealed record BivalueOddagonType2Step(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	in Cells Loop,
	int Digit1,
	int Digit2,
	int ExtraDigit
) : BivalueOddagonStep(Conclusions, Views, Loop, Digit1, Digit2)
{
	/// <inheritdoc/>
	public override decimal Difficulty => base.Difficulty + .1M;

	/// <inheritdoc/>
	public override Technique TechniqueCode => Technique.BivalueOddagonType2;

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Fiendish;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Seldom;

	[FormatItem]
	internal string ExtraDigitStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (ExtraDigit + 1).ToString();
	}

	[FormatItem]
	internal string LoopStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Loop.ToString();
	}
}
