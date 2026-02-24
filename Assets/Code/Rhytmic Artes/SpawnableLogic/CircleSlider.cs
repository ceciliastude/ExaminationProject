using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CircleSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform buttonToDrag;
    public Image fillerPrefab;
    public Transform fillerParent;
    public TMP_Text spinCountText;
    public Image filledBK;

    private TMP_Text scoreText; 
    public float radius = 180f;
    public double pointsPerSpin = 0.200;
    public float circleDuration = 7f;

    private Image currentFiller;
    private int spinCount = 0;
    private float progress = 0f;

    private RectTransform centerPoint;
    private Vector2 lastMouseDir;

    private bool isDragging = false;
    private bool timeEnded = false;
    private int rainbowIndex = 0;

    public void Initialize(TMP_Text scoreTextRef)
    {
        scoreText = scoreTextRef;
    }

    void Start()
    {
        centerPoint = GetComponent<RectTransform>();
        SpawnNewFiller();
        UpdateSpinCount();
        lastMouseDir = Vector2.up;

        // Start duration timer
        Invoke(nameof(TimeEnds), circleDuration);
    }

void Update()
{
    if (!isDragging || timeEnded) return;

    Vector2 mouseDir = (Vector2)Input.mousePosition - (Vector2)centerPoint.position;
    mouseDir.Normalize();

    float angleDelta = Vector2.SignedAngle(lastMouseDir, mouseDir);

    if (angleDelta < 0)
    {
        progress += Mathf.Abs(angleDelta) / 360f;

        if (progress >= 1f)
        {
            // Before spawning the next filler, apply the old filler’s color to filledBK
            if (currentFiller != null)
            {
                filledBK.color = currentFiller.color;
            }

            spinCount++;
            UpdateSpinCount();
            filledBK.color = currentFiller.color;
            SpawnNewFiller();
            progress = 0f;
        }
    }

    currentFiller.fillAmount = progress;

    float angle = progress * 360f;
    Vector2 pos = new Vector2(
        Mathf.Cos((angle - 90f) * Mathf.Deg2Rad),
        Mathf.Sin((angle + 90f) * Mathf.Deg2Rad)
    ) * radius;

    buttonToDrag.localPosition = pos;
    lastMouseDir = mouseDir;
}

    private static readonly Color[] rainbowColors = new Color[]
    {
    Color.red,
    new Color(1f, 0.5f, 0f),
    Color.yellow,
    Color.green,
    Color.blue,
    new Color(0.29f, 0f, 0.51f),
    Color.magenta,                 
};


void SpawnNewFiller()
{
    if (currentFiller != null)
        Destroy(currentFiller.gameObject);

    currentFiller = Instantiate(fillerPrefab, fillerParent);
    currentFiller.fillAmount = 0f;
    currentFiller.color = rainbowColors[rainbowIndex];
    rainbowIndex = (rainbowIndex + 1) % rainbowColors.Length;
}


    void UpdateSpinCount()
    {
        spinCountText.text = spinCount.ToString();
    }

    void TimeEnds()
    {
        timeEnded = true;

        double finalScore = spinCount * pointsPerSpin + progress * pointsPerSpin;

        HitResult result;
        if (spinCount >= 25)
            result = HitResult.Perfect;
        else if (spinCount >= 15)
            result = HitResult.Great;
        else if (spinCount >= 5)
            result = HitResult.Good;
        else
            result = HitResult.Miss;

        RegisterScore(finalScore, result);

        Destroy(gameObject, 0.6f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastMouseDir = (Vector2)Input.mousePosition - (Vector2)centerPoint.position;
        lastMouseDir.Normalize();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    void RegisterScore(double score, HitResult result)
    {
        RhythmGameManager.Instance.AddScore(score, result);
        
        if (scoreText != null)
            scoreText.text = $"{RhythmGameManager.Instance.GetTotalScore():F3} pt(s)";
    }
}
