using System;
using System.Reflection;

namespace Genesis.Common
{
    public class EnemyWaveDefinition
    {
        public int Order { get; set; }

        public int NumberOfMilliSecondsBetweenEnemies { get; set; }

        public EnemyDefinitionCollection EnemyDefinitions { get; set; }

        public BossDefinition BossDefinition { get; set; }

        //public void SetValue(string propertyName, int value)
        //{
        //    Type type = this.GetType();
        //    PropertyInfo propertyInfo = type.GetProperty(propertyName);
        //    propertyInfo.SetValue(this, value, null);
        //}
    }
}