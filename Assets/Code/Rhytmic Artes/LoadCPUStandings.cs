using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class LoadCPUStandings : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text standing;
    public TMP_Text opponentName;
    public TMP_Text playerName;
    public Image opponentIcon;             
    public Sprite[] characterIcons;  

    private int opponentIndex;
    private double opponentScore;

    void Start()
    {
        int playerIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        string playerSelectedName = PlayerPrefs.GetString("SelectedCharacter", "Player");

        if (characterIcons.Length < 2){
            Debug.LogError("Min 2 character icons needed");
            return;
        }

        do{
            opponentIndex = Random.Range(0, characterIcons.Length);
        }
        while (opponentIndex == playerIndex);

        opponentIcon.sprite = characterIcons[opponentIndex];

        opponentScore = (double)Random.Range(0f, 60f);
        string opponentSelectedName = PlayerPrefs.GetString($"CharacterName_{opponentIndex}", "Opponent");

        scoreText.text = $"{opponentScore:F3} pt(s)";
        opponentName.text = opponentSelectedName;
        playerName.text = playerSelectedName;

        PlayerPrefs.SetFloat("CPUFinalScore", (float)opponentScore);
        PlayerPrefs.SetInt("CPUIndex", opponentIndex);
        PlayerPrefs.SetString("CPUName", opponentSelectedName);
        PlayerPrefs.Save();
                
    }
}
