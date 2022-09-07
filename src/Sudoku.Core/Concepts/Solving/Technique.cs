﻿namespace Sudoku.Concepts.Solving;

/// <summary>
/// Represents a technique instance, which is used for comparison.
/// </summary>
public enum Technique : short
{
	/// <summary>
	/// The placeholder of this enumeration type.
	/// </summary>
	None,

	/// <summary>
	/// Indicates the full house.
	/// </summary>
	FullHouse,

	/// <summary>
	/// Indicates the last digit.
	/// </summary>
	LastDigit,

	/// <summary>
	/// Indicates the hidden single (in block).
	/// </summary>
	HiddenSingleBlock,

	/// <summary>
	/// Indicates the hidden single (in row).
	/// </summary>
	HiddenSingleRow,

	/// <summary>
	/// Indicates the hidden single (in column).
	/// </summary>
	HiddenSingleColumn,

	/// <summary>
	/// Indicates the naked single.
	/// </summary>
	NakedSingle,

	/// <summary>
	/// Indicates the pointing.
	/// </summary>
	Pointing,

	/// <summary>
	/// Indicates the claiming.
	/// </summary>
	Claiming,

	/// <summary>
	/// Indicates the almost locked pair.
	/// </summary>
	AlmostLockedPair,

	/// <summary>
	/// Indicates the almost locked triple.
	/// </summary>
	AlmostLockedTriple,

	/// <summary>
	/// Indicates the almost locked quadruple.
	/// The technique may not be useful because it'll be replaced with Sue de Coq.
	/// </summary>
	AlmostLockedQuadruple,

	/// <summary>
	/// Indicates the naked pair.
	/// </summary>
	NakedPair,

	/// <summary>
	/// Indicates the naked pair plus (naked pair (+)).
	/// </summary>
	NakedPairPlus,

	/// <summary>
	/// Indicates the locked pair.
	/// </summary>
	LockedPair,

	/// <summary>
	/// Indicates the hidden pair.
	/// </summary>
	HiddenPair,

	/// <summary>
	/// Indicates the naked triple.
	/// </summary>
	NakedTriple,

	/// <summary>
	/// Indicates the naked triple plus (naked triple (+)).
	/// </summary>
	NakedTriplePlus,

	/// <summary>
	/// Indicates the locked triple.
	/// </summary>
	LockedTriple,

	/// <summary>
	/// Indicates the hidden triple.
	/// </summary>
	HiddenTriple,

	/// <summary>
	/// Indicates the naked quadruple.
	/// </summary>
	NakedQuadruple,

	/// <summary>
	/// Indicates the naked quadruple plus (naked quadruple (+)).
	/// </summary>
	NakedQuadruplePlus,

	/// <summary>
	/// Indicates the hidden quadruple.
	/// </summary>
	HiddenQuadruple,

	/// <summary>
	/// Indicates the X-Wing.
	/// </summary>
	XWing,

	/// <summary>
	/// Indicates the finned X-Wing.
	/// </summary>
	FinnedXWing,

	/// <summary>
	/// Indicates the sashimi X-Wing.
	/// </summary>
	SashimiXWing,

	/// <summary>
	/// Indicates the siamese finned X-Wing.
	/// </summary>
	SiameseFinnedXWing,

	/// <summary>
	/// Indicates the siamese sashimi X-Wing.
	/// </summary>
	SiameseSashimiXWing,

	/// <summary>
	/// Indicates the franken X-Wing.
	/// </summary>
	FrankenXWing,

	/// <summary>
	/// Indicates the finned franken X-Wing.
	/// </summary>
	FinnedFrankenXWing,

	/// <summary>
	/// Indicates the sashimi franken X-Wing.
	/// </summary>
	SashimiFrankenXWing,

	/// <summary>
	/// Indicates the siamese finned franken X-Wing.
	/// </summary>
	SiameseFinnedFrankenXWing,

	/// <summary>
	/// Indicates the siamese sashimi franken X-Wing.
	/// </summary>
	SiameseSashimiFrankenXWing,

	/// <summary>
	/// Indicates the mutant X-Wing.
	/// </summary>
	MutantXWing,

	/// <summary>
	/// Indicates the finned mutant X-Wing.
	/// </summary>
	FinnedMutantXWing,

