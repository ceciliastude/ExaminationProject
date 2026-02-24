using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen; // Assign in Inspector
    public Image loadingBarFill;      // Assign in Inspector
    public float loadingFillDuration = 5f; // Total duration to fill the loading bar
    public string sceneName;

    public void LoadScene()
    {
        loadingScreen.SetActive(true); // Show the loading screen immediately
        StartCoroutine(LoadSceneCoroutine());
    }

    IEnumerator LoadSceneCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingFillDuration)
        {
            // Calculate the fill amount based on the elapsed time
            float fillAmount = elapsedTime / loadingFillDuration;

            // Juke effect: slow down at the middle and speed up towards the end
            if (fillAmount < 0.5f)
            {
                // Slow fill rate for the first half
                elapsedTime += Time.deltaTime * 0.5f; // Adjust this factor to slow down
            }
            else
            {
                // Speed up for the second half
                elapsedTime += Time.deltaTime * 2f; // Adjust this factor to speed up
            }

            // Update the loading bar fill
            loadingBarFill.fillAmount = Mathf.Clamp01(fillAmount); // Ensure fill amount is between 0 and 1
            yield return null; // Wait for the next frame
        }

        // Ensure the loading bar is completely filled
        loadingBarFill.fillAmount = 1f;

        // Load the next scene
        SceneManager.LoadScene(sceneName);
    }
}
