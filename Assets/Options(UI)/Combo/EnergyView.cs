using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnergyView : MonoBehaviour {
    private Slider slider;
    private Image fill;
    private Transform player;

    public AudioClip ready;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

	// Use this for initialization
	void Start () {
        fill = transform.Find("Fill Area/Fill").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        setFillLevel(1);
	}

    public void setFillLevel(float level)
    {
        slider.value = Mathf.Clamp01(level);
        updateColor();
    }

    public void updateColor()
    {
        Vector3 color = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y));
        fill.color = HSVColor.HSVToRGB(color.x, 1, 0.75f + 0.125f * color.z);
    }
}
