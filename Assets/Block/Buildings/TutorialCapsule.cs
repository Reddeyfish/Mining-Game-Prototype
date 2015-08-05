using UnityEngine;
using System.Collections;

public class TutorialCapsule : Block
{
    CanvasGroup UI;
    IEnumerator inputCheck; //store the IEnumerator so we can stop it when it is no longer needed
    KeyCode code = KeyCode.Space;
    CapsuleTutorialObjective listener;
    void Awake()
    {
        UI = transform.Find("UI").GetComponent<CanvasGroup>();
    }

    public void Instantiate(CapsuleTutorialObjective listener)
    {
        this.listener = listener;
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            UI.alpha = 1;
            inputCheck = InputCheck();
            StartCoroutine(inputCheck);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            UI.alpha = 0;
            StopCoroutine(inputCheck);
        }
    }

    IEnumerator InputCheck()
    {
        while (true)
        {
            if (Input.GetKey(code))
            {
                //then they've pressed the key

                listener.completeObjective();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.CAPSULE;
    }
}
