using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnergyView : MonoBehaviour {
    private Slider slider;
    private Image fill;
    private Transform player;
    private IEnumerator hit;
    private AudioSource source;
    private float levelTarget;
    private float lerpSpeed = 0.2f;
    public AudioClip ready;
    public AudioClip drain;
    void Awake()
    {
        slider = GetComponent<Slider>();
        source = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
        fill = transform.Find("Fill Area/Fill").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        setFillLevel(1);
	}

    public void setFillLevel(float level)
    {
        if (hit == null)
        {
            slider.value = Mathf.Clamp01(level);
            updateColor();
        }
        else
            levelTarget = level;
    }

    public void updateColor()
    {
        Vector3 color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y));
        fill.color = HSVColor.HSVToRGB(color.x, 1, 0.75f + 0.125f * color.z);
    }

    public void boost()
    {
        source.clip = ready;
        source.Play();
    }

    public void takeEnergyHit(float level)
    {
        levelTarget = level;
        if (hit == null)
        {
            hit = takeEnergyHitRoutine();
            StartCoroutine(hit);
        }
        /*
        source.clip = drain;
        source.Play();
         * */
    }

    public IEnumerator takeEnergyHitRoutine()
    {
        float startLevel = slider.value;
        while (Mathf.Abs(slider.value - levelTarget) > 0.02f)
        {
            updateColor();
            fill.color = Color.Lerp(fill.color, Color.white, Mathf.Abs((slider.value - levelTarget) / (startLevel - levelTarget)));
            slider.value += lerpSpeed * (levelTarget - slider.value);
            yield return new WaitForFixedUpdate();
        }
        hit = null;
    }
}
