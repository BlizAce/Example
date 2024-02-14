using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LootBunnies.Systems
{
    public class Utils
    {
        /// <summary>
        /// This is a coroutine that will wait for a given amount of time and then execute a given task.
        /// </summary>
        /// <param name="task">
        /// The task that is to be executed
        /// </param>
        /// <param name="delay">
        /// The duration in seconds before the task is executed.
        /// </param>
        /// <param name="timeScaled">
        /// If the duration is scaled to Unity's timescale or not.
        /// </param>
        public static IEnumerator DoTask(System.Action task, float delay, bool timeScaled = true)
        {
            if (timeScaled)
                yield return new WaitForSeconds(delay);
            else
                yield return new WaitForSecondsRealtime(delay);

            task();
        }

        /// <summary>
        /// Rolls a 50/50 chance to return true.
        /// </summary>
        public static bool RollBool()
        {
            return Random.Range(0, 2) == 0;
        }

        /// <summary>
        /// Rolls a specified chance to return true.
        /// </summary>
        /// <param name="chance">
        /// The specified chance out of 100.
        /// </param>
        public static bool RollChance(int chance)
        {
            return Random.Range(0, 101) < chance;
        }

        /// <summary>
        /// Scales a specified value of minimum and maximum value to a minimum and maximum scale.
        /// </summary>
        /// <param name="value">
        /// The value that is to be scaled.
        /// </param>
        /// <param name="min">
        /// The minimum of the unscaled value.
        /// </param>
        /// <param name="max">
        /// The maximum of the unscaled value.
        /// </param>
        /// <param name="minScale">
        /// The minimum that the value can be scaled to.
        /// </param>
        /// <param name="maxScale">
        /// The maximum that the value can be scaled to.
        /// </param>
        public static float ScaleValueBetween(float value, float min, float max, float minScale, float maxScale)
        {
            return minScale + (value - min) / (max - min) * (maxScale - minScale);
        }

        /// <summary>
        /// Scales a minimum and maximum vector between a minimum and maximum scale.
        /// </summary>
        /// <param name="position">
        /// The origin position vector.
        /// </param>
        /// <param name="location">
        /// The end location vector.
        /// </param>
        /// <param name="distance">
        /// The distance between the two vectors.
        /// </param>
        /// <param name="scale">
        /// The scale that the vector is to be scaled to.
        /// </param>
        public static Vector3 ScaleVectorBetween(Vector3 position, Vector3 location, float distance, float scale)
        {
            return new Vector3(
                ScaleValueBetween(location.x, position.x - distance, position.x + distance, -scale, scale),
                ScaleValueBetween(location.y, position.y - distance, position.y + distance, -scale, scale));
        }

        /// <summary>
        /// Gets the midway point between a collection of vectors.
        /// </summary>
        /// <param name="vectors">
        /// The collection of vectors.
        /// </param>
        public static Vector3 GetCenterPoint(Vector3[] vectors)
        {
            float x = 0;
            float y = 0;
            float z = 0;

            foreach (Vector3 vector in vectors)
            {
                x += vector.x;
                y += vector.y;
                z += vector.z;
            }

            x /= vectors.Length;
            y /= vectors.Length;
            z /= vectors.Length;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Returns a random item from a collection.
        /// </summary>
        /// <param name="collection">
        /// The collection that the item is to be returned from.
        /// </param>
        /// <typeparam name="T">
        /// The type of the collection.
        /// </typeparam>
        public static T GetRandomItem<T>(IEnumerable<T> collection)
        {
            var enumerable = collection.ToList();
            return enumerable.ElementAt(Random.Range(0, enumerable.Count));
        }
        
        /// <summary>
        /// Returns the closest transform within a range at a location.
        /// </summary>
        /// <param name="location">
        /// The location at which the search is done at.
        /// </param>
        /// <param name="distance">
        /// The search range.
        /// </param>
        /// <param name="layers">
        /// The specified layers that the search will include.
        /// </param>
        public static Transform GetNearestTransform(Vector3 location, float distance, params string[] layers)
        {
            var colliders = Physics.OverlapSphere(location, distance, LayerMask.GetMask(layers));

            return colliders.Length > 0 ? 
                colliders
                    .OrderBy(collider => Vector3.Distance(location, collider.transform.position))
                    .First().transform :
                null;
        }
    }
}