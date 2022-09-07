﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is an <b>Almost Locked Sets XZ</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Als1">Indicates the first ALS used in this pattern.</param>
/// <param name="Als2">Indicates the second ALS used in this pattern.</param>
/// <param name="XDigitsMask">Indicates the X digit used in this ALS-XZ pattern.</param>
/// <param name="ZDigitsMask">Indicates the Z digit used in this ALS-XZ pattern.</param>
/// <param name="IsDoublyLinked">Indicates whether the ALS-XZ is doubly-linked.</param>
public sealed record AlmostLockedSetsXzStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	in AlmostLockedSet Als1,
	in AlmostLockedSet Als2,
	short XDigitsMask,
	short ZDigitsMask,
	bool? IsDoublyLinked
) : AlmostLockedSetsStep(Conclusions, Views)
{
	/// <inheritdoc/>
	public override decimal Difficulty => IsDoublyLinked is true ? 5.7M : 5.5M;

	/// <inheritdoc/>
	public override string? Format =>
		R[
			IsDoublyLinked is null
				? ZDigitsMask == 0
					? "TechniqueFormat_ExtendedSubsetPrincipleWithoutDuplicate"
					: "TechniqueFormat_ExtendedSubsetPrincipleWithDuplicate"
				: "TechniqueFormat_AlmostLockedSetsXzRule"
		];

	/// <inheritdoc/>
	public override TechniqueTags TechniqueTags => base.TechniqueTags | TechniqueTags.ShortChaining;

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Fiendish;

	/// <inheritdoc/>
	public override Technique TechniqueCode =>
		IsDoublyLinked switch
		{
			true => Technique.DoublyLinkedAlmostLockedSetsXzRule,
			false => Technique.SinglyLinkedAlmostLockedSetsXzRule,
			_ => Technique.ExtendedSubsetPrinciple
		};

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Often;

	[FormatItem]
	internal string CellsStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Als1.Map | Als2.Map).ToString();
	}

	[FormatItem]
	internal string EspDigitStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (TrailingZeroCount(ZDigitsMask) + 1).ToString();
	}

	[FormatItem]
	internal string Als1Str
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Als1.ToString();
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
	internal string ZResultStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ZDigitsMask != 0 ? $", Z = {new DigitCollection(ZDigitsMask).ToString()}" : string.Empty;
	}
}
