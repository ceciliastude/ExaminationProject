using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuImageAnim : MonoBehaviour
{
    public Image imageComponent;
    public Sprite newSprite;
    public Animator animator;

    public void ChangeSprite(){
        imageComponent.sprite = newSprite;
        animator.Play("slideIn", -1, 0f);
    }
    
}
