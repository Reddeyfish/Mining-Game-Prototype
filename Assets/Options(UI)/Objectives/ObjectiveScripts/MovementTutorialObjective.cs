﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MovementTutorialObjective : ResettingObjective {
    Transform player;
    Vector3 basePlayerPos;
	// Use this for initialization

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        basePlayerPos = player.position;

        //new game, so wipe all old data
        PlayerPrefs.DeleteAll();
        player.GetComponent<Inventory>().Wipe();
        GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel/InventoryOutline/InventoryView/Content").GetComponent<BaseInventory>().Wipe();
    }

	protected void Start () {
        //tutorial tip

        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("Welcome to Chromatose! Use <color=yellow>WASD </color> or <color=yellow>the Arrow Keys</color> to move. Try moving to the <color=cyan>right</color>.");
	}

    void FixedUpdate()
    {
        if (player.position.x - basePlayerPos.x > 3)
        {
            completeObjective();
        }
    }

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 2);
    }

    protected override string getText() { return "Move to the right"; }

    public override int getID() { return 1; }
}
