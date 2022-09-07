﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Unique Rectangle Type 3</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Digit1"><inheritdoc/></param>
/// <param name="Digit2"><inheritdoc/></param>
/// <param name="Cells"><inheritdoc/></param>
/// <param name="ExtraCells">Indicates the extra cells used.</param>
/// <param name="ExtraDigitsMask">Indicates the mask that contains all extra digits used.</param>
/// <param name="Region">Indicates the region used.</param>
/// <param name="IsAvoidable"><inheritdoc/></param>
/// <param name="AbsoluteOffset"><inheritdoc/></param>
/// <param name="IsNaked">
/// <para>
/// Indicates whether the current instance uses a naked subset instead of a hidden subset to form the type 3.
/// </para>
/// <para>
/// In the default case, due to not having implemented the hidden subset cases,
/// the argument always keeps the value <see langword="true"/>. The possible values are:
/// <list type="table">
/// <item>
/// <term><see langword="true"/></term>
/// <description>The type 3 is with a naked subset.</description>
/// </item>
/// <item>
/// <term><see langword="false"/></term>
/// <description>The type 3 is with a hidden subset.</description>
/// </item>
/// </list>
/// </para>
/// </param>
public sealed record UniqueRectangleType3Step(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	int Digit1,
	int Digit2,
	in Cells Cells,
	in Cells ExtraCells,
	short ExtraDigitsMask,
	int Region,
	bool IsAvoidable,
	int AbsoluteOffset,
	bool IsNaked = true
) : UniqueRectangleStep(
	Conclusions,
	Views,
	IsAvoidable ? Technique.AvoidableRectangleType3 : Technique.UniqueRectangleType3,
	Digit1,
	Digit2,
	Cells,
	IsAvoidable,
	AbsoluteOffset
)
{
	/// <inheritdoc/>
	public override decimal Difficulty =>
		(IsNaked ? 4.5M : 4.6M) // Base difficulty.
			+ .1M * PopCount((uint)ExtraDigitsMask); // Size difficulty.

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel => DifficultyLevel.Hard;

	/// <inheritdoc/>
	public override Rarity Rarity => Rarity.Sometimes;

	[FormatItem]
	internal string DigitsStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new DigitCollection(ExtraDigitsMask).ToString();
	}

	[FormatItem]
	internal string OnlyKeyword
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => IsNaked ? string.Empty : "only ";
	}

	[FormatItem]
	internal string OnlyKeywordZhCn
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => R["Only"]!;
	}

	[FormatItem]
	internal string RegionStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new RegionCollection(Region).ToString();
	}

	[FormatItem]
	internal string AppearLimitKeyword
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => R["Appears"]!;
	}
}
