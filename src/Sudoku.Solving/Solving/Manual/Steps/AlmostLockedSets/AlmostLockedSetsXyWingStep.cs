﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is an <b>Almost Locked Sets XY-Wing</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Als1">Indicates the first ALS used in this pattern.</param>
/// <param name="Als2">Indicates the second ALS used in this pattern.</param>
/// <param name="Bridge">Indicates the ALS that is as a bridge.</param>
/// <param name="XDigitsMask">Indicates the mask that holds the digits for the X value.</param>
/// <param name="YDigitsMask">Indicates the mask that holds the digits for the Y value.</param>
/// <param name="ZDigitsMask">Indicates the mask that holds the digits for the Z value.</param>
public sealed record AlmostLockedSetsXyWingStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	in AlmostLockedSet Als1,
	in AlmostLockedSet Als2,
	in AlmostLockedSet Bridge,
	short XDigitsMask,
	short YDigitsMask,
	short ZDigitsMask
) : AlmostLockedSetsStep(Conclusions, Views)
{
	/// <inheritdoc/>
	public override decimal Difficulty => 6.0M;

	/// <inheritdoc/>
	public override TechniqueTags TechniqueTags => base.TechniqueTags | TechniqueTags.ShortChaining;

	/// <inheritdoc/>
	public override TechniqueGroup TechniqueGroup => TechniqueGroup.AlmostLockedSetsChainingLike;

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Fiendish;

	/// <inheritdoc/>
	public override Technique TechniqueCode => Technique.AlmostLockedSetsXyWing;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Sometimes;

	[FormatItem]
	internal string Als1Str
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Als1.ToString();
	}

	[FormatItem]
	internal string BridgeStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Bridge.ToString();
	}

	[FormatItem]
	internal string Als2Str
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Als2.ToString();
	}

	[FormatItem]
	internal string XStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new DigitCollection(XDigitsMask).ToString();
	}

	[FormatItem]
	internal string YStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new DigitCollection(YDigitsMask).ToString();
	}

	[FormatItem]
	internal string ZStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new DigitCollection(ZDigitsMask).ToString();
	}
}
