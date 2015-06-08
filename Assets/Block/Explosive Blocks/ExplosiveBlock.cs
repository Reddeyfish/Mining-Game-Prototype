﻿using UnityEngine;
using System.Collections;

public class ExplosiveBlock : Block, IDigListener {
    Material mat;
    private DiggingListenerSystem listener;
    
    private float hue;
    private bool stable = true;
    private const float maxEmission = 1.2f;
    private const float radius = 1.9f;
    private const float detonationTime = 2f;
    private const float detonationVariance = 8f;
    private const float detonationShake = 0.1f;
    public GameObject deathEffect;
    public GameObject explosion;
    // Use this for initialization
    void Awake()
    {
        transform.Find("Visuals").localScale = new Vector3(1, 1, 0.75f + 0.3f * (Random.value));
        listener = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<DiggingListenerSystem>();
    }

    public override void Create()
    {
        listener.Subscribe(this);
        stable = true;
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        mat = transform.Find("Visuals").GetComponent<Renderer>().material;
        mat.color = HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z);
        hue = (colorValues.x + 0.425f + 0.15f * Random.value) % 1;
        StartCoroutine(EmissionPulse());
    }

    IEnumerator EmissionPulse()
    {
        Color color= HSVColor.HSVToRGB(hue, 0.4f, 1);
        float lerpvalue = Random.value;
        float pulseSpeed = (1 + Random.value) / 8;
        while (true)
        {
            while (lerpvalue < 1)
            {
                mat.SetColor(ShaderParams.emission, color * (0.3f + maxEmission * lerpvalue));
                yield return new WaitForFixedUpdate();
                lerpvalue += Time.fixedDeltaTime * pulseSpeed;
            }
            lerpvalue = 1;
            while (lerpvalue > 0)
            {
                mat.SetColor(ShaderParams.emission, color * (0.3f + maxEmission * lerpvalue));
                yield return new WaitForFixedUpdate();
                lerpvalue -= Time.fixedDeltaTime * pulseSpeed;
            }
            lerpvalue = 0;
        }
    }

    IEnumerator Detonate()
    {
        stable = false;
        float time = 0;
        Color color = HSVColor.HSVToRGB(hue, 1f, 1);
        Transform visuals = transform.Find("Visuals");
        while (time < 1)
        {
            mat.SetColor(ShaderParams.emission, color * detonationVariance * Random.value);
            visuals.localPosition = Random.insideUnitSphere * detonationShake;
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime / detonationTime;
        }
        listener.UnSubscribe(this);
        SimplePool.Spawn(explosion, this.transform.position).GetComponent<ExplosiveBlockExplosion>().Instantiate(hue);
        Despawn();

    }

    public void OnNotify(Block block)
    {
        if (block != this && stable && (block.transform.position - this.transform.position).magnitude < radius)
        {
            StopAllCoroutines();
            StartCoroutine(Detonate());
        }
    }

    public override void Despawn()
    {
        listener.UnSubscribe(this);
        stable = true;
        base.Despawn();
    }

    public override void Destroy()
    {
        if (stable)
            SimplePool.Spawn(deathEffect, this.transform.position);
        else
        {
            SimplePool.Spawn(explosion, this.transform.position).GetComponent<ExplosiveBlockExplosion>().Instantiate(hue);
        }
        listener.UnSubscribe(this);
        base.Destroy();
    }

    public override void Obliterate()
    {
        if (stable)
        {
            StopAllCoroutines();
            StartCoroutine(Detonate());
        }
    }

    public override Color getColor()
    {
        return transform.Find("Visuals").GetComponent<Renderer>().material.color;
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.EXPLOSIVE;
    }

    public void OnDestroy()
    {
        listener.UnSubscribe(this);
    }
    
}