	/// <summary>
	/// Indicates the sashimi mutant X-Wing.
	/// </summary>
	SashimiMutantXWing,

	/// <summary>
	/// Indicates the siamese finned mutant X-Wing.
	/// </summary>
	SiameseFinnedMutantXWing,

	/// <summary>
	/// Indicates the siamese sashimi mutant X-Wing.
	/// </summary>
	SiameseSashimiMutantXWing,

	/// <summary>
	/// Indicates the swordfish.
	/// </summary>
	Swordfish,

	/// <summary>
	/// Indicates the finned swordfish.
	/// </summary>
	FinnedSwordfish,

	/// <summary>
	/// Indicates the sashimi swordfish.
	/// </summary>
	SashimiSwordfish,

	/// <summary>
	/// Indicates the siamese finned swordfish.
	/// </summary>
	SiameseFinnedSwordfish,

	/// <summary>
	/// Indicates the siamese sashimi swordfish.
	/// </summary>
	SiameseSashimiSwordfish,

	/// <summary>
	/// Indicates the swordfish.
	/// </summary>
	FrankenSwordfish,

	/// <summary>
	/// Indicates the finned franken swordfish.
	/// </summary>
	FinnedFrankenSwordfish,

	/// <summary>
	/// Indicates the sashimi franken swordfish.
	/// </summary>
	SashimiFrankenSwordfish,

	/// <summary>
	/// Indicates the siamese finned franken swordfish.
	/// </summary>
	SiameseFinnedFrankenSwordfish,

	/// <summary>
	/// Indicates the siamese sashimi franken swordfish.
	/// </summary>
	SiameseSashimiFrankenSwordfish,

	/// <summary>
	/// Indicates the mutant swordfish.
	/// </summary>
	MutantSwordfish,

	/// <summary>
	/// Indicates the finned mutant swordfish.
	/// </summary>
	FinnedMutantSwordfish,

	/// <summary>
	/// Indicates the sashimi mutant swordfish.
	/// </summary>
	SashimiMutantSwordfish,

	/// <summary>
	/// Indicates the siamese finned mutant swordfish.
	/// </summary>
	SiameseFinnedMutantSwordfish,

	/// <summary>
	/// Indicates the siamese sashimi mutant swordfish.
	/// </summary>
	SiameseSashimiMutantSwordfish,

	/// <summary>
	/// Indicates the jellyfish.
	/// </summary>
	Jellyfish,

	/// <summary>
	/// Indicates the finned jellyfish.
	/// </summary>
	FinnedJellyfish,

	/// <summary>
	/// Indicates the sashimi jellyfish.
	/// </summary>
	SashimiJellyfish,

	/// <summary>
	/// Indicates the siamese finned jellyfish.
	/// </summary>
	SiameseFinnedJellyfish,

	/// <summary>
	/// Indicates the siamese sashimi jellyfish.
	/// </summary>
	SiameseSashimiJellyfish,

	/// <summary>
	/// Indicates the franken jellyfish.
	/// </summary>
	FrankenJellyfish,

	/// <summary>
	/// Indicates the finned franken jellyfish.
	/// </summary>
	FinnedFrankenJellyfish,

	/// <summary>
	/// Indicates the sashimi franken jellyfish.
	/// </summary>
	SashimiFrankenJellyfish,

	/// <summary>
	/// Indicates the siamese finned franken jellyfish.
	/// </summary>
	SiameseFinnedFrankenJellyfish,

	/// <summary>
	/// Indicates the siamese sashimi franken jellyfish.
	/// </summary>
	SiameseSashimiFrankenJellyfish,

	/// <summary>
	/// Indicates the mutant jellyfish.
	/// </summary>
	MutantJellyfish,

	/// <summary>
	/// Indicates the finned mutant jellyfish.
	/// </summary>
	FinnedMutantJellyfish,

	/// <summary>
	/// Indicates the sashimi mutant jellyfish.
	/// </summary>
	SashimiMutantJellyfish,

	/// <summary>
	/// Indicates the siamese finned mutant jellyfish.
	/// </summary>
	SiameseFinnedMutantJellyfish,

	/// <summary>
	/// Indicates the siamese sashimi mutant jellyfish.
	/// </summary>
	SiameseSashimiMutantJellyfish,

