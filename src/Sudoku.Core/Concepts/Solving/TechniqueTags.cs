﻿namespace Sudoku.Concepts.Solving;

/// <summary>
/// Provides a series of tags to mark on a technique.
/// </summary>
/// <remarks>
/// For example, a <see cref="Technique.DeathBlossom"/> can be categorized
/// as both <see cref="Als"/> and <see cref="LongChaining"/>.
/// </remarks>
/// <seealso cref="Als"/>
/// <seealso cref="LongChaining"/>
[Flags]
public enum TechniqueTags
{
	/// <summary>
	/// Indicates none of flags that the technique belongs to.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	None = 0,

	/// <summary>
	/// Indicates the singles technique.
	/// </summary>
	Singles = 1,

	/// <summary>
	/// Indicates the intersection technique.
	/// </summary>
	Intersections = 2,

	/// <summary>
	/// Indicates the subset technique. Please note that all ALS techniques shouldn't be with this flag.
	/// </summary>
	Subsets = 4,

	/// <summary>
	/// Indicates the fish technique.
	/// </summary>
	Fishes = 8,

	/// <summary>
	/// Indicates the wing technique.
	/// </summary>
	Wings = 16,

	/// <summary>
	/// Indicates the single digit pattern technique.
	/// </summary>
	SingleDigitPattern = 32,

	/// <summary>
	/// Indicates the short chain.
	/// </summary>
	ShortChaining = 64,

	/// <summary>
	/// Indicates the long chain, which includes normal AICs, forcing chains
	/// and other chaining-like techniques, such as Bowman's Bingo.
	/// </summary>
	LongChaining = 128,

	/// <summary>
	/// Indicates the forcing chains technique, such as Bowman's Bingo, Region Forcing Chains and so on.
	/// </summary>
	ForcingChains = 256,

	/// <summary>
	/// Indicates the deadly pattern technique.
	/// </summary>
	DeadlyPattern = 512,

	/// <summary>
	/// Indicates the ALS technique.
	/// </summary>
	Als = 1024,

	/// <summary>
	/// Indicates the MSLS technique.
	/// </summary>
	Msls = 2048,

	/// <summary>
	/// Indicates the exocet technique.
	/// </summary>
	Exocet = 4096,

	/// <summary>
	/// Indicates the technique checked and searched relies on the rank theory.
	/// </summary>
	RankTheory = 8192,

	/// <summary>
	/// Indicates the symmetry technique.
	/// </summary>
	Symmetry = 16384,

	/// <summary>
	/// Indicates the last resort technique.
	/// </summary>
	LastResort = 32768
}
