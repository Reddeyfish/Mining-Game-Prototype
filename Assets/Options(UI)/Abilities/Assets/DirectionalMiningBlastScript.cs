using UnityEngine;
using System.Collections;

public class DirectionalMiningBlastScript : BaseMiningAbility
{
    private const float range = 8.5f;

    protected override Collider2D[] getHits()
    {
        RaycastHit2D[] results = Physics2D.RaycastAll(transform.position - transform.right.normalized, transform.right, range, mask);
        Collider2D[] finalResult = new Collider2D[results.Length];
        for (int i = 0; i < results.Length; i++)
            finalResult[i] = results[i].collider;
        return finalResult;
    }
}
