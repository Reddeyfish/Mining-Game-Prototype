﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MovementTutorialObjective : ResettingObjective {
    Transform player;
    Vector3 basePlayerPos;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        basePlayerPos = player.position;
        
        //tutorial tip

        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("Welcome to Chromatose! Use <color=yellow>WASD </color> or <color=yellow>the Arrow Keys</color> to move. Try moving to the right.");
	}

    void FixedUpdate()
    {
        if (player.position.x - basePlayerPos.x > 5)
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
