using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnergyView : MonoBehaviour {
    private Slider slider;
    private Image fill;
    private Transform player;
    private IEnumerator hit;
    private IEnumerator warn;
    private AudioSource source;
    private float levelTarget;
    private Color targetColor;
    private const float lerpSpeed = 0.2f;
    private const float warnLevel = 0.2f;
    private const float warnFlashTime = 0.5f;
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
        levelTarget = Mathf.Clamp01(level);
        if (hit == null)
        {
            slider.value = levelTarget;
            updateColor();
        } 
    }

    public void updateColor()
    {
        Vector3 color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y));
        targetColor = HSVColor.HSVToRGB(color.x, 1, 0.75f + 0.125f * color.z);
        if (warn == null)
        {
            fill.color = targetColor;
            CheckWarn();
        }
    }

    public void boost()
    {
        source.clip = ready;
        source.Play();
    }

    public void takeEnergyHit(float level)
    {
        levelTarget = Mathf.Clamp01(level);
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
        if (warn != null)
        {
            StopCoroutine(warn);
            warn = null;
        }
        float startLevel = slider.value;
        while (Mathf.Abs(slider.value - levelTarget) > 0.02f)
        {
            updateColor();
            fill.color = Color.Lerp(fill.color, Color.white, Mathf.Abs((slider.value - levelTarget) / (startLevel - levelTarget)));
            slider.value += lerpSpeed * (levelTarget - slider.value);
            yield return new WaitForFixedUpdate();
        }
        hit = null;
        CheckWarn();
    }

    public IEnumerator warnRoutine()
    {
        
        while (levelTarget < warnLevel)
        {
            float time = 0;
            Debug.Log("Blink!");
            fill.color = Color.white;
            while (time < warnFlashTime)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }
            time = 0;

            while (time < warnFlashTime)
            {
                fill.color = targetColor;
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }
        }
        warn = null;
    }

    public void CheckWarn()
    {
        if (levelTarget < warnLevel && warn == null)
        {
            warn = warnRoutine();
            StartCoroutine(warn);
        }
    }
}
