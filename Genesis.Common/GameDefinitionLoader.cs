using Genesis.Common;

namespace Genesis.Management.WP8
{
    using System;

    public class GameDefinitionLoader
    {
        private readonly IGameDefinitionXmlReader gameDefinitionXmlReader;

        private readonly IGameDefinitionParser gameDefinitionParser;

        private string stageContentXmlFilename;

        private StageCollection stageCollection;

        public GameDefinitionLoader(string stageContentXmlFilename, IGameDefinitionXmlReader gameDefinitionXmlReader, IGameDefinitionParser gameDefinitionParser) 
            : this(gameDefinitionXmlReader, gameDefinitionParser)
        {
            this.stageContentXmlFilename = stageContentXmlFilename;
        }

        public GameDefinitionLoader(IGameDefinitionXmlReader gameDefinitionXmlReader, IGameDefinitionParser gameDefinitionParser)
        {
            this.gameDefinitionXmlReader = gameDefinitionXmlReader;
            this.gameDefinitionParser = gameDefinitionParser;
        }

        public virtual GameDefinition LoadAllStages(string stageContentXmlFilename)
        {
            this.stageContentXmlFilename = stageContentXmlFilename;
            var gameDefinition = new GameDefinition();
            gameDefinition.Stages = this.LoadAllStages();
            return gameDefinition;
        }

        public virtual StageCollection LoadAllStages()
        {
            this.ValidateState();
            string stageXml;
            if (this.gameDefinitionXmlReader.ReadAll(this.stageContentXmlFilename, out stageXml))
            {
                return ParseAllStagesContent(stageXml);
            }

            return null;
        }
        
        public GameDefinition LoadAllStagesContent(string gameDefinitionContent)
        {
            var gameDefinition = new GameDefinition();
            gameDefinition.Stages = this.ParseAllStagesContent(gameDefinitionContent);
            return gameDefinition;
        }

        public StageCollection ParseAllStagesContent(string gameDefinitionContent)
        {
            stageCollection = this.gameDefinitionParser.Parse(gameDefinitionContent);
            return stageCollection;
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(this.stageContentXmlFilename))
            {
                throw new InvalidOperationException(
                    "Please set a correct stage content file name before calling LoadAllStages.");
            }
        }
    }
}