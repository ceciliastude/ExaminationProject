using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;         
    public Image nextButtonImage;     
    public GameObject beginningUI;

    private Camera panCamera;         
    private Vector3 initialPosition;   
    private Quaternion initialRotation; 
    public float moveSpeed = 2f;
    public Image bar1, bar2, bar3, bar4;
    public TextMeshProUGUI versusImage;
    public float fillDuration = 0.1f; 

    private void Start()
    {
        // Find the pan camera by its tag "SecondaryCamera"
        panCamera = GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<Camera>();

        // Store the initial position and rotation of the Pan Camera
        initialPosition = panCamera.transform.position;
        initialRotation = panCamera.transform.rotation;

        // Set the Main Camera inactive at the start
        mainCamera.gameObject.SetActive(false);

        // Set up the listener for the next button
        nextButtonImage.GetComponent<Button>().onClick.AddListener(OnNextButtonPressed);
        StartCoroutine(ShowUIAfterDelay(3f));
    }

    private void Update()
    {
        MovePanCamera();
    }

    private void MovePanCamera()
    {
        // Move the pan camera to the left at a constant speed
        if (panCamera != null)
        {
            panCamera.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
    }

    public void OnNextButtonPressed()
    {
        // Switch to the main camera
        if (panCamera != null)
        {
            panCamera.gameObject.SetActive(false);
        }
        mainCamera.gameObject.SetActive(true);
    }

    private IEnumerator ShowUIAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Show the next button UI
        beginningUI.gameObject.SetActive(true);
        StartCoroutine(FillBarsSequentially());
    }

    private IEnumerator FillBarsSequentially()
    {

        yield return FillBar(bar1, bar2);
        yield return new WaitForSeconds(0.5f);
        yield return FillBar(bar3, bar4);
        versusImage.gameObject.SetActive(true);
    }
    private IEnumerator FillBar(Image barFirst, Image barSecond)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float fillValue = Mathf.Lerp(0, 1, elapsedTime / fillDuration);
            barFirst.fillAmount = fillValue;
            barSecond.fillAmount = fillValue;
            yield return null;
        }
            barFirst.fillAmount = 1f;
            barSecond.fillAmount = 1f; 
    }
}
