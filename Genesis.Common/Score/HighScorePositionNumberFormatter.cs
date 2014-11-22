using System.Globalization;

namespace Genesis.Common.Score
{
    public class HighScorePositionNumberFormatter
    {
        public string Format(int position)
        {
            string postFix = string.Empty;

            switch (position)
            {
                case 1:
                    postFix = "ST";
                    break;
                case 2:
                    postFix = "ND";
                    break;
                case 3:
                    postFix = "RD";
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    postFix = "TH";
                    break;
            }

            return string.Format("{0}{1}", position.ToString(CultureInfo.InvariantCulture), postFix);
        }
    }
}
