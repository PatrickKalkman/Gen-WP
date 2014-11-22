using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

using Genesis.Common;
using Genesis.Common.GameStates;
using Genesis.Common.Score;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

using MonoGame.Framework.WindowsPhone;

namespace Genesis.WP8
{
    public partial class GamePage : PhoneApplicationPage, IGamePageController
    {
        private readonly GenesisGame game;

        public GamePage()
        {
            InitializeComponent();

            game = XamlGame<GenesisGame>.Create("", this);
            game.PlatformSpecificFactory = new WindowsPhoneFactory();
            game.GamePageController = this;
            highScoreTextBox.KeyDown += highScoreTextBox_KeyDown;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var askforReview = (bool)IsolatedStorageSettings.ApplicationSettings["askforreview"];
            if (askforReview)
            {
                //make sure we only ask once! 
                IsolatedStorageSettings.ApplicationSettings["askforreview"] = false;
                var returnvalue = MessageBox.Show("You seem to be liking my free game Gen, would you like to review this app? It will help me", "Please review my app", MessageBoxButton.OKCancel);
                if (returnvalue == MessageBoxResult.OK)
                {
                    var marketplaceReviewTask = new MarketplaceReviewTask();
                    marketplaceReviewTask.Show();
                }
            }
        }

        void highScoreTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                highScorePanel.Visibility = Visibility.Collapsed;
                game.SetHighScore(highScoreTextBox.Text);
            }
        }

        public void ShowHighscoreTextBox()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                highScorePanel.Visibility = Visibility.Visible;
            });
        }

        public bool HighScoreTextBoxIsVisible()
        {
            bool textboxIsVisible = false;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                textboxIsVisible = (highScorePanel.Visibility == Visibility.Visible);
            });

            return textboxIsVisible;
        }

        public void ShowPrivacyMenuBar()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.ApplicationBar.IsVisible = true;
                this.startButtons.Visibility = Visibility.Visible;
            });
        }

        public void HidePrivacyMenuBar()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                this.ApplicationBar.IsVisible = false;
                this.startButtons.Visibility = Visibility.Collapsed;
            });
        }

        private void ApplicationBarPrivacyMenuItem_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PrivacyView.xaml", UriKind.Relative));
        }

        private void ApplicationBarAboutMenuItem_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutView.xaml", UriKind.Relative));
        }

        private void ApplicationBarHelpMenuItem_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpView.xaml", UriKind.Relative));
        }

        private void ApplicationBarClearMenuItem_OnClick(object sender, EventArgs e)
        {
            game.ClearHighScores();
            MessageBox.Show("The leaderboard has been reset");
        }

        private void EasyButton_OnClick(object sender, RoutedEventArgs e)
        {
            game.Start(GameDifficulty.Easy);
        }

        private void MediumButton_OnClick(object sender, RoutedEventArgs e)
        {
            game.Start(GameDifficulty.Medium);
        }

        private void HardButton_OnClick(object sender, RoutedEventArgs e)
        {
            game.Start(GameDifficulty.Hard);
        }
    }
}