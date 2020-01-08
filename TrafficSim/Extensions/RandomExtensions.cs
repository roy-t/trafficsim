using System;

namespace TrafficSim.Extensions
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            var range = max - min;
            var v = (float)random.NextDouble();
            return min + (v * range);
        }

        /// <summary>
        /// Generates a random bool, with a chance factor based on the ratio, ratio in [0..1]. Where 1 is a 100% chance on true.
        /// </summary>
        public static bool NextBool(this Random random, float ratio = 0.5f) => random.NextDouble() < ratio;
    }
}
