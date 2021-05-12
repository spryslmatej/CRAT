using CRAT.Infrastructure.ImportExport;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;

namespace CRAT.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			ConfigImporter.LoadConfig();

			InitializeComponent();
		}

		private string _exportFilePath = null;

		public string ExportFilePath
		{
			get => _exportFilePath;
			set { Menu_ExportToSVG.IsEnabled = !(value is null); _exportFilePath = value; }
		}

		private void Menu_ExportToSVG_Click(object sender, RoutedEventArgs e)
		{
			if (ExportFilePath is null)
				return;

			Export();
		}

		private void Menu_ExportToSVGAs_Click(object sender, RoutedEventArgs e)
		{
			var saveFileDialog = new SaveFileDialog { Filter = "SVG file (*.svg)|*.svg" };
			if (saveFileDialog.ShowDialog() != true)
				return;

			Export(saveFileDialog.FileName);
		}

		private void Export(string path = null)
		{
			List<string> exported = Menu_ExportColorless.IsChecked ?
				annotationCanvas.VisitorExport(new ColorlessSvgExporter())
				:
				annotationCanvas.VisitorExport(new SvgExporter());

			if (!(path is null))
				ExportFilePath = path;

			Infrastructure.File.FileWriter.WriteFile(ExportFilePath, exported);
			MessageBox.Show("Canvas has been successfully exported.",
							"Export Successful",
							MessageBoxButton.OK,
							MessageBoxImage.Information);
		}
	}
}
