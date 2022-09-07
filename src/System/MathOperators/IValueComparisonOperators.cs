﻿#if FEATURE_GENERIC_MATH && FEATURE_GENERIC_MATH_IN_ARG
namespace System;

/// <summary>
/// Defines the greater-than and less-than operators. The operators are:
/// <list type="bullet">
/// <item><see cref="IValueGreaterThanOrLessThanOperators{TSelf, TOther}.operator }(in TSelf, in TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrLessThanOperators{TSelf, TOther}.operator }(in TSelf, TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrLessThanOperators{TSelf, TOther}.operator }(TSelf, in TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrLessThanOperators{TSelf, TOther}.operator {(in TSelf, in TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrLessThanOperators{TSelf, TOther}.operator {(in TSelf, TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrLessThanOperators{TSelf, TOther}.operator {(TSelf, in TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrEqualsOrLessThanOrEqualsOperators{TSelf, TOther}.operator }=(in TSelf, in TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrEqualsOrLessThanOrEqualsOperators{TSelf, TOther}.operator }=(in TSelf, TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrEqualsOrLessThanOrEqualsOperators{TSelf, TOther}.operator }=(TSelf, in TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrEqualsOrLessThanOrEqualsOperators{TSelf, TOther}.operator {=(in TSelf, in TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrEqualsOrLessThanOrEqualsOperators{TSelf, TOther}.operator {=(in TSelf, TSelf)"/></item>
/// <item><see cref="IValueGreaterThanOrEqualsOrLessThanOrEqualsOperators{TSelf, TOther}.operator {=(TSelf, in TSelf)"/></item>
/// </list>
/// </summary>
/// <typeparam name="TSelf">The type of the current instance.</typeparam>
/// <typeparam name="TOther">The type that takes part in the operation.</typeparam>
[RequiresPreviewFeatures]
public interface IValueComparisonOperators<TSelf, TOther>
	where TSelf :
		struct,
		IComparable,
		IComparable<TSelf>,
		IEquatable<TSelf>,
		IComparisonOperators<TSelf, TOther>,
		IEqualityOperators<TSelf, TOther>,
		IValueGreaterThanOrLessThanOperators<TSelf, TOther>,
		IValueGreaterThanOrEqualsOrLessThanOrEqualsOperators<TSelf, TOther>,
		IValueComparisonOperators<TSelf, TOther>,
		IValueEqualityOperators<TSelf, TOther>
	where TOther : struct
{
}

#endif