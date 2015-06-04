using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PickupPopup : BasePopup, ISpawnable {
    Image image;
    Text text;
    AudioSource source;
    public float basePitch;
    public float pitchVariance;
    private Color _color;
    public Color color { get{return _color;} set {_color = value; image.color = value;} }
    private int _count;
    public int count { get { return _count; } set { _count = value; text.text = "x" + value.ToString(); } }

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();

        //to get around ordering issues (create is sometimes called before start)
        image = transform.Find("UI/Icon").GetComponent<Image>();
        text = transform.Find("UI/Label").GetComponent<Text>();
    }

    public override void Create()
    {
        //audio
        source.pitch = basePitch + Random.value * pitchVariance;
        source.Play();
        base.Create();
    }
}
