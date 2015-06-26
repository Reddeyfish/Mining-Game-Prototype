using UnityEngine;
using System.Collections;

public class ReturnCapsule : Block {
    TutorialTip tips;
    CanvasGroup UI;
    IEnumerator inputCheck; //store the IEnumerator so we can stop it when it is no longer needed
    KeyCode code = KeyCode.Space;

    private const float stopPercent = 0.2f; //the fraction of the original distance that the teleport will stop at
    private const float speed = 20f;
	// Use this for initialization
	void Awake () {
        UI = transform.Find("UI").GetComponent<CanvasGroup>();
        tips = GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>();
	}
	

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            UI.alpha = 1;
            inputCheck = InputCheck();
            StartCoroutine(inputCheck);
            tips.SetTip(TutorialTipType.CAPSULE);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            UI.alpha = 0;
            StopCoroutine(inputCheck);
            tips.EndTip(TutorialTipType.CAPSULE);
        }
    }

    IEnumerator InputCheck()
    {
        while (true)
        {
            if (Input.GetKey(code))
            {
                //then they've pressed the key

                //activate the return

                Transform player = GameObject.FindGameObjectWithTag(Tags.player).transform;
                player.GetComponent<Controls>().QueueCoroutine(TeleportBack(player));
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    //moves the player to the origin, non-instantaneous teleport.

    //not called by this script; passed to the player's control script

    //when we add other bases, we'll have to teleport to the nearest base, not the just main base as this does
    IEnumerator TeleportBack(Transform player)
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        playerCollider.enabled = false; //so the player can pass over blocks
        float stopDistance = stopPercent * player.position.magnitude;
        while (player.position.magnitude > stopDistance)
        {
            player.position -= speed * player.position.normalized * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        playerCollider.enabled = true;
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.CAPSULE;
    }

}
