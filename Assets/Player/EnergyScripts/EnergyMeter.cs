using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ComboListenerSystem))]
public class EnergyMeter: MonoBehaviour, IObliterable {
    private ComboListenerSystem listeners;
    private EnergyView view;
    private ComboProgressView progress;
    private Vector3 position;
    private Transform player;
    private ScreenFlash flash;
    private float remainingDrainTime;
    private float _startDrainTime = 60.0f;
    private float explosionDamage = 10f;
    public string energyDeathTip = "You ran out of <color=yellow>energy</color> and died.";
    public float energyDeathTipDuration = 10f;
    public float StartDrainTime
    {
        get { return _startDrainTime; }
        set
        {
            _startDrainTime = value;
            updateMeter();
        }
    }

	// Use this for initialization

    void Start()
    {
        view = GameObject.FindGameObjectWithTag(Tags.energyUI).GetComponent<EnergyView>();
        progress = GameObject.FindGameObjectWithTag(Tags.comboProgressUI).GetComponent<ComboProgressView>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        flash = GameObject.FindGameObjectWithTag(Tags.screenFlash).GetComponent<ScreenFlash>();
        position = player.position;
        remainingDrainTime = _startDrainTime;
    }

    void FixedUpdate()
    {
        if (position != player.position) //only drain when moving
        {
            updateMeter();
            remainingDrainTime -= Time.fixedDeltaTime;
            position = player.position;
        }
    }

    public void Obliterate()
    {
        remainingDrainTime -= explosionDamage;
        float timerLevel = remainingDrainTime / _startDrainTime;
        view.takeEnergyHit(timerLevel);
        flash.Flash(0.5f);
        CheckDeath();
    }

    void updateMeter()
    {
        float timerLevel = remainingDrainTime / _startDrainTime;
        view.setFillLevel(timerLevel);
        CheckDeath();
    }

    public void Add(float amount)
    {
        remainingDrainTime += amount;
        if (remainingDrainTime > _startDrainTime)
            remainingDrainTime = _startDrainTime;
        updateMeter();
    }

    void CheckDeath()
    {
        //warning for low energy should be in the view
        if (remainingDrainTime <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die() //might need to move this to it's own seperate behavior
    {
        remainingDrainTime = _startDrainTime;
        this.GetComponent<Inventory>().Wipe();
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return flash.Fade(1.5f);

        this.transform.position = Vector3.zero;
        Camera.main.transform.parent.parent.position = Vector3.zero;
        WorldController.thi.RecreateCreatedBlocks();
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTimedTip(energyDeathTip, energyDeathTipDuration);
    }
}

