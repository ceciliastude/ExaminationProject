using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int characterIndex; 
    private CharacterSelection characterSelection;

    void Start()
    {
        characterSelection = FindObjectOfType<CharacterSelection>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (characterSelection != null)
        {
            string hoveredName = characterSelection.characters[characterIndex].characterName;
            Debug.Log("Hovering over: " + hoveredName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Stopped hovering.");
    }
}
