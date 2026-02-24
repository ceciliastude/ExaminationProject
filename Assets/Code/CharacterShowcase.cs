using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class CharacterShowcase : MonoBehaviour
{
    public Image imageComponent;
    public Sprite newSprite;
    public Animator animator;
    public TextMeshProUGUI title;
    public string newText; 

    public void ChangeSprite()
    {
        imageComponent.sprite = newSprite;
        imageComponent.SetNativeSize();
        animator.Play("characterShowcase", -1, 0f);
    }

    public void ChangeText()
    {
        title.text = newText;
    }
}
