using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ComboView : MonoBehaviour {
    private Slider slider;
    private Image fill;
    private Transform player;
    private CanvasGroup group;
    private AudioSource source;
    private Animator handleAnimator;
    private bool toPulse;

    private const float readyFillMultiplier = 4f; //0.25 sec
    private const float pulseMultiplier = 1f; // 1 sec
    private static int activeParam = Animator.StringToHash("Active");

    public AudioClip ready;
    public AudioClip start;
    public AudioClip end;
    void Awake()
    {
        slider = GetComponent<Slider>();
        group = GetComponent<CanvasGroup>();
        source = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
        fill = transform.Find("Fill Area/Fill").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        handleAnimator = transform.Find("Handle Slide Area/Handle").GetComponent<Animator>();
	}

    private IEnumerator FillRoutine()
    {
        float startLevel = slider.value;
        float lerpValue = 0;
        while (lerpValue < 1)
        {
            slider.value = Mathf.Lerp(startLevel, 1, lerpValue);

            group.alpha = lerpValue;

            yield return new WaitForFixedUpdate();
            lerpValue += readyFillMultiplier * Time.fixedDeltaTime; //4x, so it takes 1/4th of a second total
        }
        slider.value = 1;
        group.alpha = 1;
    }

    public void Fill()
    {
        StartCoroutine(FillRoutine());
        if(source.isPlaying)
            source.Stop();
        source.clip = ready;
        source.Play();
    }

    private IEnumerator HideRoutine()
    {
        fill.color = Color.clear;
        float lerpValue = 1;
        while (lerpValue > 0)
        {
            group.alpha = lerpValue;
            yield return new WaitForFixedUpdate();
            lerpValue -= readyFillMultiplier * Time.fixedDeltaTime; //4x, so it takes 1/4th of a second
        }

        group.alpha = 0;

    }

    public void Hide()
    {
        StartCoroutine(HideRoutine());
        if (source.isPlaying)
            source.Stop();
        source.clip = end;
        source.Play();
        handleAnimator.SetBool(activeParam, false);
    }

    private IEnumerator PulseAnimation()
    {
        float lerpValue = 1;
        Vector2 playerPosition;
        Vector3 color;
        while (toPulse)
        {
            //fade out
            while (lerpValue > 0)
            {
                playerPosition = player.position;
                color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y));
                fill.color = Color.Lerp(Color.white, HSVColor.HSVToRGB(color.x, 1, 0.75f + 0.125f * color.z), lerpValue);
                yield return new WaitForFixedUpdate();
                lerpValue -= Time.fixedDeltaTime * pulseMultiplier;
            }
            lerpValue = 0;
            //fade in
            while (lerpValue < 1)
            {
                playerPosition = player.position;
                color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y));
                fill.color = Color.Lerp(Color.white, HSVColor.HSVToRGB(color.x, 1, 0.75f + 0.125f * color.z), lerpValue);
                yield return new WaitForFixedUpdate();
                lerpValue += Time.fixedDeltaTime * pulseMultiplier;
            }
            lerpValue = 1;
        }

        playerPosition = player.position;
        color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y));
        fill.color = HSVColor.HSVToRGB(color.x, 1, 0.75f + 0.125f * color.z);
    }

    public void Drain()
    {
        if (source.isPlaying)
            source.Stop();
        source.clip = start;
        source.Play();
        handleAnimator.SetBool(activeParam, true);
    }

    public void Pulse(bool yes)
    {
        if (yes && !toPulse)
        {
            toPulse = true;
            StartCoroutine(PulseAnimation());
        }
        else if (!yes && toPulse)
        {
            toPulse = false;
        }
    }

    public void setFillLevel(float level)
    {
        slider.value = Mathf.Clamp01(level);
    }
}
