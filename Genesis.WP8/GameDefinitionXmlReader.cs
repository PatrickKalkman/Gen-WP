using Genesis.Common;

namespace Genesis.Management.WP8
{
    using System.IO;

    public class GameDefinitionXmlReader : IGameDefinitionXmlReader
    {
        public bool ReadAll(string xmlFilename, out string contents)
        {
            if (File.Exists(xmlFilename))
            {
                contents = string.Empty;
#if !WINDOWS_PHONE
                contents = File.ReadAllText(xmlFilename);
#endif
                return true;
            }

            contents = null;
            return false;
        }
    }
}