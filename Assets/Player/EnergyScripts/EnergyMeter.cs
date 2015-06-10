using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ComboListenerSystem))]
public class EnergyMeter: MonoBehaviour, IObliterable {
    private ComboListenerSystem listeners;
    private EnergyView view;
    private ComboProgressView progress;
    private Vector3 position;
    private Transform player;
    private float remainingDrainTime;
    private float _startDrainTime = 60.0f;
    private float explosionDamage = 10f;
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
        GameObject.FindGameObjectWithTag(Tags.screenFlash).GetComponent<ScreenFlash>().Flash(0.5f);
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
            //die

            //todo: add visual effects

            this.transform.position = Vector3.zero;
            Camera.main.transform.parent.parent.position = Vector3.zero;
            WorldController.thi.RecreateCreatedBlocks();
            this.GetComponent<Inventory>().Wipe();
        }
    }
}

