using System.Collections.Generic;

namespace Genesis.Common
{
    public class EnemyWaveCollection : List<EnemyWaveDefinition>
    {
        public int NumberOfWaves
        {
            get
            {
                return this.Count;
            }
        }
    }
}