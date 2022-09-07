﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Bowman's Bingo</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="ContradictionLinks">Indicates the list of contradiction links.</param>
public sealed record BowmanBingoStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	ImmutableArray<Conclusion> ContradictionLinks
) : LastResortStep(Conclusions, Views), IChainLikeStep
{
	/// <inheritdoc/>
	public override decimal Difficulty =>
		8.0M // Base difficulty.
			+ IChainLikeStep.GetExtraDifficultyByLength(ContradictionLinks.Length); // Length difficulty.

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.LastResort;

	/// <inheritdoc/>
	public override TechniqueTags TechniqueTags =>
		base.TechniqueTags | TechniqueTags.LongChaining | TechniqueTags.ForcingChains;

	/// <inheritdoc/>
	public override Technique TechniqueCode => Technique.BowmanBingo;

	/// <inheritdoc/>
	public override TechniqueGroup TechniqueGroup => TechniqueGroup.BowmanBingo;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Often;

	[FormatItem]
	internal string ContradictionSeriesStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new ConclusionCollection(ContradictionLinks.ToArray()).ToString(false, " -> ");
	}
}
