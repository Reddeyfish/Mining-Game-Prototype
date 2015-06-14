using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
public class AbilityController : MonoBehaviour {
    List<Item> abilityItems;
    public KeyCode[] codes;
	void Update () {
        for (int i = 0; i < abilityItems.Count; i++)
        {
            if (Input.GetKeyDown(codes[i]))
                ((BaseActiveAbility)(abilityItems[i].component)).Activate();
        }
	}

    public void notifyChanged(List<Item> items)
    {
        foreach (Transform child in transform)
        {
            SimplePool.Despawn(child.gameObject);
        }
        abilityItems = items.Where(x => x.component is BaseActiveAbility).ToList(); //filter it to only abilities
        for (int i = 0; i < abilityItems.Count; i++)
        {
            Assert.IsNotNull(theUpgradeData.IDToUI(abilityItems[i].ID));
            Transform abilityUITransform = SimplePool.Spawn(theUpgradeData.IDToUI(abilityItems[i].ID)).transform;
            abilityUITransform.SetParent(this.transform);
            abilityUITransform.localScale = Vector3.one;
            ((BaseActiveAbility)(abilityItems[i].component)).Initialize(abilityUITransform, i+1); //start at one
        }
    }
}
