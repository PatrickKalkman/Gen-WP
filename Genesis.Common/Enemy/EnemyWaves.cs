using System.Collections.Generic;

namespace Genesis.Common.Enemy
{
    public class EnemyWaves : List<EnemyWave>
    {
        public int NumberOfMilliSecondsBetweenEnemies { get; set; }
    }
}