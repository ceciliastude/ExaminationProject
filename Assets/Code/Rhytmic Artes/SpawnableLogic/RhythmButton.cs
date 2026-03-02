using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RhythmButton : MonoBehaviour
{
    private TMP_Text scoreText;
    private Conductor conductor;

    private float spawnBeat;   // beat when this button was spawned
    private float targetBeat;  // beat when button should be hit

    [Header("Settings")]
    public float approachBeats = 3f; // beats from spawn → target
    public float startScale = 0.5f;
    public float minScale = 0.1f;
    public KeyCode requiredKey;

    [Header("UI")]
    public Image defaultImage;
    public Image pressedImage;
    public Image circleImage;
    public Image starBK;
    public GameObject starAnimation;

    private bool clicked = false;
    private float despawnTimer = -1f;

    private float growSpeed = 2.0f;
    private float fadeSpeed = 2.0f;
    private CanvasGroup canvasGroup;
    public PlayerAnimator controller;
    private float approachSeconds;

    public float SpawnBeat => spawnBeat;
    public float TargetBeat => targetBeat;
    public KeyCode RequiredKey => requiredKey;

public void Initialize(TMP_Text scoreTextRef, Conductor conductorRef, PlayerAnimator animator, int index = 0)
{
    scoreText = scoreTextRef;
    conductor = conductorRef;
    controller = animator; 

    spawnBeat = conductor.songPositionInBeats;
    targetBeat = spawnBeat + approachBeats;
    approachSeconds = approachBeats * conductor.secPerBeat;

    ButtonInputHandler handler = GetComponent<ButtonInputHandler>();
    if (handler == null)
        handler = gameObject.AddComponent<ButtonInputHandler>();

    handler.Initialize(this, index);
}



    void Start()
    {
        if (defaultImage != null) defaultImage.gameObject.SetActive(true);
        if (pressedImage != null) pressedImage.gameObject.SetActive(false);
        if (circleImage != null) 
            circleImage.transform.localScale = new Vector3(startScale, startScale, 1f);

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        if (conductor == null) return;

        float currentBeat = conductor.songPositionInBeats;

        // Shrink approach circle
        if (!clicked && circleImage != null)
        {
            if (approachSeconds > 0f)
            {
                float scaleDelta = (startScale - minScale) / approachSeconds;
                float newScale = Mathf.Max(minScale, circleImage.transform.localScale.x - scaleDelta * Time.deltaTime);
                circleImage.transform.localScale = new Vector3(newScale, newScale, 1f);
            }
            else
            {
                circleImage.transform.localScale = new Vector3(minScale, minScale, 1f);
            }
        }

        // Hit animation after pressing
        if (clicked)
        {
            if (starBK != null)
            {
                    starBK.transform.localScale = Vector3.Lerp(
                    starBK.transform.localScale,
                    new Vector3(4.6f, 4.6f, 1f),
                    growSpeed * Time.deltaTime
                    );
                    starBK.transform.Rotate (new Vector3 (0, 0, 0.3f));
            }
            if (starAnimation != null)
            {
                    starAnimation.transform.localScale = Vector3.Lerp(
                    starAnimation.transform.localScale,
                    new Vector3(2.6f, 2.6f, 1f),
                    growSpeed * Time.deltaTime
                    );
            }

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
        }

        // Auto miss if time passes
        if (!clicked && currentBeat > targetBeat + 0.5f)
        {
            RegisterScore(0, HitResult.Miss);
            Destroy(gameObject);
        }

        if (despawnTimer > 0f && Time.time >= despawnTimer)
        {
            Destroy(gameObject);
        }
    }

    public void OnPressed(bool correct)
    {
        if (controller != null) controller.PlayTrickAnimation();
        if (clicked) return;
        clicked = true;

        if (defaultImage != null) defaultImage.gameObject.SetActive(false);
        if (pressedImage != null) pressedImage.gameObject.SetActive(true);

        float currentBeat = conductor.songPositionInBeats;
        float diff = currentBeat - targetBeat;
        float absDiff = Mathf.Abs(diff);

        double score = 0.0;
        HitResult result = HitResult.Miss;

        if (correct)
        {
            if (absDiff < 0.1f)
            {
                score = 1.100;
                result = HitResult.Perfect;
            }
            else if (absDiff < 0.25f)
            {
                score = 1.000;
                result = (diff < 0) ? HitResult.GreatEarly : HitResult.GreatLate;
            }
            else if (absDiff < 0.5f)
            {
                score = 0.500;
                result = (diff < 0) ? HitResult.GoodEarly : HitResult.GoodLate;
            }
        }

        RegisterScore(score, result);
        despawnTimer = Time.time + 0.6f;
    }

    // Registers the hit result and updates the UI
    void RegisterScore(double score, HitResult result)
    {
        RhythmGameManager.Instance.AddScore(score, result);
        if (scoreText != null)
            scoreText.text = $"{RhythmGameManager.Instance.GetTotalScore():F3} pt(s)";
    }
}