	/// <summary>
	/// Indicates the squirmbag.
	/// </summary>
	Squirmbag,

	/// <summary>
	/// Indicates the finned squirmbag.
	/// </summary>
	FinnedSquirmbag,

	/// <summary>
	/// Indicates the sashimi squirmbag.
	/// </summary>
	SashimiSquirmbag,

	/// <summary>
	/// Indicates the siamese finned squirmbag.
	/// </summary>
	SiameseFinnedSquirmbag,

	/// <summary>
	/// Indicates the siamese sashimi squirmbag.
	/// </summary>
	SiameseSashimiSquirmbag,

	/// <summary>
	/// Indicates the franken squirmbag.
	/// </summary>
	FrankenSquirmbag,

	/// <summary>
	/// Indicates the finned franken squirmbag.
	/// </summary>
	FinnedFrankenSquirmbag,

	/// <summary>
	/// Indicates the sashimi franken squirmbag.
	/// </summary>
	SashimiFrankenSquirmbag,

	/// <summary>
	/// Indicates the siamese finned franken squirmbag.
	/// </summary>
	SiameseFinnedFrankenSquirmbag,

	/// <summary>
	/// Indicates the siamese sashimi franken squirmbag.
	/// </summary>
	SiameseSashimiFrankenSquirmbag,

	/// <summary>
	/// Indicates the mutant squirmbag.
	/// </summary>
	MutantSquirmbag,

	/// <summary>
	/// Indicates the finned mutant squirmbag.
	/// </summary>
	FinnedMutantSquirmbag,

	/// <summary>
	/// Indicates the sashimi mutant squirmbag.
	/// </summary>
	SashimiMutantSquirmbag,

	/// <summary>
	/// Indicates the siamese finned mutant squirmbag.
	/// </summary>
	SiameseFinnedMutantSquirmbag,

	/// <summary>
	/// Indicates the siamese sashimi mutant squirmbag.
	/// </summary>
	SiameseSashimiMutantSquirmbag,

	/// <summary>
	/// Indicates the whale.
	/// </summary>
	Whale,

	/// <summary>
	/// Indicates the finned whale.
	/// </summary>
	FinnedWhale,

	/// <summary>
	/// Indicates the sashimi whale.
	/// </summary>
	SashimiWhale,

	/// <summary>
	/// Indicates the siamese finned whale.
	/// </summary>
	SiameseFinnedWhale,

	/// <summary>
	/// Indicates the siamese sashimi whale.
	/// </summary>
	SiameseSashimiWhale,

	/// <summary>
	/// Indicates the franken whale.
	/// </summary>
	FrankenWhale,

	/// <summary>
	/// Indicates the finned franken whale.
	/// </summary>
	FinnedFrankenWhale,

	/// <summary>
	/// Indicates the sashimi franken whale.
	/// </summary>
	SashimiFrankenWhale,

	/// <summary>
	/// Indicates the siamese finned franken whale.
	/// </summary>
	SiameseFinnedFrankenWhale,

	/// <summary>
	/// Indicates the siamese sashimi franken whale.
	/// </summary>
	SiameseSashimiFrankenWhale,

	/// <summary>
	/// Indicates the mutant whale.
	/// </summary>
	MutantWhale,

	/// <summary>
	/// Indicates the finned mutant whale.
	/// </summary>
	FinnedMutantWhale,

	/// <summary>
	/// Indicates the sashimi mutant whale.
	/// </summary>
	SashimiMutantWhale,

	/// <summary>
	/// Indicates the siamese finned mutant whale.
	/// </summary>
	SiameseFinnedMutantWhale,

	/// <summary>
	/// Indicates the siamese sashimi mutant whale.
	/// </summary>
	SiameseSashimiMutantWhale,

	/// <summary>
	/// Indicates the leviathan.
	/// </summary>
	Leviathan,

	/// <summary>
	/// Indicates the finned leviathan.
	/// </summary>
	FinnedLeviathan,

	/// <summary>
	/// Indicates the sashimi leviathan.
	/// </summary>
	SashimiLeviathan,

	/// <summary>
	/// Indicates the siamese finned leviathan.
	/// </summary>
	SiameseFinnedLeviathan,

