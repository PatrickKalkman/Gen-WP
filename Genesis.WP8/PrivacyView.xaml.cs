using Genesis.WP8.Common;

using Microsoft.Phone.Controls;

namespace Genesis.WP8
{
    public partial class PrivacyView : PhoneApplicationPage
    {
        public PrivacyView()
        {
            InitializeComponent();
            var background = new BackgroundImageBrush();
            LayoutRoot.Background = background.GetBackground();
        }
    }
}