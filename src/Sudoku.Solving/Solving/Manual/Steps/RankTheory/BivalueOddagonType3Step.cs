﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Bi-value Oddagon Type 3</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Loop"><inheritdoc/></param>
/// <param name="Digit1"><inheritdoc/></param>
/// <param name="Digit2"><inheritdoc/></param>
/// <param name="ExtraCells">Indicates the extra cells used.</param>
/// <param name="ExtraDigitsMask">Indicates the mask that contains all extra digits used.</param>
public sealed record BivalueOddagonType3Step(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	in Cells Loop,
	int Digit1,
	int Digit2,
	in Cells ExtraCells,
	short ExtraDigitsMask
) : BivalueOddagonStep(Conclusions, Views, Loop, Digit1, Digit2)
{
	/// <inheritdoc/>
	public override decimal Difficulty => base.Difficulty + (ExtraCells.Count >> 1) * .1M;

	/// <inheritdoc/>
	public override Technique TechniqueCode => Technique.BivalueOddagonType3;

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Fiendish;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Seldom;

	[FormatItem]
	internal string LoopStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Loop.ToString();
	}

	[FormatItem]
	internal string Digit1Str
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Digit1 + 1).ToString();
	}

	[FormatItem]
	internal string Digit2Str
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Digit2 + 1).ToString();
	}

	[FormatItem]
	internal string DigitsStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new DigitCollection(ExtraDigitsMask).ToString();
	}

	[FormatItem]
	internal string ExtraCellsStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ExtraCells.ToString();
	}
}