	/// <summary>
	/// Indicates the siamese sashimi leviathan.
	/// </summary>
	SiameseSashimiLeviathan,

	/// <summary>
	/// Indicates the franken leviathan.
	/// </summary>
	FrankenLeviathan,

	/// <summary>
	/// Indicates the finned franken leviathan.
	/// </summary>
	FinnedFrankenLeviathan,

	/// <summary>
	/// Indicates the sashimi franken leviathan.
	/// </summary>
	SashimiFrankenLeviathan,

	/// <summary>
	/// Indicates the siamese finned franken leviathan.
	/// </summary>
	SiameseFinnedFrankenLeviathan,

	/// <summary>
	/// Indicates the siamese sashimi franken leviathan.
	/// </summary>
	SiameseSashimiFrankenLeviathan,

	/// <summary>
	/// Indicates the mutant leviathan.
	/// </summary>
	MutantLeviathan,

	/// <summary>
	/// Indicates the finned mutant leviathan.
	/// </summary>
	FinnedMutantLeviathan,

	/// <summary>
	/// Indicates the sashimi mutant leviathan.
	/// </summary>
	SashimiMutantLeviathan,

	/// <summary>
	/// Indicates the siamese finned mutant leviathan.
	/// </summary>
	SiameseFinnedMutantLeviathan,

	/// <summary>
	/// Indicates the siamese sashimi mutant leviathan.
	/// </summary>
	SiameseSashimiMutantLeviathan,

	/// <summary>
	/// Indicates the XY-Wing.
	/// </summary>
	XyWing,

	/// <summary>
	/// Indicates the XYZ-Wing.
	/// </summary>
	XyzWing,

	/// <summary>
	/// Indicates the WXYZ-Wing.
	/// </summary>
	WxyzWing,

	/// <summary>
	/// Indicates the VWXYZ-Wing.
	/// </summary>
	VwxyzWing,

	/// <summary>
	/// Indicates the UVWXYZ-Wing.
	/// </summary>
	UvwxyzWing,

	/// <summary>
	/// Indicates the TUVWXYZ-Wing.
	/// </summary>
	TuvwxyzWing,

	/// <summary>
	/// Indicates the STUVWXYZ-Wing.
	/// </summary>
	StuvwxyzWing,

	/// <summary>
	/// Indicates the RSTUVWXYZ-Wing.
	/// </summary>
	RstuvwxyzWing,

	/// <summary>
	/// Indicates the incomplete WXYZ-Wing.
	/// </summary>
	IncompleteWxyzWing,

	/// <summary>
	/// Indicates the incomplete VWXYZ-Wing.
	/// </summary>
	IncompleteVwxyzWing,

	/// <summary>
	/// Indicates the incomplete UVWXYZ-Wing.
	/// </summary>
	IncompleteUvwxyzWing,

	/// <summary>
	/// Indicates the incomplete TUVWXYZ-Wing.
	/// </summary>
	IncompleteTuvwxyzWing,

	/// <summary>
	/// Indicates the incomplete STUVWXYZ-Wing.
	/// </summary>
	IncompleteStuvwxyzWing,

	/// <summary>
	/// Indicates the incomplete RSTUVWXYZ-Wing.
	/// </summary>
	IncompleteRstuvwxyzWing,

	/// <summary>
	/// Indicates the W-Wing.
	/// </summary>
	WWing,

	/// <summary>
	/// Indicates the M-Wing.
	/// </summary>
	MWing,

	/// <summary>
	/// Indicates the local wing.
	/// </summary>
	LocalWing,

	/// <summary>
	/// Indicates the split wing.
	/// </summary>
	SplitWing,

	/// <summary>
	/// Indicates the hybrid wing.
	/// </summary>
	HybridWing,

	/// <summary>
	/// Indicates the grouped XY-Wing.
	/// </summary>
	GroupedXyWing,

	/// <summary>
	/// Indicates the grouped W-Wing.
	/// </summary>
	GroupedWWing,

	/// <summary>
	/// Indicates the grouped M-Wing.
	/// </summary>
	GroupedMWing,

	/// <summary>
	/// Indicates the grouped local wing.
	/// </summary>
	GroupedLocalWing,

	/// <summary>
	/// Indicates the grouped split wing.
	/// </summary>
	GroupedSplitWing,

