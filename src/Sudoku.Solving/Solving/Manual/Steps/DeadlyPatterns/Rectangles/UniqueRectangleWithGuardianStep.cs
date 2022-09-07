﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Unique Rectangle with Guardians</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Digit1"><inheritdoc/></param>
/// <param name="Digit2"><inheritdoc/></param>
/// <param name="Cells"><inheritdoc/></param>
/// <param name="GuardianCells">Indicates the cells that the guardians lie in.</param>
/// <param name="GuardianDigit">Indicates the digit that the guardians are used.</param>
/// <param name="IsIncomplete">Indicates whether the rectangle is incomplete.</param>
/// <param name="AbsoluteOffset"><inheritdoc/></param>
public sealed record UniqueRectangleWithGuardianStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	int Digit1,
	int Digit2,
	in Cells Cells,
	in Cells GuardianCells,
	int GuardianDigit,
	bool IsIncomplete,
	int AbsoluteOffset
) : UniqueRectangleStep(
	Conclusions,
	Views,
	Technique.UniqueRectangleBrokenWing,
	Digit1,
	Digit2,
	Cells,
	false,
	AbsoluteOffset
)
{
	/// <inheritdoc/>
	public override decimal Difficulty =>
		4.5M
			+ .1M * (GuardianCells.Count >> 1) // Guardian count difficulty.
			+ (IsIncomplete ? .1M : 0); // Incompleteness difficulty.

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Fiendish;

	/// <inheritdoc/>
	public override TechniqueGroup TechniqueGroup => TechniqueGroup.UniqueRectanglePlus;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Often;

	[FormatItem]
	internal string GuardianDigitStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (GuardianDigit + 1).ToString();
	}

	[FormatItem]
	internal string GuardianCellsStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => GuardianCells.ToString();
	}
}
