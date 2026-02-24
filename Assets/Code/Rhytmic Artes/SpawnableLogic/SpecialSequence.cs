using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialSequence : MonoBehaviour
{
    private TMP_Text scoreText;
    private Conductor conductor;

    private float spawnBeat;   // beat when spawned
    private float targetBeat;  // beat when should be hit

    [Header("Settings")]
    public float approachBeats = 3f; // beats from spawn → target
    public float startScale = 0.5f;
    public float minScale = 0.1f;

    [Header("UI")]
    public Image circleImage;

    private bool clicked = false;
    private float despawnTimer = -1f;

    private float growSpeed = 2.0f;
    private float fadeSpeed = 2.0f;
    private CanvasGroup canvasGroup;
    public PlayerAnimator controller;
    private float approachSeconds;

    public float SpawnBeat => spawnBeat;
    public float TargetBeat => targetBeat;

public void Initialize(TMP_Text scoreTextRef, Conductor conductorRef, PlayerAnimator animator, int index = 0)
{
    scoreText = scoreTextRef;
    conductor = conductorRef;
    controller = animator; 

    spawnBeat = conductor.songPositionInBeats;
    targetBeat = spawnBeat + approachBeats;
    approachSeconds = approachBeats * conductor.secPerBeat;
}



    void Start()
    {
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

        if (!clicked && Input.anyKeyDown)
        {
            OnPressed();
        }

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
            if (circleImage != null)
            {
                circleImage.transform.localScale = Vector3.Lerp(
                    circleImage.transform.localScale,
                    new Vector3(0.6f, 0.6f, 1f),
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

    public void OnPressed()
    {
        //if (controller != null) controller.PlaySpecialAnimation();
        if (clicked) return;
        clicked = true;

        float currentBeat = conductor.songPositionInBeats;
        float diff = currentBeat - targetBeat;
        float absDiff = Mathf.Abs(diff);

        double score = 0.0;
        HitResult result = HitResult.Miss;

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
