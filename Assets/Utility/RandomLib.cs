using UnityEngine;
using System.Collections;


public class RandomLib : MonoBehaviour {

    public static float RandFloatRange(float midpoint, float variance)
    {
        return midpoint + (variance * Random.value);
    }

    public static Vector3 PerlinColor(float seedX, float seedY, float x, float y)
    {
        return new Vector3((2.5f * Mathf.PerlinNoise(seedX + x / 151, seedY + y / 151)) % 1, 0.65f + 0.25f * Mathf.PerlinNoise(seedX + x / 101, seedX + y / 101), 0.9f - Mathf.Pow(Mathf.PerlinNoise(x / 101 - seedY, y / 101 - seedY), 2));
    }
}
