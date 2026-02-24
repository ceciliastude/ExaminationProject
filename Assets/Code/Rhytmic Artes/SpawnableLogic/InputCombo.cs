using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InputCombo : MonoBehaviour
{
    [Header("UI")]
    public GameObject[] inputPrefabs;

    [Header("Functionality")]
    public int currentInputIndex = 0;
    public float comboDuration = 7f;
    public float delayBetweenCombos = 0.2f;
    public RectTransform[] spawnSlots;

    [Header("Player Animation")]
    public PlayerAnimator controller;

    private List<GameObject> activeInputs = new List<GameObject>();
    private bool timeEnded = false;
    public int comboCounter = 0;
    private double currentComboScore = 0.000;
    private double accuracyPenalty = 1.0;
    private TMP_Text scoreText;
    private int lastInputFrame = -1;

    public void Initialize(TMP_Text scoreTextRef,PlayerAnimator animator)
    {
        controller = animator;
        scoreText = scoreTextRef;
    }

    void Start()
    {
        StartCoroutine(ComboRoutine());
        Invoke(nameof(TimeEnds), comboDuration);
    }

    private IEnumerator ComboRoutine()
    {
        while (!timeEnded)
        {
            yield return StartCoroutine(PlayCombo());
            yield return new WaitForSeconds(delayBetweenCombos);
        }
    }

    private IEnumerator PlayCombo()
    {
        currentComboScore = 0.0;
        accuracyPenalty = 1.0;
        currentInputIndex = 0;

          int maxIndex = 2;
            if (comboCounter == 1) maxIndex = Mathf.Min(3, inputPrefabs.Length);
            else if (comboCounter >= 2) maxIndex = inputPrefabs.Length;

        // Randomize the order of prefabs and spawn in slots
        for (int i = 0; i < 4; i++)
        {
            GameObject prefab = inputPrefabs[Random.Range(0, maxIndex)];

            GameObject inputObj = Instantiate(prefab, spawnSlots[i]);
            inputObj.transform.localPosition = Vector3.zero;
            inputObj.transform.localScale = Vector3.one;

            ComboInputHandler handler = inputObj.AddComponent<ComboInputHandler>();
            handler.Initialize(this, i);
            activeInputs.Add(inputObj);
        }

        // Wait until all inputs are completed
        while (currentInputIndex < 4 && !timeEnded)
            yield return null;
        comboCounter++;
         if (controller != null) controller.PlayComboCastAnimation();
        
    }

    // Called by InputHandler to ensure single consumption per frame
    public bool TryConsumeFrameInput()
    {
        if (lastInputFrame == Time.frameCount) return false;
        lastInputFrame = Time.frameCount;
        return true;
    }

    // Now takes a parameter
    public void OnPress(bool correct)
    {
        if (timeEnded) return;

        if (correct)
        {
            // Apply penalty multiplier to base score
            double pressScore = 0.100 * accuracyPenalty;
            currentComboScore += pressScore;
            currentInputIndex++;
            accuracyPenalty = 1.0;
        }
        else
        {
            // Multiply penalty (10% off each mistake)
            accuracyPenalty *= 0.9;
            Debug.Log($"Penalty applied → new accuracyPenalty: {accuracyPenalty:F3}");
        }
        
        if (currentInputIndex >= 4)
        {
            ClearAllInputs();
        }
    }

    private void ClearAllInputs()
{
        foreach (var inputObj in activeInputs)
        {
            if (inputObj != null)
                Destroy(inputObj);
        }
    activeInputs.Clear();
}



    void TimeEnds()
    {
        timeEnded = true;

        HitResult result;
        if (comboCounter >= 3)
            result = HitResult.Perfect;
        else if (comboCounter >= 2)
            result = HitResult.Great;
        else if (comboCounter >= 1)
            result = HitResult.Good;
        else
            result = HitResult.Miss;

        RegisterScore(currentComboScore, result);

        Destroy(gameObject, 0.6f);
    }

    void RegisterScore(double score, HitResult result)
    {
        RhythmGameManager.Instance.AddScore(score, result);

        if (scoreText != null)
            scoreText.text = $"{RhythmGameManager.Instance.GetTotalScore():F3} pt(s)";
    }
}
