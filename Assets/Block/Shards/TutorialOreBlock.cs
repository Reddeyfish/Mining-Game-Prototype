using UnityEngine;
using System.Collections;


//ore block with special code to detect explosions

public class TutorialOreBlock : OreBlock {

    IExplosionListener listener;

    new protected IEnumerator Detonate()
    {
        yield return StartCoroutine(base.Detonate());
        listener.OnNotifyExplosion(this);
    }

    public void Instantiate(IExplosionListener listener)
    {
        this.listener = listener;
    }
}

public interface IExplosionListener
{
    void OnNotifyExplosion(ExplosiveBlock block);
}