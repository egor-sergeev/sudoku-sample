﻿#if FEATURE_GENERIC_MATH && FEATURE_GENERIC_MATH_IN_ARG
namespace System;

/// <summary>
/// Defines the logical-and operators. The operators are:
/// <list type="bullet">
/// <item><see cref="operator |(in TSelf, in TSelf)"/></item>
/// <item><see cref="operator |(in TSelf, TSelf)"/></item>
/// <item><see cref="operator |(TSelf, in TSelf)"/></item>
/// </list>
/// </summary>
/// <typeparam name="TSelf">The type of the current instance.</typeparam>
[RequiresPreviewFeatures]
public interface IValueLogicalOrOperators<TSelf> where TSelf : struct, IValueLogicalOrOperators<TSelf>
{
	/// <summary>
	/// Make logical-or operation on two <typeparamref name="TSelf"/>-typed instance.
	/// </summary>
	/// <param name="left">The left-side instance to calculate.</param>
	/// <param name="right">The left-side instance to calculate.</param>
	/// <returns>The result value.</returns>
	static abstract TSelf operator |(in TSelf left, in TSelf right);

	/// <inheritdoc cref="operator |(in TSelf, in TSelf)"/>
	static abstract TSelf operator |(in TSelf left, TSelf right);

	/// <inheritdoc cref="operator |(in TSelf, in TSelf)"/>
	static abstract TSelf operator |(TSelf left, in TSelf right);
}

#endif