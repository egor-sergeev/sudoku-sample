﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using Sudoku.UI.Drawing;
using Sudoku.UI.Drawing.Shapes;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;

namespace Sudoku.UI.Views.Controls;

/// <summary>
/// Defines a user control that interacts with a sudoku grid.
/// </summary>
public sealed partial class SudokuPane : UserControl, INotifyPropertyChanged
{
	/// <summary>
	/// Indicates the delta that is used for checking whether two <see cref="double"/> values are same
	/// or their difference is below to the delta value.
	/// </summary>
	private const double Epsilon = 1E-2;


	/// <summary>
	/// Indicates the inner collection that stores the drawing elements, and also influences the controls
	/// displaying in the window.
	/// </summary>
	private readonly DrawingElementBag _drawingElements = new();

	/// <summary>
	/// Indicates the size that the current pane is, which is the backing field
	/// of the property <see cref="Size"/>.
	/// </summary>
	/// <seealso cref="Size"/>
	private double _size;

	/// <summary>
	/// Indicates the outside offset value, which is the backing field
	/// of the property <see cref="OutsideOffset"/>.
	/// </summary>
	/// <seealso cref="OutsideOffset"/>
	private double _outsideOffset;

	/// <summary>
	/// Indicates the current mouse point.
	/// </summary>
	/// <remarks>
	/// The variable is used for the following members:
	/// <list type="bullet">
	/// <item><see cref="OnPointerMoved(PointerRoutedEventArgs)"/></item>
	/// <item><see cref="OnKeyDown(KeyRoutedEventArgs)"/></item>
	/// </list>
	/// </remarks>
	private Point _currentPointPosition;


	/// <summary>
	/// Initializes a <see cref="SudokuPane"/> instance.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public SudokuPane() => InitializeComponent();


