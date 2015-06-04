using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))] // just in case someone really screws up when importing
[RequireComponent(typeof(VolumeController))]
public class MusicManager : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] BGMPlaylist; //MUST BE ASSIGNED!
    private int currentBGM = 0; //the playlist should be a queue, but it's easier on non-programmers if we make it an array
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!source.isPlaying)
        {
            currentBGM += 1;
            if (currentBGM >= BGMPlaylist.Length) //we've reached the end of the array and need to return to the beginning
                currentBGM = 0;
            source.clip = BGMPlaylist[currentBGM];
            source.Play();
        }
    }
}
