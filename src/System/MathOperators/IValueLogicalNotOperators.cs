﻿#if FEATURE_GENERIC_MATH && FEATURE_GENERIC_MATH_IN_ARG
namespace System;

/// <summary>
/// Defines the logical-not operators. The operators are:
/// <list type="bullet">
/// <item><see cref="operator !(in TSelf)"/></item>
/// </list>
/// </summary>
/// <typeparam name="TSelf">The type of the current instance.</typeparam>
[RequiresPreviewFeatures]
public interface IValueLogicalNotOperators<TSelf> where TSelf : struct, IValueLogicalNotOperators<TSelf>
{
	/// <summary>
	/// Gets the logical-not result value from the instance.
	/// Both are of type <typeparamref name="TSelf"/>.
	/// </summary>
	/// <param name="value">The value itself.</param>
	/// <returns>The result value.</returns>
	static abstract TSelf operator !(in TSelf value);
}

#endif