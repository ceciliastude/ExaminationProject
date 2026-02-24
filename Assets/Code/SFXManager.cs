using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public AudioSource audioSource;
    public AudioClip hoverSFX;
    public AudioClip pressSFX;

    public AudioClip[] gameSFX;
    private AudioSource loopSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);

            // Setup loop audio source
            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.loop = true;
            loopSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHoverSFX()
    {
        if (hoverSFX != null)
        {
            audioSource.PlayOneShot(hoverSFX);
        }
    }

    public void PlayPressSFX()
    {
        if (pressSFX != null)
        {
            audioSource.PlayOneShot(pressSFX);
        }
    }

    public void PlayHitSFX(HitResult result, int perfectStreak)
    {
        switch (result)
        {
            case HitResult.Perfect:
                audioSource.PlayOneShot(gameSFX[3], 0.5f);
                //Crowd cheer
                audioSource.PlayOneShot(gameSFX[0]);

                if (perfectStreak == 5)
                    //Crowd clapping
                    StartCrowdLoop(gameSFX[1]);
                break;

            case HitResult.Great:
            case HitResult.GreatEarly:
            case HitResult.GreatLate:
                audioSource.PlayOneShot(gameSFX[4], 0.7f);
                break;

            case HitResult.Good:
            case HitResult.GoodEarly:
            case HitResult.GoodLate:
                audioSource.PlayOneShot(gameSFX[5], 0.7f);
                break;

            case HitResult.Miss:
                audioSource.PlayOneShot(gameSFX[6], 0.7f);
                if (loopSource.isPlaying)
                    //Crowd dissapointment
                    audioSource.PlayOneShot(gameSFX[2], 0.7f);
                StopCrowdLoop();
                break;
        }
    }

    private void StartCrowdLoop(AudioClip clip)
    {
        if (loopSource.isPlaying) return;
        loopSource.clip = clip;
        loopSource.Play();
    }
    private void StopCrowdLoop()
    {
        if (loopSource.isPlaying)
        {
            loopSource.Stop();
            loopSource.clip = null;
        }
    }
}
