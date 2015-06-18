using UnityEngine;
using System.Collections;

public class TestGrammar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string grammar = "abc\nBC\nBCab\nCc:a\nBb";
		Grammar test = new Grammar();
		test.loadFromString(grammar);
		Debug.Log(test.getInfo());
	}
}