	/// <summary>
	/// Indicates the grouped hybrid wing.
	/// </summary>
	GroupedHybridWing,

	/// <summary>
	/// Indicates the unique rectangle type 1.
	/// </summary>
	UniqueRectangleType1,

	/// <summary>
	/// Indicates the unique rectangle type 2.
	/// </summary>
	UniqueRectangleType2,

	/// <summary>
	/// Indicates the unique rectangle type 3.
	/// </summary>
	UniqueRectangleType3,

	/// <summary>
	/// Indicates the unique rectangle type 4.
	/// </summary>
	UniqueRectangleType4,

	/// <summary>
	/// Indicates the unique rectangle type 5.
	/// </summary>
	UniqueRectangleType5,

	/// <summary>
	/// Indicates the unique rectangle type 6.
	/// </summary>
	UniqueRectangleType6,

	/// <summary>
	/// Indicates the hidden unique rectangle.
	/// </summary>
	HiddenUniqueRectangle,

	/// <summary>
	/// Indicates the unique rectangle + 2D.
	/// </summary>
	UniqueRectangle2D,

	/// <summary>
	/// Indicates the unique rectangle + 2B / 1SL.
	/// </summary>
	UniqueRectangle2B1,

	/// <summary>
	/// Indicates the unique rectangle + 2D / 1SL.
	/// </summary>
	UniqueRectangle2D1,

	/// <summary>
	/// Indicates the unique rectangle + 3X.
	/// </summary>
	UniqueRectangle3X,

	/// <summary>
	/// Indicates the unique rectangle + 3x / 1SL.
	/// </summary>
	UniqueRectangle3X1L,

	/// <summary>
	/// Indicates the unique rectangle + 3X / 1SL.
	/// </summary>
	UniqueRectangle3X1U,

	/// <summary>
	/// Indicates the unique rectangle + 3X / 2SL.
	/// </summary>
	UniqueRectangle3X2,

	/// <summary>
	/// Indicates the unique rectangle + 3N / 2SL.
	/// </summary>
	UniqueRectangle3N2,

	/// <summary>
	/// Indicates the unique rectangle + 3U / 2SL.
	/// </summary>
	UniqueRectangle3U2,

	/// <summary>
	/// Indicates the unique rectangle + 3E / 2SL.
	/// </summary>
	UniqueRectangle3E2,

	/// <summary>
	/// Indicates the unique rectangle + 4x / 1SL.
	/// </summary>
	UniqueRectangle4X1L,

	/// <summary>
	/// Indicates the unique rectangle + 4X / 1SL.
	/// </summary>
	UniqueRectangle4X1U,

	/// <summary>
	/// Indicates the unique rectangle + 4x / 2SL.
	/// </summary>
	UniqueRectangle4X2L,

	/// <summary>
	/// Indicates the unique rectangle + 4X / 2SL.
	/// </summary>
	UniqueRectangle4X2U,

	/// <summary>
	/// Indicates the unique rectangle + 4X / 3SL.
	/// </summary>
	UniqueRectangle4X3,

	/// <summary>
	/// Indicates the unique rectangle + 4C / 3SL.
	/// </summary>
	UniqueRectangle4C3,

	/// <summary>
	/// Indicates the unique rectangle-XY-Wing.
	/// </summary>
	UniqueRectangleXyWing,

	/// <summary>
	/// Indicates the unique rectangle-XYZ-Wing.
	/// </summary>
	UniqueRectangleXyzWing,

	/// <summary>
	/// Indicates the unique rectangle-WXYZ-Wing.
	/// </summary>
	UniqueRectangleWxyzWing,

	/// <summary>
	/// Indicates the unique rectangle sue de coq.
	/// </summary>
	UniqueRectangleSueDeCoq,

	/// <summary>
	/// Indicates the unique rectangle unknown covering.
	/// </summary>
	UniqueRectangleUnknownCovering,

	/// <summary>
	/// Indicates the unique rectangle guardian.
	/// </summary>
	UniqueRectangleBrokenWing,

	/// <summary>
	/// Indicates the avoidable rectangle type 1.
	/// </summary>
	AvoidableRectangleType1,

	/// <summary>
	/// Indicates the avoidable rectangle type 2.
	/// </summary>
	AvoidableRectangleType2,