	/// <summary>
	/// Gets or sets the size of the pane.
	/// </summary>
	public double Size
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _size;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_size.NearlyEquals(value, Epsilon))
			{
				return;
			}

			_size = value;
			foreach (var drawingElement in _drawingElements.OfType<CellLine, BlockLine>())
			{
				drawingElement.DynamicAssign(
					instance =>
					{
						instance.OutsideOffset = OutsideOffset;
						instance.PaneSize = value;
					}
				);
			}
		}
	}

	/// <summary>
	/// Gets or sets the outside offset to the view model.
	/// </summary>
	public double OutsideOffset
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _outsideOffset;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_outsideOffset.NearlyEquals(value, Epsilon))
			{
				return;
			}

			_outsideOffset = value;
			foreach (var drawingElement in _drawingElements.OfType<CellLine, BlockLine>())
			{
				drawingElement.DynamicAssign(
					instance =>
					{
						instance.OutsideOffset = value;
						instance.PaneSize = Size;
					}
				);
			}
		}
	}

	/// <summary>
	/// Indicates the current cell focused. The default value is -1.
	/// </summary>
	public int CurrentCell { get; internal set; } = -1;

	/// <summary>
	/// Indicates the number of the total undo steps.
	/// </summary>
	public int UndoStepsCount => GetSudokuGridViewModel().UndoStepsCount;

	/// <summary>
	/// Indicates the number of the total redo steps.
	/// </summary>
	public int RedoStepsCount => GetSudokuGridViewModel().RedoStepsCount;

	/// <summary>
	/// Gets or sets the current used grid.
	/// </summary>
	public Grid Grid
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete($"Due to the prefromance, please use the method '{nameof(GridByReference)}' instead.", false)]
		get => GetSudokuGridViewModel().Grid;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => GetSudokuGridViewModel().Grid = value;
	}


	/// <inheritdoc/>
	public event PropertyChangedEventHandler? PropertyChanged;


	/// <summary>
	/// Undo a step.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UndoStep() => GetSudokuGridViewModel().Undo();

	/// <summary>
	/// Redo a step.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RedoStep() => GetSudokuGridViewModel().Redo();

	/// <summary>
	/// To make the cell fill the digit.
	/// </summary>
	/// <param name="cell">The cell.</param>
	/// <param name="digit">The digit.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MakeDigit(int cell, int digit) => GetSudokuGridViewModel().MakeDigit(cell, digit);

	/// <summary>
	/// To eliminate the digit from the grid.
	/// </summary>
	/// <param name="cell">The cell.</param>
	/// <param name="digit">The digit.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void EliminateDigit(int cell, int digit) => GetSudokuGridViewModel().EliminateDigit(cell, digit);

	/// <summary>
	/// To fix the grid, to change all modifiable values to given ones.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void FixGrid() => GetSudokuGridViewModel().FixGrid();

	/// <summary>
	/// To unfix the grid, to change all given values to modifiable ones.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UnfixGrid() => GetSudokuGridViewModel().UnfixGrid();

	/// <summary>
	/// To reset the grid, to revert the grid to the initial status.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ResetGrid() => GetSudokuGridViewModel().ResetGrid();

	/// <summary>
	/// To replace the grid with the new one, in order to make the current operation undoable.
	/// </summary>
	/// <param name="grid">The grid to be replaced with.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ReplaceGridUndoable(in Grid grid) => GetSudokuGridViewModel().ReplaceGrid(grid);

	/// <summary>
	/// Gets or sets the current used grid by reference. The method will return by reference, in order to
	/// copy the reference instead of the instance itself, to optimize the memory allocation.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref readonly Grid GridByReference() => ref GetSudokuGridViewModel().GetGridByReference();

	/// <inheritdoc/>
	protected override void OnPointerMoved(PointerRoutedEventArgs e)
	{
		base.OnPointerMoved(e);

		_currentPointPosition = e.GetCurrentPoint(this).Position;
	}

	/// <inheritdoc/>
	protected override void OnKeyDown(KeyRoutedEventArgs e)
	{
		base.OnKeyDown(e);

		// TODO: Update more mode to focus a cell.
		// Here we just suppose a mode to fill with value in a cell:
		//     1. Gets the mouse position, and then get the position of the cell.
		//     2. Gets the pressed key from the argument 'e'.
		//     3. Fill the cell with the value.
		int cell = PointConversions.GetCell(_currentPointPosition, Size, OutsideOffset);
		if (cell == -1)
		{
			e.Handled = true;
			return;
		}

		var a = EliminateDigit;
		var b = MakeDigit;
		switch (e.Key)
		{
			// Digits.
			case var key and >= VirtualKey.Number0 and <= VirtualKey.Number9:
			{
				(modifierKeyIsPressed(VirtualKey.Shift) ? a : b)(cell, key - VirtualKey.Number0 - 1);

				break;
			}
			// Digits.
			case var key and >= VirtualKey.NumberPad0 and <= VirtualKey.NumberPad9:
			{
				(modifierKeyIsPressed(VirtualKey.Shift) ? a : b)(cell, key - VirtualKey.NumberPad0 - 1);

				break;
			}
			// Other cases.
			default:
			{
				// Here we must set the value to 'false', in order to process keyboard accelerators.
				// If we se 'e.Handled = true', the keyboard accelerators in the base page won't be triggered.
				e.Handled = false;
				return;
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool modifierKeyIsPressed(VirtualKey key) =>
			InputKeyboardSource.GetKeyStateForCurrentThread(key).Flags(CoreVirtualKeyStates.Down);
	}

	/// <inheritdoc/>
	protected override void OnRightTapped(RightTappedRoutedEventArgs e)
	{
		base.OnRightTapped(e);

		// Gets the mouse point and converts to the real cell value.
		var point = e.GetPosition(this);
		int cell = PointConversions.GetCell(point, Size, OutsideOffset);

		// Checks whether the cell value is valid.
		if (cell == -1)
		{
			return;
		}

		// Sets the tag with the cell value, in order to get the details by other methods later.
		CurrentCell = cell;

		// Try to create the menu flyout and show the items.
		for (int i = 0; i < 9; i++)
		{
			((MenuFlyoutItem)FindName($"_cButtonMake{i + 1}")).Visibility = getVisibilityViaCandidate(cell, i);
		}
		for (int i = 0; i < 9; i++)
		{
			((MenuFlyoutItem)FindName($"_cButtonDelete{i + 1}")).Visibility = getVisibilityViaCandidate(cell, i);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Visibility getVisibilityViaCandidate(int cell, int i) =>
			Grid.Exists(cell, i) is true ? Visibility.Visible : Visibility.Collapsed;
	}

	/// <summary>
	/// Gets the <see cref="SudokuGrid"/> instance as the view model.
	/// </summary>
	/// <returns>The <see cref="SudokuGrid"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private SudokuGrid GetSudokuGridViewModel() => _drawingElements.OfType<SudokuGrid>().Single();


	/// <summary>
	/// Triggers when the pane is loaded.
	/// </summary>
	/// <param name="sender">The object to trigger the event. The instance is always itself.</param>
	/// <param name="e">The event arguments provided.</param>
	private void SudokuPane_Loaded(object sender, RoutedEventArgs e)
	{
		var up = ((App)Application.Current).UserPreference;

		// Initializes the outside border if worth.
		if (up.OutsideBorderWidth != 0 && OutsideOffset != 0)
		{
			_drawingElements.Add(new OutsideRectangle(up.OutsideBorderColor, Size, up.OutsideBorderWidth));
		}

		// Initializes candidate border lines if worth.
		if (up.ShowCandidateBorderLines)
		{
			for (byte i = 0; i < 28; i++)
			{
				if (i % 3 == 0)
				{
					// Skips the overlapping lines.
					continue;
				}

				_drawingElements.Add(
					new CandidateLine(up.CandidateBorderColor, up.CandidateBorderWidth, Size, OutsideOffset, i));
				_drawingElements.Add(
					new CandidateLine(
						up.CandidateBorderColor, up.CandidateBorderWidth, Size, OutsideOffset, (byte)(i + 28)));
			}
		}

		// Initializes cell border lines.
		for (byte i = 0; i < 10; i++)
		{
			if (i % 3 == 0)
			{
				// Skips the overlapping lines.
				continue;
			}

			_drawingElements.Add(new CellLine(up.CellBorderColor, up.CellBorderWidth, Size, OutsideOffset, i));
			_drawingElements.Add(
				new CellLine(up.CellBorderColor, up.CellBorderWidth, Size, OutsideOffset, (byte)(i + 10)));
		}

		// Initializes block border lines.
		for (byte i = 0; i < 4; i++)
		{
			_drawingElements.Add(new BlockLine(up.BlockBorderColor, up.BlockBorderWidth, Size, OutsideOffset, i));
			_drawingElements.Add(
				new BlockLine(up.BlockBorderColor, up.BlockBorderWidth, Size, OutsideOffset, (byte)(i + 4)));
		}

		// Initializes the sudoku grid.
		_drawingElements.Add(new SudokuGrid(up, Size, OutsideOffset, triggerBoth));

		// Add them into the control collection.
		foreach (var control in from drawingElement in _drawingElements select drawingElement.GetControl())
		{
			_cCanvasMain.Children.Add(control);
		}


		void triggerBoth()
		{
			PropertyChanged?.Invoke(this, new(nameof(UndoStepsCount)));
			PropertyChanged?.Invoke(this, new(nameof(RedoStepsCount)));
		}
	}

	/// <summary>
	/// Triggers when the mouse is moved into the current control.
	/// </summary>
	/// <param name="sender">The object to trigger the event.</param>
	/// <param name="e">The event arguments provided.</param>
	private void SudokuPane_PointerEntered(object sender, PointerRoutedEventArgs e) =>
		// Set the focus to the control in order to trigger the 'KeyDown' event.
		Focus(FocusState.Programmatic);

	/// <summary>
	/// Triggers when the target <see cref="MenuFlyoutItem"/> is clicked.
	/// </summary>
	/// <param name="sender">The object that triggers the event.</param>
	/// <param name="e">The event arguments provided.</param>
	private void MakeOrDeleteMenuItem_Click(object sender, RoutedEventArgs e)
	{
		if (sender is not MenuFlyoutItem { Tag: string s } || !int.TryParse(s, out int possibleDigit))
		{
			return;
		}

		var a = EliminateDigit;
		var b = MakeDigit;
		var (digit, action) = possibleDigit switch
		{
			> 0 => (possibleDigit - 1, b),
			< 0 => (~possibleDigit, a),
			_ => throw new()
		};
		action(CurrentCell, digit);
	}
}
