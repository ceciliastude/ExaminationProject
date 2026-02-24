using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public float songBPM;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;

    public AudioClip songClip;   
    private MusicManager musicManager;

    void Start()
    {
        musicManager = MusicManager.Instance;  
        if (musicManager == null)
        {
            Debug.LogError("No MusicManager found in scene!");
            return;
        }
    }

    public void StartMusic()
    {
        secPerBeat = 60f / songBPM;
        dspSongTime = (float)AudioSettings.dspTime;

        // Play through MusicManager
        musicManager.PlayMusic(songClip);
    }

    void Update()
    {
        if (musicManager != null && musicManager.MusicSource.isPlaying)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);
            songPositionInBeats = songPosition / secPerBeat;
        }
    }
}
