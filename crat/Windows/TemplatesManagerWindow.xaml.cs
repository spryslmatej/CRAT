using System.Collections.Generic;
using System.Windows;

namespace CRAT.Windows
{
    /// <summary>
    /// Interaction logic for TemplatesManagerWindow.xaml
    /// </summary>

    public partial class TemplatesManagerWindow : Window
    {
        //  https://htmlcolorcodes.com/color-picker/
        private readonly List<ColorItem> _colorsList = new List<ColorItem>()
        {
            new ColorItem("Red", "#FF0000"),
            new ColorItem("DarkRed", "#8B0000"),
            new ColorItem("Pink", "#FFC0CB"),
            new ColorItem("DarkOrange", "#FF8C00"),
            new ColorItem("Orange", "#FFA500"),
            new ColorItem("Gold", "#FFD700"),
            new ColorItem("Yellow", "#FFFF00"),
            new ColorItem("Violet", "#EE82EE"),
            new ColorItem("Magenta", "#FF00FF"),
            new ColorItem("Indigo", "#4B0082"),
            new ColorItem("Lime", "#00FF00"),
            new ColorItem("Green", "#008000"),
            new ColorItem("Aqua", "#00FFFF"),
            new ColorItem("Blue", "#0000FF"),
        };

        public TemplatesManagerWindow()
        {
            InitializeComponent();

            ComboBox_Colors.ItemsSource = _colorsList;
        }


        #region Window Elements
        private void Button_OK(object sender, RoutedEventArgs e) { this.Close(); }
        #endregion

    }
}
