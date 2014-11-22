using Genesis.Management.WP8;

namespace Genesis.Common
{
    public class StageManager
    {
        private readonly GameDefinitionLoader gameDefinitionLoader;

        private int currentStageNumber;

        private Stage currentStage;

        public StageManager(GameDefinitionLoader gameDefinitionLoader)
        {
            this.gameDefinitionLoader = gameDefinitionLoader;
        }

        public int NumberOfStages { get; set; }

        public void LoadNextStage()
        {
        }

        public void Initialize()
        {
            this.NumberOfStages = this.gameDefinitionLoader.LoadAllStages().Count;
        }
    }
}