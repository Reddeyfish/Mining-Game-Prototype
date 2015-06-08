using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ComboListenerSystem))]
public class ComboMeter : BaseDigListener {
    private ComboListenerSystem listeners;
    private IEnumerator drainingRoutine;
    private ComboState _state = ComboState.EMPTY;
    public ComboState State { get { return _state; } }
    private ComboView view;
    private ComboProgressView progress;
    private float timerLevel = 0f;
    private float remainingDrainTime;
    private int comboLevel = 0;
    private float comboAmount = 0;
    private float inputtedAdds = 0;

    private const float startDrainTime = 5.0f;

    public float[] levels = {12, 24, 36};
    public float fillMultiplier = 1f; //just in case we need it

    public enum ComboState
    {
        EMPTY,
        READY,
        DRAINING,
    }
	// Use this for initialization
    void Awake()
    {
        listeners = GetComponent<ComboListenerSystem>();
    }

    protected override void Start()
    {
        view = GameObject.FindGameObjectWithTag(Tags.comboUI).GetComponent<ComboView>();
        progress = GameObject.FindGameObjectWithTag(Tags.comboProgressUI).GetComponent<ComboProgressView>();
        base.Start();

    }

    void HandleComboMaterial()
    {
        if(inputtedAdds != 0)
        {
            if (_state == ComboState.DRAINING)
            {
                comboAmount += fillMultiplier * inputtedAdds;
                float level = comboAmount / levels[comboLevel];
                if(level >=1)
                {
                    comboAmount -= levels[comboLevel];
                    listeners.ComboNotify(comboLevel);
                    comboLevel++;
                    progress.Ding(comboAmount / levels[comboLevel]);
                    Debug.Log(comboAmount / levels[comboLevel]);
                }
                else
                {
                    progress.setFillLevel(level);
                }
            }
            inputtedAdds = 0;
        }
    }

    public override void OnNotify(Block block)
    {
        if (_state == ComboState.EMPTY && block.getBlockType() == blockDataType.OREBLOCK)
        {
            //ready the combo
            _state = ComboState.READY;
            view.Fill();
            view.Pulse(true);

        }
        else if (_state == ComboState.READY)
        {
            //start draining
            _state = ComboState.DRAINING;
            view.Pulse(false);
            progress.Show();
            remainingDrainTime = startDrainTime;
            drainingRoutine = Drain();
            StartCoroutine(drainingRoutine);
        }
    }

    private IEnumerator Drain()
    {
        view.Drain();

        float time = remainingDrainTime;
        
        while (time > 0)
        {
            timerLevel = time / remainingDrainTime;
            view.setFillLevel(timerLevel);
            yield return new WaitForFixedUpdate();
            time -= Time.fixedDeltaTime;
        }
        view.setFillLevel(0);
        view.Hide();
        progress.setFillLevel(0);
        progress.Hide();

        _state = ComboState.EMPTY;
        comboAmount = 0;
        comboLevel = 0;
    }

    public void Add(float amount)
    {
        inputtedAdds += amount; 
        Callback.FireForFixedUpdate(HandleComboMaterial, this); //saves it until the next update so that state changes can be handled properly
    }
}

