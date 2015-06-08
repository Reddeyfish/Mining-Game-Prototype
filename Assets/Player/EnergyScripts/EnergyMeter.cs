using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ComboListenerSystem))]
public class EnergyMeter: MonoBehaviour {
    private ComboListenerSystem listeners;
    private EnergyView view;
    private ComboProgressView progress;
    private Vector3 position;
    private Transform player;
    private float remainingDrainTime;
    private float _startDrainTime = 60.0f;
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

    void updateMeter()
    {
        float timerLevel = remainingDrainTime / _startDrainTime;
        view.setFillLevel(timerLevel);
    }

    public void Add(float amount)
    {
        remainingDrainTime += amount;
        if (remainingDrainTime > _startDrainTime)
            remainingDrainTime = _startDrainTime;
    }
}

