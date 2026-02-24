using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum HitResult
{
    Perfect,
    Great,
    GreatEarly,
    GreatLate,
    Good, 
    GoodEarly,
    GoodLate,
    Miss
}


public class RhythmGameManager : MonoBehaviour
{
    public static RhythmGameManager Instance;
    [Header("Status UI")]
    public GameObject[] scoreStatus;
    public RectTransform statusSpawnParent;

    private double totalScore = 0.0;
    private bool songEnded = false;
    private int perfectStreak = 0;
    public Animator animator;
    public Conductor conductor;
    private FaceChangeAnim handler;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        handler = FindObjectOfType<FaceChangeAnim>();
    }

    
    public void AddScore(double score, HitResult result)
    {
        totalScore += score;
        //Debug.Log($"Total: {totalScore} | Result: {result}");
        if (result == HitResult.Perfect)
        {
            perfectStreak++;
        }
        else
        {
            if (perfectStreak > 0) perfectStreak = 0;
        }
        if (SFXManager.instance != null)
        {
            SFXManager.instance.PlayHitSFX(result, perfectStreak);
        }
        StartCoroutine(handler.StatusFaceChange(result));
        ShowStatus(result);

    }

    private void ShowStatus(HitResult result)
    {
        int index = (int)result;
        if (index >= 0 && index < scoreStatus.Length && scoreStatus[index] != null)
        {
            GameObject statusObj = Instantiate(scoreStatus[index], statusSpawnParent);
            statusObj.transform.localPosition = new Vector3(0f, -350f, 0f);
            Destroy(statusObj, 0.6f);
        }
    }

    public double GetTotalScore()
    {
        return totalScore;
    }

    public void GameStarts()
    {
        StartCoroutine(StartGame());
    }
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);
        animator.SetTrigger("GameStarts");
        yield return new WaitForSeconds(6f);
        conductor.StartMusic();
        animator.SetBool("isWalking", true);
        
    }

    public void MusicEnds()
    {
        animator.SetTrigger("endGame");
        animator.SetBool("isSpinning", false);
        if (songEnded) return;
        songEnded = true;

        PlayerPrefs.SetFloat("PlayerFinalScore", (float)totalScore);
        PlayerPrefs.Save();
        Debug.Log("Final score saved: " + totalScore);

        Results results = FindObjectOfType<Results>();
        if (results != null)
        {
            results.EventEnds();
        }
    }

    public void PlayAgain()
    {
        totalScore = 0.0;
        songEnded = false;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReturnToMainMenu()
    {
        totalScore = 0.0;
        songEnded = false;
        SceneManager.LoadScene("MainMenu");
    }
}
