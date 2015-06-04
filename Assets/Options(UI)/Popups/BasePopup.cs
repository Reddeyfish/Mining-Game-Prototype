using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BasePopup : MonoBehaviour, ISpawnable
{
    Transform canvas;
    CanvasGroup group;
    Transform child;
    public float defaultFinalHeight = 1;
    public float finalHeight { get; set; }
    public float totalTime = 1f;
    public float preferredOffset = 0.5f;
    protected virtual void Awake()
    {
        //to get around ordering issues (create is sometimes called before start)
        canvas = transform.Find("UI");
        group = canvas.GetComponent<CanvasGroup>();
    }

    public virtual void Create()
    {
        this.finalHeight = defaultFinalHeight;
        this.transform.SetParent(GameObject.FindGameObjectWithTag(Tags.player).transform);
        foreach (Transform t in this.transform.parent)
        {
            if (t.CompareTag(Tags.popup) && t != this.transform)
            {
                t.SetParent(this.transform);
                child = t;
                t.GetComponent<BasePopup>().finalHeight += preferredOffset - this.finalHeight;
            }
        }
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        group.alpha = 1;
        float time = 0;
        Transform thi = this.transform;
        while (time < totalTime)
        {
            float lerp = time / totalTime;
            //floating upward

            /*
             * old absolute system
            thi.localPosition = new Vector3(0, finalHeight * (1 - Mathf.Pow(lerp - 1, 4)), 0);
             */

            //relative/recursive system
            thi.localPosition += new Vector3(0, -3 * finalHeight * Mathf.Pow(lerp - 1, 3) * Time.fixedDeltaTime); //I actually had to use calculus for once!

            //and fading out
            group.alpha = 1 - Mathf.Pow(lerp, 2);

            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }

        //reset stuff
        if (child != null)
        {
            child.SetParent(this.transform.parent);
            child.GetComponent<BasePopup>().finalHeight += this.transform.localPosition.y;
            BasePopup parent = GetComponentInParent<BasePopup>();
            if (parent != null)
            {
                parent.newChild(child);
            }
        }

        thi.localPosition = Vector3.zero;
        this.transform.parent = null;
        SimplePool.Despawn(this.gameObject);
    }

    public void newChild(Transform child)
    {
        this.child = child;
    }
}
