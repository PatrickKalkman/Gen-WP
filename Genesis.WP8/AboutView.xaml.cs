using System.Reflection;
using System.Windows;
using System.Windows.Media;

using Genesis.WP8.Common;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Genesis.WP8
{
    public partial class AboutView : PhoneApplicationPage
    {
        public AboutView()
        {
            InitializeComponent();
            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            Version.Text = nameHelper.Version.ToString();
            var background = new BackgroundImageBrush();
            LayoutRoot.Background = background.GetBackground();
        }

        private void SendAnEmail_OnClick(object sender, RoutedEventArgs e)
        {
            var emailTask = new EmailComposeTask();
            emailTask.To = "pkalkie@gmail.com";
            emailTask.Show();
        }

        private void RateThisApp_OnClick(object sender, RoutedEventArgs e)
        {
            var reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }
    }
}
