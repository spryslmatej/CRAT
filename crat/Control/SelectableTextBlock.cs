using System.Windows.Controls;
using System.Windows.Media;

namespace CRAT.Control
{
    public class SelectableTextBlock : TextBlock
    {
        public Brush DefaultBackground { get; set; }
        public void ResetBackground() { Background = DefaultBackground; }
        public SelectableTextBlock() : this(null) { }
        public SelectableTextBlock(Brush def)
        {
            DefaultBackground = def;
            ResetBackground();
        }
    }
}
