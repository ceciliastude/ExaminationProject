using UnityEngine;
using UnityEngine.UI; // Needed for Image

public class LoadIconUI : MonoBehaviour
{
    public Image playerIcon;             
    public Sprite[] characterIcons;      

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        // Safety check
        if (selectedIndex < 0 || selectedIndex >= characterIcons.Length)
        {
            Debug.LogError("Selected character index is out of bounds.");
            return;
        }

        // Set the player icon based on the selected character
        playerIcon.sprite = characterIcons[selectedIndex];
    }
}
