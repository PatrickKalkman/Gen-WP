using System;

namespace Genesis.Common
{
    public class RandomGenerator
    {
        private readonly Random random = new Random(DateTime.Now.Millisecond);

        public double NextDouble()
        {
            return this.random.NextDouble();
        }
    }
}