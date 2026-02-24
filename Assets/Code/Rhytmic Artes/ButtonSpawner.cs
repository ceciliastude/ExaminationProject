using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] buttonPrefabs;
    public GameObject circleSliderPrefab;
    public GameObject balanceSliderPrefab;
    public GameObject inputComboPrefab;
    public GameObject specialPrefab;

    [Header("UI")]
    public RectTransform spawnParent;  
    public TMP_Text scoreText;

    [Header("Player Animation")]
    public PlayerAnimator controller;
    private BeatMovementController beatMovementController;

    public void SpawnButton(int prefabIndex = 0, float approachBeats = 3f, Vector2 anchoredPos = default)
    {
        GameObject btnObj = Instantiate(buttonPrefabs[prefabIndex], spawnParent);

        // Set position
        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;

        RhythmButton rb = btnObj.GetComponent<RhythmButton>();
        rb.approachBeats = approachBeats;
        rb.Initialize(scoreText, FindObjectOfType<Conductor>(), controller);
    }



    public void SpawnCircleSlider(float targetTime, bool isFinalSpin)
    {
        GameObject sliderObj = Instantiate(circleSliderPrefab, spawnParent);

        CircleSlider cs = sliderObj.GetComponent<CircleSlider>();
        cs.Initialize(scoreText);

        if (controller == null) return;

        if (isFinalSpin)
        {
            // Final spin = longer + endGame
            controller.PlayCircleAnimation(6f, true);
        }
        else
        {
            // Normal spin
            controller.PlayCircleAnimation(5f, false);
        }
}


    public void SpawnBalanceSlider(float targetTime)
    {
        GameObject balanceObj = Instantiate(balanceSliderPrefab, spawnParent);

        BalanceSlider bs = balanceObj.GetComponent<BalanceSlider>();
        bs.Initialize(scoreText);
        if (controller != null) controller.PlayBalanceAnimation(5f);
    }

    public void SpawnInputCombo(float targetTime)
    {
        GameObject comboObj = Instantiate(inputComboPrefab, spawnParent);

        InputCombo ic = comboObj.GetComponent<InputCombo>();
        ic.Initialize(scoreText, controller);
        if (controller != null) controller.PlayComboAnimation(5f);
    }

    public void SpawnSpecial(float approachBeats = 3f, Vector2 anchoredPos = default)
    {
        GameObject btnObj = Instantiate(specialPrefab, spawnParent);

        // Set position
        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;

        SpecialSequence rb = btnObj.GetComponent<SpecialSequence>();
        rb.approachBeats = approachBeats;
        rb.Initialize(scoreText, FindObjectOfType<Conductor>(), controller);
    }
    
}
