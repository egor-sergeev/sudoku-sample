﻿namespace Sudoku.Concepts.Collections.Handlers;

/// <summary>
/// Provides a collection that contains the conclusions.
/// </summary>
public readonly ref partial struct ConclusionCollection
{
	/// <summary>
	/// The internal collection.
	/// </summary>
	private readonly Conclusion[] _collection;


	/// <summary>
	/// Initializes an instance with the specified collection.
	/// </summary>
	/// <param name="collection">The collection.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ConclusionCollection(Conclusion[] collection) : this() => _collection = collection;
	

	/// <summary>
	/// Indicates all cells used in this conclusions list.
	/// </summary>
	public Cells Cells
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => new(from conclusion in _collection select conclusion.Cell);
	}

	/// <summary>
	/// Indicates all digits used in this conclusions list, represented as a mask.
	/// </summary>
	public short Digits
	{
		get
		{
			short result = 0;
			foreach (var conclusion in _collection)
			{
				result |= (short)(1 << conclusion.Digit);
			}

			return result;
		}
	}


	/// <summary>
	/// Determine whether the two collections are equal.
	/// </summary>
	/// <param name="other">The collection to compare.</param>
	/// <returns>A <see cref="bool"/> result.</returns>
	public bool Equals(ConclusionCollection other) => _collection == other._collection;

	/// <inheritdoc cref="object.ToString"/>
	public override string ToString() => ToString(true, ", ");

	/// <summary>
	/// Converts the current instance to <see cref="string"/> with the specified separator.
	/// </summary>
	/// <param name="shouldSort">Indicates whether the specified collection should be sorted first.</param>
	/// <param name="separator">The separator.</param>
	/// <returns>The string result.</returns>
	public string ToString(bool shouldSort, string separator)
	{
		return _collection switch
		{
			[] => string.Empty,
			[var conclusion] => conclusion.ToString(),
			_ => f(_collection)
		};


		unsafe string f(in ReadOnlySpan<Conclusion> collection)
		{
			var conclusions = collection.ToArray();
			var sb = new StringHandler(50);
			if (shouldSort)
			{
				conclusions.Sort(&cmp);

				var selection =
					from conclusion in conclusions
					orderby conclusion.Digit
					group conclusion by conclusion.ConclusionType;
				bool hasOnlyOneType = selection.HasOnlyOneElement();
				foreach (var typeGroup in selection)
				{
					string op = typeGroup.Key == ConclusionType.Assignment ? " = " : " <> ";
					foreach (var digitGroup in
						from conclusion in typeGroup
						group conclusion by conclusion.Digit)
					{
						sb.Append(new Cells(from conclusion in digitGroup select conclusion.Cell));
						sb.Append(op);
						sb.Append(digitGroup.Key + 1);
						sb.Append(separator);
					}

					sb.RemoveFromEnd(separator.Length);
					if (!hasOnlyOneType)
					{
						sb.Append(separator);
					}
				}

				if (!hasOnlyOneType)
				{
					sb.RemoveFromEnd(separator.Length);
				}
			}
			else
			{
				sb.AppendRangeWithSeparator(conclusions, static c => c.ToString(), separator);
			}

			return sb.ToStringAndClear();
		}


		static int cmp(in Conclusion left, in Conclusion right) => left.CompareTo(right);
	}
}
