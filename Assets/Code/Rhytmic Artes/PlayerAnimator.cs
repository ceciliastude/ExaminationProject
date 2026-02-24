using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private int numberOfTrickAnimations = 3;
    private int numberOfComboAnimations = 3;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayBalanceAnimation(float loopDuration)
    {
        StartCoroutine(BalanceRoutine(loopDuration));
    }

    private IEnumerator BalanceRoutine(float loopDuration)
    {
        animator.SetBool("isBalancing", true);

        yield return new WaitForSeconds(loopDuration);

        animator.SetTrigger("endBalance");

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float waitTime = clipInfo[0].clip.length;
        yield return new WaitForSeconds(waitTime);


        animator.SetBool("isBalancing", false);
    }

    public void PlayTrickAnimation()
    {
        StartCoroutine(TrickRoutine());
    }

   private IEnumerator TrickRoutine()
{ 
    // Trick sequence
    int randomIndex = Random.Range(0, numberOfTrickAnimations); 
    animator.SetInteger("randomTrickIndex", randomIndex);
    animator.SetBool("isTricking", true);


    // Wait until the current animation finishes
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    yield return new WaitForSeconds(stateInfo.length);
    Debug.Log("StateInfo: " + stateInfo.length);
    animator.SetBool("isTricking", false);
}


    public void PlayComboAnimation(float loopDuration)
    {
        StartCoroutine(ComboRoutine(loopDuration));
    }

    private IEnumerator ComboRoutine(float loopDuration)
    {
        animator.SetBool("isComboing", true);

        yield return new WaitForSeconds(loopDuration);

        animator.SetTrigger("ComboEnd");

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float waitTime = stateInfo.length;
        yield return new WaitForSeconds(waitTime);

        animator.SetBool("isComboing", false);
    }

public void PlayComboCastAnimation()
{
    StartCoroutine(ComboCastRoutine());
}

private IEnumerator ComboCastRoutine()
{
    int randomIndex = Random.Range(0, numberOfComboAnimations);
    animator.SetInteger("randomCombo", randomIndex);
    animator.SetBool("comboCompleted", true);

    yield return null; 

    animator.SetBool("comboCompleted", false);
}

public void PlaySpecialAnimation(float loopDuration)
    {
        StartCoroutine(SpecialRoutine(loopDuration));
    }

private IEnumerator SpecialRoutine(float loopDuration)
    {
        animator.SetBool("isSpecial", true);

        yield return new WaitForSeconds(loopDuration);

        animator.SetTrigger("endSpecial");
        animator.SetBool("isSpecial", false);
    }

public void PlayCircleAnimation(float loopDuration, bool isFinalSpin)
{
    StartCoroutine(CircleRoutine(loopDuration, isFinalSpin));
}


private IEnumerator CircleRoutine(float loopDuration, bool isFinalSpin)
{
    int layerIndex = animator.GetLayerIndex("MaskLayer");
    float fadeTime = 0.3f;
    float elapsed = 0f;

    while (elapsed < fadeTime)
    {
        elapsed += Time.deltaTime;
        animator.SetLayerWeight(layerIndex, Mathf.Lerp(0f, 1f, elapsed / fadeTime));
        yield return null;
    }

    animator.SetLayerWeight(layerIndex, 1f);
    animator.SetBool("isSpinning", true);

    yield return new WaitForSeconds(loopDuration);

    elapsed = 0f;
    while (elapsed < fadeTime)
    {
        elapsed += Time.deltaTime;
        animator.SetLayerWeight(layerIndex, Mathf.Lerp(1f, 0f, elapsed / fadeTime));
        yield return null;
    }

    animator.SetLayerWeight(layerIndex, 0f);

    if (isFinalSpin)
    {
        animator.SetBool("isFinalSpin", true);
        animator.SetTrigger("endSpin");
        animator.SetTrigger("endGame");
        animator.SetBool("isSpinning", false);
        yield break;
    }
        else
        {
            animator.SetTrigger("endSpin");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);

            animator.SetBool("isSpinning", false);
        }




}

}
