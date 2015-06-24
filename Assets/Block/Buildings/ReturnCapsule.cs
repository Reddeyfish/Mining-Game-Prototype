using UnityEngine;
using System.Collections;

public class ReturnCapsule : MonoBehaviour {
    CanvasGroup UI;
    IEnumerator inputCheck; //store the IEnumerator so we can stop it when it is no longer needed
    KeyCode code = KeyCode.Space;
	// Use this for initialization
	void Awake () {
        UI = transform.Find("UI").GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
	
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

                //activate the return
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
