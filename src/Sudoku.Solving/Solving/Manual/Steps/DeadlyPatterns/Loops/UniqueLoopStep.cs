﻿using U3 = Sudoku.Solving.Manual.Steps.UniqueLoopType3Step;
using U4 = Sudoku.Solving.Manual.Steps.UniqueLoopType4Step;

namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Unique Loop</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Digit1">Indicates the first digit.</param>
/// <param name="Digit2">Indicates the second digit.</param>
/// <param name="Loop">Indicates the loop that the instance used.</param>
public abstract record UniqueLoopStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	int Digit1,
	int Digit2,
	in Cells Loop
) : DeadlyPatternStep(Conclusions, Views), IDistinctableStep<UniqueLoopStep>
{
	/// <inheritdoc/>
	public override decimal Difficulty =>
		Type switch
		{
			1 or 3 => 4.5M,
			2 or 4 => 4.6M,
			_ => throw new NotSupportedException("The specified type is not supported.")
		} // Type difficulty.
			+ ((Loop.Count >> 1) - 3) * .1M; // Length difficulty.

	/// <summary>
	/// Indicates the type.
	/// </summary>
	public abstract int Type { get; }

	/// <inheritdoc/>
	public sealed override Technique TechniqueCode => Enum.Parse<Technique>($"UniqueLoopType{Type}");

	/// <inheritdoc/>
	public sealed override TechniqueGroup TechniqueGroup => TechniqueGroup.UniqueLoop;

	/// <inheritdoc/>
	public sealed override TechniqueTags TechniqueTags => TechniqueTags.DeadlyPattern;

	/// <inheritdoc/>
	public sealed override DifficultyLevel DifficultyLevel => DifficultyLevel.Hard;

	/// <summary>
	/// Indicates the loop string.
	/// </summary>
	[FormatItem]
	internal string LoopStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Loop.ToString();
	}

	/// <summary>
	/// Indicates the digit 1 string.
	/// </summary>
	[FormatItem]
	internal string Digit1Str
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Digit1 + 1).ToString();
	}

	/// <summary>
	/// Indicates the digit 2 string.
	/// </summary>
	[FormatItem]
	internal string Digit2Str
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (Digit2 + 1).ToString();
	}


	/// <inheritdoc/>
	public static bool Equals(UniqueLoopStep left, UniqueLoopStep right) =>
		left.Type == right.Type && left.Loop == right.Loop
			&& (1 << left.Digit1 | 1 << left.Digit2) == (1 << right.Digit1 | 1 << right.Digit2)
			&& (left, right) switch
			{
				(U3 { SubsetDigitsMask: var a }, U3 { SubsetDigitsMask: var b }) => a == b,
				(U4 { ConjugatePair: var a }, U4 { ConjugatePair: var b }) => a == b,
				_ => true
			};
}
