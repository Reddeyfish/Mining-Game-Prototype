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
    public float lerpSpeed = 0.2f;
    public float warnLevel = 0.2f;
    public float warnFlashTime = 0.5f;
    public string lowEnergyTip = "You are low on <color=yellow>energy</color>. Return to base and refill your energy or you will die.";
    public AudioClip ready;
    public AudioClip drain;
    public AudioClip warnSound;
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
        Callback.FireForNextFrame(updateColor, this); //give everything a chance to update first
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
        Vector3 color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));
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

    public Coroutine takeEnergyHit(float level)
    {
        Coroutine result = null;
        levelTarget = Mathf.Clamp01(level);
        if (hit == null)
        {
            hit = takeEnergyHitRoutine();
            result = StartCoroutine(hit);
        }
        return result;
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
        while (Mathf.Abs(slider.value - levelTarget) > 0.002f)
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
        TutorialTip tip = GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>();
        tip.SetTip(lowEnergyTip);
        while (levelTarget < warnLevel)
        {
            source.clip = warnSound;
            source.Play();

            yield return StartCoroutine(flashRoutine());
        }
        warn = null;
        tip.EndTip(lowEnergyTip);
    }

    public IEnumerator flashRoutine()
    {
        float time = 0;
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

    public void setWarnRoutine(IEnumerator warn) //for the tutorial
    {
        this.warn = warn;
    }

    public Coroutine CheckWarn()
    {
        Coroutine result = null;
        if (levelTarget < warnLevel && warn == null)
        {
            warn = warnRoutine();
            result = StartCoroutine(warn);
        }
        return result;
    }
}
