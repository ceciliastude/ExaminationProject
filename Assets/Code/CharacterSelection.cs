using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CharacterData{
    public string characterName;
    public GameObject[] characterPrefabs;

}

public class CharacterSelection : MonoBehaviour
{

    public List<CharacterData> characters = new List<CharacterData>();
    private string currentSelection = "";
    public LoadingScreen loadingScreen;

    void Start(){
        loadingScreen = FindObjectOfType<LoadingScreen>();
    }
    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Count)
        {
            Debug.LogWarning("Character index out of range.");
            return;
        }
        string selectedName = characters[index].characterName;
         PlayerPrefs.SetInt("SelectedCharacterIndex", index);   
        PlayerPrefs.SetString("SelectedCharacter", selectedName); 

        for (int i = 0; i < characters.Count; i++)
        {
            PlayerPrefs.SetString($"CharacterName_{i}", characters[i].characterName);
        }

        PlayerPrefs.Save(); 

        currentSelection = selectedName;
        Debug.Log("Selected: " + currentSelection + " (index " + index + ")");
    }


    public string GetSelectedCharacter()
    {
        return PlayerPrefs.GetString("SelectedCharacter", "None");
    }

    public void OnBackButtonPressed()
    {
        PlayerPrefs.DeleteKey("SelectedCharacter");
        currentSelection = "";
        Debug.Log("Returning to previous screen and clearing selection. Testing: " + currentSelection);
    }
}
