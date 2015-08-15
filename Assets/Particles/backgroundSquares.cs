using UnityEngine;
using System.Collections;

public class backgroundSquares : MonoBehaviour {
    public static float perlinSeedX;
    public static float perlinSeedY;
    public static float perlinSeedXOffset = 0;

    public float updatePeriod = 0.1f;
    public Vector2 squareSize;
    public Vector2 gapSize;
    public GameObject squarePrefab;

	// Use this for initialization
	void Start () {
        perlinSeedX = Random.Range(-9999, 9999) / 11;
        perlinSeedY = Random.Range(-9999, 9999) / 11;

        int numXSquares = Mathf.CeilToInt(Screen.width / (2 * (squareSize.x + gapSize.x)));
        int numYSquares = Mathf.CeilToInt(Screen.height / (2 * (squareSize.y + gapSize.y)));

        Transform parent = this.transform;

        for (int x = -numXSquares; x <= numXSquares; x++)
        {
            for (int y = -numYSquares; y <= numYSquares; y++)
            {
                Transform spawnedSquare = (Instantiate(squarePrefab, new Vector2(x * (squareSize.x + gapSize.x), y * (squareSize.y + gapSize.y)), Quaternion.identity) as GameObject).transform;
                spawnedSquare.SetParent(parent, Vector3.one);
                spawnedSquare.GetComponent<mainMenuSquare>().Initialize(squareSize, x, y);
            }
        }

        StartCoroutine(updateColorSeed());
	}

    IEnumerator updateColorSeed()
    {
        while (true)
        {
            perlinSeedXOffset++;
            yield return new WaitForSeconds(updatePeriod);
        }
    }

}
