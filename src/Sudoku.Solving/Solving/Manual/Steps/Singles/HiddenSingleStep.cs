﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Hidden Single</b> or <b>Last Digit</b> (for special cases) technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Cell"><inheritdoc/></param>
/// <param name="Digit"><inheritdoc/></param>
/// <param name="Region">Indicates the region used.</param>
/// <param name="EnableAndIsLastDigit">
/// Indicates whether the current step is a <b>Last Digit</b> technique usage.
/// </param>
public sealed record HiddenSingleStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	int Cell,
	int Digit,
	int Region,
	bool EnableAndIsLastDigit
) : SingleStep(Conclusions, Views, Cell, Digit)
{
	/// <inheritdoc/>
	public override decimal Difficulty => EnableAndIsLastDigit ? 1.1M : Region < 9 ? 1.2M : 1.5M;

	/// <inheritdoc/>
	public override Rarity Rarity => EnableAndIsLastDigit || Region < 9 ? Rarity.Always : Rarity.Often;

	/// <inheritdoc/>
	public override Technique TechniqueCode =>
		EnableAndIsLastDigit
			? Technique.LastDigit
			: (Technique)((int)Technique.HiddenSingleBlock + (int)Region.ToRegion());

	/// <inheritdoc/>
	public override string? Format =>
		R[EnableAndIsLastDigit ? "TechniqueFormat_LastDigit" : "TechniqueFormat_HiddenSingle"];

	[FormatItem]
	internal string CellStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Cells.Empty + Cell).ToString();
	}

	[FormatItem]
	internal string DigitStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Digit + 1).ToString();
	}

	[FormatItem]
	internal string RegionStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new RegionCollection(Region).ToString();
	}
}
