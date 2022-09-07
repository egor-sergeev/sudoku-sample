﻿namespace Sudoku.Diagnostics;

/// <summary>
/// Encapsulates a result after <see cref="FileCounter"/>.
/// </summary>
/// <param name="ResultLines">The number of lines found.</param>
/// <param name="FilesCount">The number of files found.</param>
/// <param name="CharactersCount">The number of characters found.</param>
/// <param name="Bytes">All bytes.</param>
/// <param name="Elapsed">The elapsed time during searching.</param>
/// <param name="FileList">
/// The list of files. This property won't be output. If you want to use this property,
/// please write this property explicitly.
/// </param>
/// <seealso cref="FileCounter"/>
public sealed record FileCounterResult(
	int ResultLines, int FilesCount, long CharactersCount,
	long Bytes, in TimeSpan Elapsed, IList<string> FileList)
{
	/// <inheritdoc/>
	public override string ToString()
	{
		string bytesConvertedStr = SizeUnitConverter.Convert(Bytes, out var unit).ToString(".000");
		string bytesUnitStr = unit switch
		{
			SizeUnit.Byte => "B",
			SizeUnit.Kilobyte => "KB",
			SizeUnit.Megabyte => "MB",
			SizeUnit.Gigabyte => "GB",
			SizeUnit.Terabyte => "TB",
			SizeUnit.IKilobyte => "KiB",
			SizeUnit.IMegabyte => "MiB",
			SizeUnit.IGigabyte => "GiB",
			SizeUnit.ITerabyte => "TiB",
			_ => throw new()
		};

		return $$"""
		Results:
		* Code lines: {{ResultLines}}
		* Files: {{FilesCount}}
		* Characters: {{CharactersCount}}
		* Bytes: {{bytesConvertedStr}} {{bytesUnitStr}} ({{Bytes}} Bytes)
		* Time elapsed: {{Elapsed:hh\:mm\.ss\.fff}}

		About more information, please call each property in this instance.
		""";
	}
}
