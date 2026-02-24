using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0); 
        if (selectedIndex < 0 || selectedIndex >= characterPrefabs.Length)
        {
            Debug.LogError("Selected character index is out of bounds.");
            return;
        }

        GameObject prefab = characterPrefabs[selectedIndex];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
