﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is an <b>Empty Rectangle Intersection Pair</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="StartCell">Indicates the start cell used.</param>
/// <param name="EndCell">Indicates the end cell used.</param>
/// <param name="Region">The region that forms the dual empty rectangle.</param>
/// <param name="Digit1">Indicates the digit 1 used in this pattern.</param>
/// <param name="Digit2">Indicates the digit 2 used in this pattern.</param>
public sealed record EmptyRectangleIntersectionPairStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	int StartCell,
	int EndCell,
	int Region,
	int Digit1,
	int Digit2
) : AlmostLockedSetsStep(Conclusions, Views)
{
	/// <inheritdoc/>
	public override decimal Difficulty => 6.0M;

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Fiendish;

	/// <inheritdoc/>
	public override TechniqueGroup TechniqueGroup => TechniqueGroup.EmptyRectangleIntersectionPair;

	/// <inheritdoc/>
	public override Technique TechniqueCode => Technique.EmptyRectangleIntersectionPair;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Sometimes;

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
	internal string StartCellStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Cells.Empty + StartCell).ToString();
	}

	[FormatItem]
	internal string EndCellStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Cells.Empty + EndCell).ToString();
	}

	[FormatItem]
	internal string RegionStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new RegionCollection(Region).ToString();
	}
}
