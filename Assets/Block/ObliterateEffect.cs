using UnityEngine;
using System.Collections;

public class ObliterateEffect : MonoBehaviour, ISpawnable
{
    Material mat;
    public float fadeTime = 0.25f;
    void Awake()
    {
        mat = transform.Find("Visuals").GetComponent<Renderer>().material;
    }
    public void Create()
    {
        StartCoroutine(MainRoutine());
    }
    IEnumerator MainRoutine()
    {
        float time = 0;
        while (time < fadeTime)
        {
            mat.color = Color.Lerp(Color.white, Color.clear, time / fadeTime);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
        SimplePool.Despawn(this.gameObject);
    }
}
