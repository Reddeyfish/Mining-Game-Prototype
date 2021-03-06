﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class defaultCombo : BaseDigListener
{
    private EnergyMeter combo;
    private static Dictionary<blockDataType, float> blockValues = new Dictionary<blockDataType, float>()
    {
        {blockDataType.BOULDER, 0},
        {blockDataType.EMPTYBLOCK, 0},
        {blockDataType.LIGHTBLOCK, 0},
        {blockDataType.MAPBLOCK, 1},
        {blockDataType.TRANSPARENTMAP, 1},
        {blockDataType.EXPLOSIVE, 1},
        {blockDataType.OREBLOCK, 6},
    };

    // Use this for initialization
    protected override void Start()
    {
        combo = GetComponent<EnergyMeter>();
        base.Start();
    }

    public override void OnNotifyDig(Block block)
    {
        combo.Add(blockValues[block.getBlockType()]);
    }
}
