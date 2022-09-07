﻿using Microsoft.UI;
using Microsoft.UI.Windowing;

namespace Sudoku.UI.Views.Windows;

/// <summary>
/// An empty window that can be used on its own or navigated to within a <see cref="Frame"/>.
/// </summary>
/// <seealso cref="Frame"/>
public sealed partial class MainWindow : Window
{
	/// <summary>
	/// Initializes a <see cref="MainWindow"/> instance.
	/// </summary>
	public MainWindow()
	{
		// Intializes the controls.
		InitializeComponent();

		// Sets the title of the window.
		Title = Get("ProgramName");

		// To customize the title bar if available.
		CustomizeTitleBar();
	}


	/// <summary>
	/// Customize the title bar if available.
	/// </summary>
	private void CustomizeTitleBar()
	{
		// Check to see if customization is supported.
		// Currently only supported on Windows 11.
		if (AppWindowTitleBar.IsCustomizationSupported() && this.GetAppWindow() is { TitleBar: var titleBar })
		{
			// Hide default title bar.
			titleBar.ExtendsContentIntoTitleBar = true;

			// Sets the background color on "those" three buttons to transparent.
			titleBar.ButtonBackgroundColor = Colors.Transparent;
			titleBar.ButtonForegroundColor = Colors.Black;
			titleBar.ButtonHoverBackgroundColor = Colors.LightGray;
			titleBar.ButtonHoverForegroundColor = Colors.Black;
			titleBar.ButtonPressedBackgroundColor = Colors.DimGray;
			titleBar.ButtonPressedForegroundColor = Colors.Black;
			titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
			titleBar.ButtonInactiveForegroundColor = Colors.Gray;
		}
		else
		{
			// Title bar customization using these APIs is currently
			// supported only on Windows 11. In other cases, hide
			// the custom title bar element.
			_cAppTitleBar.Visibility = Visibility.Collapsed;
		}
	}
}
