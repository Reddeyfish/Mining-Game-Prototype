using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ComboProgressView : MonoBehaviour {
    private Slider lerpslider;
    private Image fill;
    private Slider nolerpslider;
    private Image nolerpfill;
    private Transform player;
    private CanvasGroup group;
    private AudioSource source;

    private IEnumerator lerpfilling;

    private const float readyFillMultiplier = 4f; //0.25 sec
    private const float fillSpeed = 1;

    public AudioClip ding;
    void Awake()
    {
        lerpslider = GetComponent<Slider>();
        group = GetComponent<CanvasGroup>();
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        fill = transform.Find("Fill Area/Fill").GetComponent<Image>();
        nolerpslider = transform.Find("no-lag slider").GetComponent<Slider>();
        nolerpfill = transform.Find("no-lag slider/Fill Area/Fill").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
    }

    private IEnumerator HideRoutine()
    {
        fill.color = Color.clear;
         //nolerpfill doesn't have the glow, so it's not visible when empty
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
    }

    private IEnumerator FillRoutine(float level)
    {
        nolerpslider.value = level;

        while (Mathf.Abs(level - lerpslider.value) > 0.005)
        {
            lerpslider.value += Time.fixedDeltaTime * fillSpeed * (level - lerpslider.value);
            yield return new WaitForFixedUpdate();
        }

        lerpslider.value = level;

        lerpfilling = null;
    }

    public void setFillLevel(float level)
    {
        
        if (lerpfilling != null)
        {
            StopCoroutine(lerpfilling);
        }
        lerpfilling = FillRoutine(Mathf.Clamp01(level));
        StartCoroutine(lerpfilling);
    }

    private IEnumerator DingRoutine(float level)
    {
        Vector2 playerPosition = player.position;
        Vector3 color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y));
        Color target = HSVColor.HSVToRGB(color.x, 1f, 0.75f + 0.125f * color.z);

        lerpslider.value = 1;
        fill.color = Color.white;
        
        nolerpslider.value = level;
        nolerpfill.color = HSVColor.HSVToRGB(color.x, 1, 0.45f + 0.075f * color.z, 0.5f);

        float lerpValue = 0;
        while (lerpValue < 1)
        {
            lerpslider.value = Mathf.Lerp(1, level, lerpValue);
            fill.color = Color.Lerp(Color.white, target, lerpValue);
            yield return new WaitForFixedUpdate();
            lerpValue += readyFillMultiplier * Time.fixedDeltaTime; //4x, so it takes 1/4th of a second
        }
        lerpslider.value = level;
        fill.color = target;
    }

    public void Ding(float level)
    {
        source.clip = ding;
        source.Play();

        if (lerpfilling != null)
        {
            StopCoroutine(lerpfilling);
            lerpfilling = null;
        }
        StartCoroutine(DingRoutine(Mathf.Clamp01(level)));
    }

    private IEnumerator ShowRoutine() //oppisite of hiding
    {
        Vector2 playerPosition = player.position;
        Vector3 color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.y));
        fill.color = HSVColor.HSVToRGB(color.x, 1f, 0.75f + 0.125f * color.z);
        nolerpfill.color = HSVColor.HSVToRGB(color.x, 1, 0.45f + 0.075f * color.z, 0.5f);
        float lerpValue = 0;
        while (lerpValue < 1)
        {
            group.alpha = lerpValue;
            yield return new WaitForFixedUpdate();
            lerpValue += readyFillMultiplier * Time.fixedDeltaTime; //4x, so it takes 1/4th of a second
        }

        group.alpha = 1;
    }

    public void Show()
    {
        StartCoroutine(ShowRoutine());
    }
}