	/// <summary>
	/// Indicates the avoidable rectangle type 3.
	/// </summary>
	AvoidableRectangleType3,

	/// <summary>
	/// Indicates the avoidable rectangle type 5.
	/// </summary>
	AvoidableRectangleType5,

	/// <summary>
	/// Indicates the hidden avoidable rectangle.
	/// </summary>
	HiddenAvoidableRectangle,

	/// <summary>
	/// Indicates the avoidable rectangle + 2D.
	/// </summary>
	AvoidableRectangle2D,

	/// <summary>
	/// Indicates the avoidable rectangle + 3X.
	/// </summary>
	AvoidableRectangle3X,

	/// <summary>
	/// Indicates the avoidable rectangle XY-Wing.
	/// </summary>
	AvoidableRectangleXyWing,

	/// <summary>
	/// Indicates the avoidable rectangle XYZ-Wing.
	/// </summary>
	AvoidableRectangleXyzWing,

	/// <summary>
	/// Indicates the avoidable rectangle WXYZ-Wing.
	/// </summary>
	AvoidableRectangleWxyzWing,

	/// <summary>
	/// Indicates the avoidable rectangle sue de coq.
	/// </summary>
	AvoidableRectangleSueDeCoq,

	/// <summary>
	/// Indicates the avoidable rectangle guardian.
	/// </summary>
	AvoidableRectangleBrokenWing,

	/// <summary>
	/// Indicates the avoidable rectangle hidden single in block.
	/// </summary>
	AvoidableRectangleHiddenSingleBlock,

	/// <summary>
	/// Indicates the avoidable rectangle hidden single in row.
	/// </summary>
	AvoidableRectangleHiddenSingleRow,

	/// <summary>
	/// Indicates the avoidable rectangle hidden single in column.
	/// </summary>
	AvoidableRectangleHiddenSingleColumn,

	/// <summary>
	/// Indicates the unique loop type 1.
	/// </summary>
	UniqueLoopType1,

	/// <summary>
	/// Indicates the unique loop type 2.
	/// </summary>
	UniqueLoopType2,

	/// <summary>
	/// Indicates the unique loop type 3.
	/// </summary>
	UniqueLoopType3,

	/// <summary>
	/// Indicates the unique loop type 4.
	/// </summary>
	UniqueLoopType4,

	/// <summary>
	/// Indicates the extended rectangle type 1.
	/// </summary>
	ExtendedRectangleType1,

	/// <summary>
	/// Indicates the extended rectangle type 2.
	/// </summary>
	ExtendedRectangleType2,

	/// <summary>
	/// Indicates the extended rectangle type 3.
	/// </summary>
	ExtendedRectangleType3,

	/// <summary>
	/// Indicates the extended rectangle type 4.
	/// </summary>
	ExtendedRectangleType4,

	/// <summary>
	/// Indicates the bi-value universal grave type 1.
	/// </summary>
	BivalueUniversalGraveType1,

	/// <summary>
	/// Indicates the bi-value universal grave type 2.
	/// </summary>
	BivalueUniversalGraveType2,

	/// <summary>
	/// Indicates the bi-value universal grave type 3.
	/// </summary>
	BivalueUniversalGraveType3,

	/// <summary>
	/// Indicates the bi-value universal grave type 4.
	/// </summary>
	BivalueUniversalGraveType4,

	/// <summary>
	/// Indicates the bi-value universal grave + n.
	/// </summary>
	BivalueUniversalGravePlusN,

	/// <summary>
	/// Indicates the bi-value universal grave + n with forcing chains.
	/// </summary>
	BivalueUniversalGravePlusNForcingChains,

	/// <summary>
	/// Indicates the bi-value universal grave XZ rule.
	/// </summary>
	BivalueUniversalGraveXzRule,

	/// <summary>
	/// Indicates the bi-value universal grave XY-Wing.
	/// </summary>
	BivalueUniversalGraveXyWing,

	/// <summary>
	/// Indicates the unique polygon type 1.
	/// </summary>
	UniquePolygonType1,

	/// <summary>
	/// Indicates the unique polygon type 2.
	/// </summary>
	UniquePolygonType2,

	/// <summary>
	/// Indicates the unique polygon type 3.
	/// </summary>
	UniquePolygonType3,

