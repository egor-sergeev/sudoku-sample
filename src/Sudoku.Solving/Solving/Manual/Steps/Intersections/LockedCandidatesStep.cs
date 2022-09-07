﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Locked Candidates</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Digit">Indicates the digit used.</param>
/// <param name="BaseSet">Indicates the region that the current locked candidates forms.</param>
/// <param name="CoverSet">Indicates the region that the current locked candidates influences.</param>
public sealed record LockedCandidatesStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	int Digit,
	int BaseSet,
	int CoverSet
) : IntersectionStep(Conclusions, Views), IElementaryStep, IStepWithRank
{
	/// <inheritdoc/>
	public override decimal Difficulty => BaseSet < 9 ? 2.6M : 2.8M;

	/// <inheritdoc/>
	public int Rank => 0;

	/// <inheritdoc/>
	public override Technique TechniqueCode => BaseSet < 9 ? Technique.Pointing : Technique.Claiming;

	/// <inheritdoc/>
	public override TechniqueGroup TechniqueGroup => TechniqueGroup.LockedCandidates;

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Moderate;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Often;

	[FormatItem]
	internal string DigitStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Digit + 1).ToString();
	}

	[FormatItem]
	internal string BaseSetStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new RegionCollection(BaseSet).ToString();
	}

	[FormatItem]
	internal string CoverSetStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new RegionCollection(CoverSet).ToString();
	}
}