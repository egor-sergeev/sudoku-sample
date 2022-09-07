﻿namespace Sudoku.Solving.Collections;

/// <summary>
/// Encapsulates a type that represents the reason why the exocet eliminations can be removed.
/// </summary>
[Flags]
public enum ExocetEliminatedReason : byte
{
	/// <summary>
	/// Indicates the elimination is the basic elimination (Target eliminations).
	/// </summary>
	Basic = 1,

	/// <summary>
	/// Indicates the target inference eliminations (that is eliminated via mirror cells).
	/// </summary>
	TargetInference = 2,

	/// <summary>
	/// Indicates the mirror eliminations.
	/// </summary>
	Mirror = 4,

	/// <summary>
	/// Indicates the bi-bi pattern.
	/// </summary>
	BiBiPattern = 8,

	/// <summary>
	/// Indicates the target pair eliminations.
	/// </summary>
	TargetPair = 16,

	/// <summary>
	/// Indicates the generalized swordfish eliminations.
	/// </summary>
	GeneralizedSwordfish = 32,

	/// <summary>
	/// Indicates the true base eliminations.
	/// </summary>
	TrueBase = 64,

	/// <summary>
	/// Indicates the compatibility test eliminations.
	/// </summary>
	CompatibilityTest = 128
}
