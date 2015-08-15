using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
public class AbilityController : MonoBehaviour, IItemsListener {
    ItemsView itemsView;
    List<Item> abilityItems;
    public KeyCode[] codes;

    void Start()
    {
        //itemsView is deactivated, so we have to traverse through child trees

        itemsView = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel/SlotsOutline/SlotsBackground").GetComponent<ItemsView>();

        //itemsView = GameObject.FindGameObjectWithTag(Tags.itemSlots).GetComponent<ItemsView>();
        itemsView.Subscribe(this);
        Notify(null); //setup
    }

	void Update () {

        //input to abilities
        for (int i = 0; i < abilityItems.Count; i++)
        {
            if (Input.GetKeyDown(codes[i]))
                ((BaseActiveAbility)(abilityItems[i].component)).Activate();
        }
	}

    public void Notify(Item message)
    {
        //clear the old
        foreach (Transform child in transform)
        {
            SimplePool.Despawn(child.gameObject);
        }
        abilityItems = itemsView.Items.Where(x => x.component is BaseActiveAbility).ToList(); //filter it to only abilities
        for (int i = 0; i < abilityItems.Count; i++)
        {
            Assert.IsNotNull(theUpgradeData.IDToUI(abilityItems[i].ID));
            Transform abilityUITransform = SimplePool.Spawn(theUpgradeData.IDToUI(abilityItems[i].ID)).transform;
            abilityUITransform.SetParent(this.transform, false);
            ((BaseActiveAbility)(abilityItems[i].component)).Initialize(abilityUITransform, i+1); //start at one
        }
    }

    public void OnDestroy()
    {
        itemsView.UnSubscribe(this);
    }
}
