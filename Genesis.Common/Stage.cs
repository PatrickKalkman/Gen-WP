namespace Genesis.Common
{
    public class Stage
    {
        public int Order { get; set; }

        public BackgroundLayerCollection BackgroundLayers { get; set; }

        public EnemyWaveCollection EnemyWaves { get; set; }
    }
}