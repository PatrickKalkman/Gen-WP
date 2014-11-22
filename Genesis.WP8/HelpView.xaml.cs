using Genesis.WP8.Common;

using Microsoft.Phone.Controls;

namespace Genesis.WP8
{
    public partial class HelpView : PhoneApplicationPage
    {
        public HelpView()
        {
            InitializeComponent();
            var background = new BackgroundImageBrush();
            LayoutRoot.Background = background.GetBackground();
        }
    }
}