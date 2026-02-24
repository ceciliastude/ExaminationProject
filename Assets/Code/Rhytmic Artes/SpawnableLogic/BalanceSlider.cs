using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BalanceSlider : MonoBehaviour
{
    public Transform targetPosition;
    public RectTransform buttonToBalance;

    public GameObject mouseLeft;
    public GameObject mouseRight;
    private TMP_Text scoreText;
    public float radius = 180f;
    public double pointsPerSecond = 0.200;
    [SerializeField] private float driftSpeed = 30f;
    [SerializeField] private float pushStrength = 60f;

    private double currentScore = 0.000;
    public float balanceDuration = 7f;

    private RectTransform centerPoint;

    private bool isLeftClicking = false;
    private bool isRightClicking = false;
    private bool timeEnded = false;
    public Transform p0, p1, p2, p3;
    [SerializeField] private float t = 0.5f;

    public void Initialize(TMP_Text scoreTextRef)
    {
        scoreText = scoreTextRef;
    }

    void Start()
    {
        centerPoint = GetComponent<RectTransform>();
        Invoke(nameof(TimeEnds), balanceDuration);
        t = 0.5f + Random.Range(-0.05f, 0.05f);
    }

    void Update()
    {
        if (timeEnded)
            return;

        isLeftClicking = Input.GetMouseButton(0) && !Input.GetMouseButton(1);
        isRightClicking = Input.GetMouseButton(1) && !Input.GetMouseButton(0);

       
        bool leftHeld = Input.GetMouseButton(0);
        bool rightHeld = Input.GetMouseButton(1);

        mouseLeft.SetActive(leftHeld);
        mouseRight.SetActive(rightHeld);


        HandleBalance();
        HandleScoring();
    }
    private Vector3 CalculateBezierPoint(float t, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * a; // (1 - t)^3 * P0
        p += 3 * uu * t * b; // 3(1 - t)^2 * t * P1
        p += 3 * u * tt * c; // 3(1 - t) * t^2 * P2
        p += ttt * d;        // t^3 * P3

        return p;
    }

private void HandleBalance()
{
    if (isLeftClicking)
        t -= (pushStrength / 100f) * Time.deltaTime;
    else if (isRightClicking)
        t += (pushStrength / 100f) * Time.deltaTime;

    // Drift away from center (t = 0.5f is middle)
    if (t > 0.5f)
        t += (driftSpeed / 100f) * Time.deltaTime;
    else if (t < 0.5f)
        t -= (driftSpeed / 100f) * Time.deltaTime;

    t = Mathf.Clamp01(t); // keep inside 0–1

    Vector3 pos = CalculateBezierPoint(t, p0.position, p1.position, p2.position, p3.position);
    buttonToBalance.position = pos;
}



    private bool IsInsideTarget()
    {
        RectTransform targetRect = targetPosition as RectTransform;
        if (targetRect == null)
            return false;

        // Get the world corners of the target rect
        Vector3[] worldCorners = new Vector3[4];
        targetRect.GetWorldCorners(worldCorners);

        // Convert button’s position into world space
        Vector3 buttonWorldPos = buttonToBalance.position;

        // Check if button is inside the bounds
        if (buttonWorldPos.x >= worldCorners[0].x && buttonWorldPos.x <= worldCorners[2].x &&
            buttonWorldPos.y >= worldCorners[0].y && buttonWorldPos.y <= worldCorners[2].y)
        {
            return true;
        }

        return false;
    }


    private void HandleScoring()
    {
        if (IsInsideTarget())
        {
            currentScore += pointsPerSecond * Time.deltaTime;
        }
    }

    private void TimeEnds()
    {
        timeEnded = true;
        double finalScore = currentScore; 

        HitResult result;
        if (finalScore > 1.000)
            result = HitResult.Perfect;
        else if (finalScore >= 0.600)
            result = HitResult.Great;
        else if (finalScore >= 0.400)
            result = HitResult.Good;
        else
            result = HitResult.Miss;

        RegisterScore(finalScore, result);
        Destroy(gameObject, 0.6f);
    }

    void RegisterScore(double score, HitResult result)
    {
        RhythmGameManager.Instance.AddScore(score, result);

        if (scoreText != null)
            scoreText.text = $"{RhythmGameManager.Instance.GetTotalScore():F3} pt(s)";
    }
    
    /* //Keeping this if I want to change UI and need to edit the visualized curve again
    private void OnDrawGizmos()
    {
        if (p0 == null || p1 == null || p2 == null || p3 == null)
            return;

        Gizmos.color = Color.green;
        Vector3 prev = p0.position;

        for (int i = 1; i <= 20; i++) // 20 segments
        {
            float t = i / 20f;
            Vector3 point = CalculateBezierPoint(t, p0.position, p1.position, p2.position, p3.position);
            Gizmos.DrawLine(prev, point);
            prev = point;
        }
    }
*/
}
