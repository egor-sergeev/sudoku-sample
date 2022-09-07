﻿namespace System.Text;

partial struct StringHandler
{
	/// <summary>
	/// Encapsulates the enumerator of this collection.
	/// </summary>
	public unsafe ref partial struct Enumerator
	{
		/// <summary>
		/// Indicates the length.
		/// </summary>
		private readonly int _length;

		/// <summary>
		/// Indicates whether 
		/// </summary>
		private int _index;

		/// <summary>
		/// Indicates the pointer that points to the current character.
		/// </summary>
		private char* _ptr;


		/// <summary>
		/// Initializes an instance with the specified character list specified as a <see cref="Span{T}"/>.
		/// </summary>
		/// <param name="chars">The characters.</param>
		/// <seealso cref="Span{T}"/>
		internal Enumerator(in StringHandler chars) : this()
		{
			_length = chars.Length;
			_index = -1;
			fixed (char* p = chars._chars)
			{
				_ptr = p - 1;
			}
		}


		/// <inheritdoc cref="IEnumerator.Current"/>
		public char Current { get; private set; }


		/// <inheritdoc cref="IEnumerator.MoveNext"/>
		public bool MoveNext()
		{
			if (++_index >= _length)
			{
				return false;
			}

			_ptr++;
			return true;
		}
	}
}