	/// <summary>
	/// Indicates the unique polygon type 4.
	/// </summary>
	UniquePolygonType4,

	/// <summary>
	/// Indicates the Qiu's deadly pattern type 1.
	/// </summary>
	QiuDeadlyPatternType1,

	/// <summary>
	/// Indicates the Qiu's deadly pattern type 2.
	/// </summary>
	QiuDeadlyPatternType2,

	/// <summary>
	/// Indicates the Qiu's deadly pattern type 3.
	/// </summary>
	QiuDeadlyPatternType3,

	/// <summary>
	/// Indicates the Qiu's deadly pattern type 4.
	/// </summary>
	QiuDeadlyPatternType4,

	/// <summary>
	/// Indicates the locked Qiu's deadly pattern.
	/// </summary>
	LockedQiuDeadlyPattern,

	/// <summary>
	/// Indicates the unique square type 1.
	/// </summary>
	UniqueSquareType1,

	/// <summary>
	/// Indicates the unique square type 2.
	/// </summary>
	UniqueSquareType2,

	/// <summary>
	/// Indicates the unique square type 3.
	/// </summary>
	UniqueSquareType3,

	/// <summary>
	/// Indicates the unique square type 4.
	/// </summary>
	UniqueSquareType4,

	/// <summary>
	/// Indicates the sue de coq.
	/// </summary>
	SueDeCoq,

	/// <summary>
	/// Indicates the sue de coq with isolated digit.
	/// </summary>
	SueDeCoqIsolated,

	/// <summary>
	/// Indicates the 3-dimensional sue de coq.
	/// </summary>
	SueDeCoq3Dimension,

	/// <summary>
	/// Indicates the sue de coq cannibalism.
	/// </summary>
	SueDeCoqCannibalism,

	/// <summary>
	/// Indicates the skyscraper.
	/// </summary>
	Skyscraper,

	/// <summary>
	/// Indicates the two-string kite.
	/// </summary>
	TwoStringKite,

	/// <summary>
	/// Indicates the turbot fish.
	/// </summary>
	TurbotFish,

	/// <summary>
	/// Indicates the empty rectangle.
	/// </summary>
	EmptyRectangle,

	/// <summary>
	/// Indicates the broken wing.
	/// </summary>
	BrokenWing,

	/// <summary>
	/// Indicates the bi-value oddagon type 1.
	/// </summary>
	BivalueOddagonType1,

	/// <summary>
	/// Indicates the bi-value oddagon type 2.
	/// </summary>
	BivalueOddagonType2,

	/// <summary>
	/// Indicates the bi-value oddagon type 3.
	/// </summary>
	BivalueOddagonType3,

	/// <summary>
	/// Indicates the grouped bi-value oddagon.
	/// </summary>
	GroupedBivalueOddagon,

	/// <summary>
	/// Indicates the X-Chain.
	/// </summary>
	XChain,

	/// <summary>
	/// Indicates the Y-Chain.
	/// </summary>
	YChain,

	/// <summary>
	/// Indicates the fishy cycle.
	/// </summary>
	FishyCycle,

	/// <summary>
	/// Indicates the XY-Chain.
	/// </summary>
	XyChain,

	/// <summary>
	/// Indicates the XY-Cycle.
	/// </summary>
	XyCycle,

	/// <summary>
	/// Indicates the XY-X-Chain.
	/// </summary>
	XyXChain,

	/// <summary>
	/// Indicates the purple cow.
	/// </summary>
	PurpleCow,

	/// <summary>
	/// Indicates the discontinuous nice loop.
	/// </summary>
	DiscontinuousNiceLoop,

	/// <summary>
	/// Indicates the continuous nice loop.
	/// </summary>
	ContinuousNiceLoop,

	/// <summary>
	/// Indicates the alternating inference chain.
	/// </summary>
	AlternatingInferenceChain,

	/// <summary>
	/// Indicates the grouped X-Chain.
	/// </summary>
	GroupedXChain,

	/// <summary>
	/// Indicates the grouped fishy cycle.
	/// </summary>
	GroupedFishyCycle,

	/// <summary>
	/// Indicates the grouped XY-Chain.
	/// </summary>
	GroupedXyChain,

