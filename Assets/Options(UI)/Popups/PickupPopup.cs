using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PickupPopup : BasePopup, ISpawnable {
    Image icon;
    Image image;
    Text text;
    AudioSource source;
    public float basePitch;
    public float pitchVariance;
    private Color _color;
    public Color color { get{return _color;} set {_color = value; icon.color = value;} }
    private int _count;
    public int count { get { return _count; } set { _count = value; text.text = "x" + value.ToString(); } }
    resourceType _type;
    public resourceType type { get { return _type; } set { _type = value; image.sprite = value.UISprite(); } }

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();

        //to get around ordering issues (create is sometimes called before start)
        icon = transform.Find("UI/Icon").GetComponent<Image>();
        image = transform.Find("UI/Icon/Image").GetComponent<Image>();
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
