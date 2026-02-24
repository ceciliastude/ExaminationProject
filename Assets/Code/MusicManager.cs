using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public static MusicManager Instance = null;

    private Coroutine endCheckCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();

        
        if (endCheckCoroutine != null)
            StopCoroutine(endCheckCoroutine);

        if (SceneManager.GetActiveScene().name == "Rhytmic Gymnastics")
        {
            endCheckCoroutine = StartCoroutine(CheckForMusicEnd());
        }
    }

    private IEnumerator CheckForMusicEnd()
    {
        yield return new WaitWhile(() => MusicSource.isPlaying);
		Debug.Log("Music ending...loading rhytmic gymnastics function");
        RhytmicGymnastics();
    }

    private void RhytmicGymnastics()
    {
        RhythmGameManager startEndOfEvent = FindObjectOfType<RhythmGameManager>();
        if (startEndOfEvent != null)
        {
			Debug.Log("Loading musicends function");
            startEndOfEvent.MusicEnds();	
        }
		
    }
}
