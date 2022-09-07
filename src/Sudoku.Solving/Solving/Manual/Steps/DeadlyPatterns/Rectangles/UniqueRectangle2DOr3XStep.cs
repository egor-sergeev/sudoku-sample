﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Unique Rectangle 2D (or 3X)</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="TechniqueCode2"><inheritdoc/></param>
/// <param name="Digit1"><inheritdoc/></param>
/// <param name="Digit2"><inheritdoc/></param>
/// <param name="Cells"><inheritdoc/></param>
/// <param name="IsAvoidable"><inheritdoc/></param>
/// <param name="XDigit">Indicates the digit X.</param>
/// <param name="YDigit">Indicates the digit Y.</param>
/// <param name="XyCell">Indicates the cell XY.</param>
/// <param name="AbsoluteOffset"><inheritdoc/></param>
public sealed record UniqueRectangle2DOr3XStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	Technique TechniqueCode2,
	int Digit1,
	int Digit2,
	in Cells Cells,
	bool IsAvoidable,
	int XDigit,
	int YDigit,
	int XyCell,
	int AbsoluteOffset
) : UniqueRectangleStep(Conclusions, Views, TechniqueCode2, Digit1, Digit2, Cells, IsAvoidable, AbsoluteOffset)
{
	/// <inheritdoc/>
	public override decimal Difficulty => 4.7M;

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Hard;

	/// <inheritdoc/>
	public override TechniqueGroup TechniqueGroup => TechniqueGroup.UniqueRectanglePlus;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.HardlyEver;

	[FormatItem]
	internal string XDigitStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (XDigit + 1).ToString();
	}

	[FormatItem]
	internal string YDigitStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (YDigit + 1).ToString();
	}

	[FormatItem]
	internal string XYCellsStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Cells.Empty + XyCell).ToString();
	}
}
