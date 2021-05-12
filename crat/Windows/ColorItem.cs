using System.Windows.Media;

namespace CRAT.Windows
{
    public class ColorItem
    {
        public string Name { get; }
        public SolidColorBrush Brush { get; }
        public ColorItem(string s, string c)
        {
            Name = s;
            Brush = new BrushConverter().ConvertFromString(c) as SolidColorBrush;
        }
    }
}
