using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//there should only be one of these, placed on the component that will play the music

//Options should be in the Tag static class


public class VolumeController : MonoBehaviour
{
    //	public Text timerText;
    //	public float timer = 0.0f;
    private AudioSource music;
    public static VolumeController manager; //singleton, so we can make stuff static
    // Use this for initialization
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.ignoreListenerVolume = true;
        if (PlayerPrefs.HasKey(Options.SoundLevel)) // set volumes from stored values
            AudioListener.volume = PlayerPrefs.GetInt(Options.SoundLevel) / 100.0f;
        if (PlayerPrefs.HasKey(Options.MusicLevel)) // set volumes from stored values
            music.volume = PlayerPrefs.GetInt(Options.MusicLevel) / 100.0f;
        //...wait really there's no Text initialization
        manager = this;
    }

    public static void DoUpdate()
    {
        AudioListener.volume = (PlayerPrefs.GetInt(Options.SoundLevel)) / 100.0f;
        manager.music.volume = (PlayerPrefs.GetInt(Options.MusicLevel)) / 100.0f;
    }
}