	/// <summary>
	/// Indicates the grouped XY-Cycle.
	/// </summary>
	GroupedXyCycle,

	/// <summary>
	/// Indicates the grouped XY-X-Chain.
	/// </summary>
	GroupedXyXChain,

	/// <summary>
	/// Indicates the grouped purple cow.
	/// </summary>
	GroupedPurpleCow,

	/// <summary>
	/// Indicates the grouped discontinuous nice loop.
	/// </summary>
	GroupedDiscontinuousNiceLoop,

	/// <summary>
	/// Indicates the grouped continuous nice loop.
	/// </summary>
	GroupedContinuousNiceLoop,

	/// <summary>
	/// Indicates the grouped alternating inference chain.
	/// </summary>
	GroupedAlternatingInferenceChain,

	/// <summary>
	/// Indicates the nishio forcing chains.
	/// </summary>
	NishioForcingChains,

	/// <summary>
	/// Indicates the region forcing chains.
	/// </summary>
	RegionForcingChains,

	/// <summary>
	/// Indicates the cell forcing chains.
	/// </summary>
	CellForcingChains,

	/// <summary>
	/// Indicates the dynamic region forcing chains.
	/// </summary>
	DynamicRegionForcingChains,

	/// <summary>
	/// Indicates the dynamic cell forcing chains.
	/// </summary>
	DynamicCellForcingChains,

	/// <summary>
	/// Indicates the dynamic contradiction forcing chains.
	/// </summary>
	DynamicContradictionForcingChains,

	/// <summary>
	/// Indicates the dynamic double forcing chains.
	/// </summary>
	DynamicDoubleForcingChains,

	/// <summary>
	/// Indicates the dynamic forcing chains.
	/// </summary>
	DynamicForcingChains,

	/// <summary>
	/// Indicates the empty rectangle intersection pair.
	/// </summary>
	EmptyRectangleIntersectionPair,

	/// <summary>
	/// Indicates the extended subset principle.
	/// </summary>
	ExtendedSubsetPrinciple,

	/// <summary>
	/// Indicates the singly linked ALS-XZ.
	/// </summary>
	SinglyLinkedAlmostLockedSetsXzRule,

	/// <summary>
	/// Indicates the doubly linked ALS-XZ.
	/// </summary>
	DoublyLinkedAlmostLockedSetsXzRule,

	/// <summary>
	/// Indicates the ALS-XY-Wing.
	/// </summary>
	AlmostLockedSetsXyWing,

	/// <summary>
	/// Indicates the ALS-W-Wing.
	/// </summary>
	AlmostLockedSetsWWing,

	/// <summary>
	/// Indicates the death blossom.
	/// </summary>
	DeathBlossom,

	/// <summary>
	/// Indicates the Gurth's symmetrical placement.
	/// </summary>
	GurthSymmetricalPlacement,

	/// <summary>
	/// Indicates the extended Gurth's symmetrical placement.
	/// </summary>
	ExtendedGurthSymmetricalPlacement,

	/// <summary>
	/// Indicates the junior exocet.
	/// </summary>
	JuniorExocet,

	/// <summary>
	/// Indicates the senior exocet.
	/// </summary>
	SeniorExocet,

	/// <summary>
	/// Indicates the complex senior exocet.
	/// </summary>
	ComplexSeniorExocet,

	/// <summary>
	/// Indicates the siamese junior exocet.
	/// </summary>
	SiameseJuniorExocet,

	/// <summary>
	/// Indicates the siamese senior exocet.
	/// </summary>
	SiameseSeniorExocet,

	/// <summary>
	/// Indicates the domino loop.
	/// </summary>
	DominoLoop,

	/// <summary>
	/// Indicates the multi-sector locked sets.
	/// </summary>
	MultisectorLockedSets,

	/// <summary>
	/// Indicates the pattern overlay method.
	/// </summary>
	PatternOverlay,

	/// <summary>
	/// Indicates the template set.
	/// </summary>
	TemplateSet,

	/// <summary>
	/// Indicates the template delete.
	/// </summary>
	TemplateDelete,

	/// <summary>
	/// Indicates the bowman's bingo.
	/// </summary>
	BowmanBingo,

	/// <summary>
	/// Indicates the brute force.
	/// </summary>
	BruteForce,
}
