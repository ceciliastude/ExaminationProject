using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    public Camera mainCamera;
    public Camera resultCamera;

    public float zoomDuration = 1f;
    public float zoomDistance = 5f;

    public Transform playerTrans;
    public Transform enemyTrans;

    public GameObject finishedUI;
    public GameObject resultUI;
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject gameUI;
    public Button playAgainButton;
    public Button returnToMenuButton;
    private void Start()
    {
        if (mainCamera == null) // Check if the main camera has been assigned
        {
            Debug.LogError("Main Camera is not assigned in the CameraManager script.");
            return;
        }

        defaultPosition = mainCamera.transform.position;
        defaultRotation = mainCamera.transform.rotation;
        finishedUI.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);
        resultCamera.gameObject.SetActive(false); 
        playAgainButton.onClick.AddListener(PlayAgain);
        returnToMenuButton.onClick.AddListener(ReturnToMainMenu);

    }

    public void ZoomOnDefeatedFighter(bool isPlayerDefeated)
    {
        StartCoroutine(ZoomAndDisplay(isPlayerDefeated));
    }

    private IEnumerator ZoomAndDisplay(bool isPlayerDefeated)
    {
        Transform targetTransform = isPlayerDefeated ? playerTrans : enemyTrans;

        // Zoom in
        Vector3 targetPosition = targetTransform.position + new Vector3(0, 1, -zoomDistance); // Adjust this vector based on your camera angle
        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(defaultPosition, targetPosition, elapsedTime / zoomDuration);
            yield return null;
        }

        yield return new WaitForSeconds(2f); // Wait for 2 seconds before zooming out

        // Zoom back to default position
        elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(targetPosition, defaultPosition, elapsedTime / zoomDuration);
            yield return null;
        }

        // Show "Finished" UI
        finishedUI.SetActive(true);
        yield return new WaitForSeconds(3f); // Display the UI for 3 seconds
        finishedUI.SetActive(false); // Hide the UI again
        SwitchToResultCamera(isPlayerDefeated);
    }


    private void SwitchToResultCamera(bool isPlayerDefeated)
    {
        mainCamera.gameObject.SetActive(false);
        resultCamera.gameObject.SetActive(true);
        gameUI.SetActive(false);
        resultUI.SetActive(true);

        if (isPlayerDefeated)
        {
            loseUI.SetActive(true); 
        }
        else
        {
            winUI.SetActive(true);  
        }
    }

    private void PlayAgain()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); 
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}
