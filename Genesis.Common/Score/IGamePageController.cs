namespace Genesis.Common.Score
{
    public interface IGamePageController
    {
        void ShowHighscoreTextBox();

        bool HighScoreTextBoxIsVisible();

        void ShowPrivacyMenuBar();

        void HidePrivacyMenuBar();
    }
}
