﻿namespace Sudoku.Solving.Manual.Steps;

/// <summary>
/// Provides with a step that is a <b>Alternating Inference Chain</b> technique.
/// </summary>
/// <param name="Conclusions"><inheritdoc/></param>
/// <param name="Views"><inheritdoc/></param>
/// <param name="Chain">Indicates the whole chain.</param>
/// <param name="XEnabled"><inheritdoc cref="AlternatingInferenceChainStepSearcher.XEnabled"/></param>
/// <param name="YEnabled"><inheritdoc cref="AlternatingInferenceChainStepSearcher.YEnabled"/></param>
public sealed record AlternatingInferenceChainStep(
	ImmutableArray<Conclusion> Conclusions,
	ImmutableArray<View> Views,
	AlternatingInferenceChain Chain,
	bool XEnabled,
	bool YEnabled
) : ChainStep(Conclusions, Views), IChainStep, IChainLikeStep
{
	/// <inheritdoc/>
	public override decimal Difficulty =>
		TechniqueCode switch
		{
			Technique.MWing => 4.5M,
			Technique.SplitWing or Technique.HybridWing or Technique.LocalWing => 4.8M,
			_ => (XEnabled && YEnabled ? 5.0M : 4.6M) + IChainLikeStep.GetExtraDifficultyByLength(FlatComplexity - 2)
		};

	/// <inheritdoc/>
	public override int FlatComplexity => Chain.RealChainNodes.Length;

	/// <inheritdoc/>
	public override TechniqueGroup TechniqueGroup =>
		this switch
		{
			{ IsMWing: true } => TechniqueGroup.Wing,
			{ IsLocalWing: true } or { IsSplitWing: true } or { IsHybridWing: true } => TechniqueGroup.Wing,
			_ => TechniqueGroup.AlternatingInferenceChain
		};

	/// <inheritdoc/>
	public override TechniqueTags TechniqueTags =>
		this switch
		{
			{ IsMWing: true } => TechniqueTags.Wings,
			{ IsLocalWing: true } or { IsSplitWing: true } or { IsHybridWing: true } => TechniqueTags.Wings,
			_ => TechniqueTags.LongChaining
		};

	/// <inheritdoc/>
	public override Technique TechniqueCode =>
		this switch
		{
			{ IsXChain: true } => Technique.XChain,
			{ IsMWing: true } => Technique.MWing,
			{ IsSplitWing: true } => Technique.SplitWing,
			{ IsHybridWing: true } => Technique.HybridWing,
			{ IsLocalWing: true } => Technique.LocalWing,
			{
				Chain.RealChainNodes: [{ Digit: var a }, .., { Digit: var b }],
				IsXyChain: var isXy
			} when a == b => isXy ? Technique.XyChain : Technique.AlternatingInferenceChain,
			_ => Conclusions.Length switch
			{
				1 => Technique.DiscontinuousNiceLoop,
				2 => Technique.XyXChain,
				_ => Technique.AlternatingInferenceChain
			}
		};

	/// <inheritdoc/>
	public override DifficultyLevel DifficultyLevel =>
		TechniqueCode is Technique.MWing or Technique.SplitWing or Technique.HybridWing or Technique.LocalWing
			? DifficultyLevel.Hard
			: DifficultyLevel.Fiendish;

	/// <inheritdoc/>
	public override Rarity Rarity =>
		TechniqueCode switch
		{
			Technique.MWing => Rarity.Sometimes,
			Technique.SplitWing => Rarity.Sometimes,
			Technique.HybridWing => Rarity.Often,
			Technique.LocalWing => Rarity.Sometimes,
			Technique.XChain => Rarity.Often,
			Technique.XyChain => Rarity.Often,
			Technique.XyXChain => Rarity.Often,
			Technique.DiscontinuousNiceLoop => Rarity.Often,
			Technique.AlternatingInferenceChain => Rarity.Often,
			_ => default
		};

	/// <inheritdoc/>
	public override ChainTypeCode SortKey => Enum.Parse<ChainTypeCode>(TechniqueCode.ToString());

	/// <summary>
	/// Indicates whether the specified chain is an XY-Chain.
	/// </summary>
	private bool IsXyChain
	{
		get
		{
			var realChainNodes = Chain.RealChainNodes;
			for (int i = 0, count = realChainNodes.Length; i < count; i += 2)
			{
#pragma warning disable IDE0055
				if ((realChainNodes[i].Cells, realChainNodes[i + 1].Cells) is not ([var c1], [var c2])
					|| c1 != c2)
				{
					return false;
				}
#pragma warning restore IDE0055
			}

			return true;
		}
	}

	/// <summary>
	/// Indicates whether the specified chain is an X-Chain.
	/// </summary>
	private bool IsXChain => XEnabled && !YEnabled;

	/// <summary>
	/// Indicates whether the chain is M-Wing (<c>(x = y) - y = (y - x) = x</c>).
	/// </summary>
	/// <returns>A <see cref="bool"/> value indicating that.</returns>
	private bool IsMWing =>
		Chain is
		{
			RealChainNodes: [
				_,
				SoleCandidateNode { Cell: var a },
				SoleCandidateNode { Cell: var b },
				SoleCandidateNode { Cell: var c },
				SoleCandidateNode { Cell: var d },
				SoleCandidateNode { Cell: var e },
				SoleCandidateNode { Cell: var f },
				..
			]
		} && (
			a / 9 == b / 9 && d / 9 == e / 9
			&& b % 9 == c % 9 && c % 9 == d % 9
			&& a % 9 == e % 9 && e % 9 == f % 9
			|| f / 9 == e / 9 && c / 9 == b / 9 // Reverse case.
			&& d % 9 == e % 9 && c % 9 == d % 9
			&& b % 9 == f % 9 && a % 9 == b % 9
		);

	/// <summary>
	/// Indicates whether the chain is Split-Wing (<c>x = x - (x = y) - y = y</c>).
	/// </summary>
	/// <returns>A <see cref="bool"/> value indicating that.</returns>
	private bool IsSplitWing =>
		Chain is
		{
			RealChainNodes: [
				_,
				SoleCandidateNode { Cell: var a },
				SoleCandidateNode { Cell: var b },
				SoleCandidateNode { Cell: var c },
				SoleCandidateNode { Cell: var d },
				SoleCandidateNode { Cell: var e },
				SoleCandidateNode { Cell: var f },
				..
			]
		}
		&& a % 9 == b % 9 && b % 9 == c % 9 // First three nodes hold a same digit.
		&& d % 9 == e % 9 && e % 9 == f % 9 // Last three nodes hold a same digit.
		&& c / 9 == d / 9; // In same cell.

	/// <summary>
	/// Indicates whether the chain is Hybrid-Wing.
	/// This wing has two types:
	/// <list type="bullet">
	/// <item><c>(x = y) - y = (y - z) = z</c></item>
	/// <item><c>(x = y) - (y = z) - z = z</c></item>
	/// </list>
	/// </summary>
	/// <returns>A <see cref="bool"/> value indicating that.</returns>
	private bool IsHybridWing =>
		Chain is
		{
			RealChainNodes: [
				_,
				SoleCandidateNode { Cell: var a },
				SoleCandidateNode { Cell: var b },
				SoleCandidateNode { Cell: var c },
				SoleCandidateNode { Cell: var d },
				SoleCandidateNode { Cell: var e },
				SoleCandidateNode { Cell: var f },
				..
			]
		}
		&& (
			a / 9 == b / 9 && d / 9 == e / 9
			&& b % 9 == c % 9 && c % 9 == d % 9
			&& e % 9 == f % 9
			|| e / 9 == f / 9 && b / 9 == c / 9
			&& d % 9 == e % 9 && c % 9 == d % 9
			&& a % 9 == b % 9
			|| a / 9 == b / 9 && c / 9 == d / 9 // Reverse case.
			&& b % 9 == c % 9
			&& d % 9 == e % 9 && e % 9 == f % 9
			|| e / 9 == f / 9 && c / 9 == d / 9
			&& d % 9 == e % 9
			&& b % 9 == c % 9 && a % 9 == b % 9
		);

	/// <summary>
	/// Indicates whether the chain is Local-Wing (<c>x = (x - z) = (z - y) = y</c>).
	/// </summary>
	/// <returns>A <see cref="bool"/> value indicating that.</returns>
	private bool IsLocalWing =>
		Chain is
		{
			RealChainNodes: [
				_,
				SoleCandidateNode { Cell: var a },
				SoleCandidateNode { Cell: var b },
				SoleCandidateNode { Cell: var c },
				SoleCandidateNode { Cell: var d },
				SoleCandidateNode { Cell: var e },
				SoleCandidateNode { Cell: var f },
				..
			]
		}
		&& b / 9 == c / 9 && d / 9 == e / 9
		&& a % 9 == b % 9 && c % 9 == d % 9 && e % 9 == f % 9;

	[FormatItem]
	internal string ChainStr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Chain.ToString();
	}
}
