using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonLoadScene : MonoBehaviour {

    void Start()
    {
        if (!(PlayerPrefs.HasKey(PlayerPrefKeys.tutorialComplete) && PlayerPrefsX.GetBool(PlayerPrefKeys.tutorialComplete))) //if data not detected
        {
            transform.Find("Continue/Button").GetComponent<Button>().interactable = false;
        }
    }

    public void LoadScene(string name)
    {
        Application.LoadLevel(name);
    }

    public void wipeData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnNewGamePressed()
    {
        if (PlayerPrefs.HasKey(PlayerPrefKeys.tutorialComplete) && PlayerPrefsX.GetBool(PlayerPrefKeys.tutorialComplete)) //if saved value is true, then there is data
        {
            GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("DataWipeDialog").gameObject.SetActive(true);
        }
        else
        {
            LoadScene(Scenes.tutorial);
        }
    }
}
