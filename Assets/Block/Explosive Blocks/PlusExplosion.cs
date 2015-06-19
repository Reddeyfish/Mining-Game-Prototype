using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlusExplosion : ExplosiveBlockExplosion {

    private static Vector2 size = new Vector2(0.8f, 0.8f); //should be const

    public override void Instantiate(float hue)
    {
        transform.Find("SubEmitter").GetComponent<ParticleSystem>().startColor = HSVColor.HSVToRGB(hue, smokeSaturation, smokeValue);

        Color debrisColor = HSVColor.HSVToRGB(hue, debrisSaturation, debrisValue);
        InstantiateParticleSubSystem("Left", debrisColor);
        InstantiateParticleSubSystem("Right", debrisColor);
        InstantiateParticleSubSystem("Up", debrisColor);
        InstantiateParticleSubSystem("Down", debrisColor);
        
    }

    private void InstantiateParticleSubSystem(string transformName, Color debrisColor)
    {
        ParticleSystem subSystem = transform.Find(transformName).GetComponent<ParticleSystem>();
        subSystem.startColor = debrisColor;
        subSystem.Play();
    }

    protected override Collider2D[] getHits()
    {
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        results.AddRange(Physics2D.BoxCastAll(this.transform.position, size, 0f, Vector2.up, 3f)); //up
        results.AddRange(Physics2D.BoxCastAll(this.transform.position, size, 0f, Vector2.down, 3f)); //down
        results.AddRange(Physics2D.BoxCastAll(this.transform.position, size, 0f, Vector2.left, 3f)); //left
        results.AddRange(Physics2D.BoxCastAll(this.transform.position, size, 0f, Vector2.right, 3f)); //right
        Collider2D[] finalResult = new Collider2D[results.Count];
        for (int i = 0; i < results.Count; i++)
        {
            finalResult[i] = results[i].collider;
        }
        return finalResult;
    }

}
