using UnityEngine;
using System.Collections;


//ore block with special code to detect explosions

public class TutorialOreBlock : OreBlock {

    IExplosionListener listener;

    protected override void OnDetonate()
    {
        listener.OnNotifyExplosion(this);
    }

    public void Instantiate(IExplosionListener listen)
    {
        listener = listen;
    }
}

public interface IExplosionListener
{
    void OnNotifyExplosion(ExplosiveBlock block);
}