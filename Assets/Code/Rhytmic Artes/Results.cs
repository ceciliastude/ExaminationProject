using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    [System.Serializable]
    public class PlacementUI{
        public TMP_Text nameText;
        public TMP_Text scoreText;
        public Image icon;
    }

    public TMP_Text individualScore;
    public PlacementUI firstPlaceUI;
    public PlacementUI secondPlaceUI;
    public GameObject gameUI;
    public GameObject canvasGroup;
    private OutroManager endUI;

    void Start()
    {
        endUI = FindObjectOfType<OutroManager>();
    }

    public void EventEnds()
    {
        gameUI.SetActive(false);
        canvasGroup.SetActive(true);
        endUI.FadeIn(2);
        float playerScore = PlayerPrefs.GetFloat("PlayerFinalScore", 0f);
        string playerSelectedName = PlayerPrefs.GetString("SelectedCharacter", "Player");
        int playerIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        float cpuScore = PlayerPrefs.GetFloat("CPUFinalScore", 0f);
        int cpuIndex = PlayerPrefs.GetInt("CPUIndex", -1);
        string cpuName = PlayerPrefs.GetString($"CharacterName_{cpuIndex}", "Opponent");

        Sprite[] icons = FindObjectOfType<LoadCPUStandings>().characterIcons;

        var results = new List<(string name, float score, int index)>
        {
            (playerSelectedName, playerScore, playerIndex),
            (cpuName, cpuScore, cpuIndex)
        };

        individualScore.text = $"{playerScore:F3} pt(s)";

        results.Sort((a, b) => b.score.CompareTo(a.score));

        firstPlaceUI.nameText.text = results[0].name;
        firstPlaceUI.scoreText.text = $"{results[0].score:F3} pt(s)";
        if (results[0].index >= 0 && results[0].index < icons.Length)
            firstPlaceUI.icon.sprite = icons[results[0].index];

        secondPlaceUI.nameText.text = results[1].name;
        secondPlaceUI.scoreText.text = $"{results[1].score:F3} pt(s)";
        if (results[1].index >= 0 && results[1].index < icons.Length)
            secondPlaceUI.icon.sprite = icons[results[1].index];
    }
}
