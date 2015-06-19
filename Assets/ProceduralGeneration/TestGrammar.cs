using UnityEngine;
using System.Collections;

public class TestGrammar : MonoBehaviour {
	
	// Use this for initialization
	void Start ()
	{
		string grammarStr = "abc\nBC\nBCaC\nCc:B\nBbBC";

		Grammar testGrammar = new Grammar();

		testGrammar.LoadFromString(grammarStr);

		Debug.Log(testGrammar.GetInfo());

		for (int i = 0; i < 100; i++)
		{
			Debug.Log(testGrammar.GenerateWord(5, 'F'));
		}
	}
}
