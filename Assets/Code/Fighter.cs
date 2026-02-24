using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isSpecialAttacking = false;
    public bool isParrying = false;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isStunned = false;
    public bool isWalking = false;

    public HealthManager healthManager;
    public ChargeMeterManager chargeMeterManager;

    protected Animator animator;  
    protected SpriteRenderer spriteRenderer;

    public bool isDefeated = false;

    protected int currentAttackIndex = 0;
    public float attackDuration = 0.5f; 

    public static bool canMove = false;
    

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); 
        StartCoroutine(StartUp());
    }

    private IEnumerator StartUp(){
        canMove = false;
        yield return new WaitForSeconds(10f);
        canMove = true;
    }

    protected virtual void Update()
    {
        
        if (isDefeated) return;
        
        if (isSpecialAttacking || isParrying || isAttacking || isStunned) return;

        if (isBlocking)
        {
            animator.SetBool("isBlocking", true);
            
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
            if (stateInfo.IsName("block") || stateInfo.IsName("blockEnemy") && stateInfo.normalizedTime >= 0.99f && animator.speed != 0)
            {
                animator.speed = 0; 
            }
        }
        else
        {
            animator.SetBool("isBlocking", false);
            animator.speed = 1; 
        }

        if (isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (isAttacking)
        {
            ChangeSpriteDuringAttack();
        }

        if (isParrying && IsAttacking()){
            StartCoroutine(Stunned());
        }

    }

    public void TakeDamage(float damage, bool isPlayer)
    {
        if (healthManager != null)
        {
            healthManager.TakeDamage(damage, isPlayer);
            StartCoroutine(TakenDamage());
        }
    }

    protected IEnumerator PerformSpecialAttack(bool isPlayer)
    {
        isSpecialAttacking = true;
        chargeMeterManager.ActivateSpecialAttack(isPlayer); 
        canMove = false;
        //animator.Play("SpecialAttack"); // Play special attack animation
        yield return new WaitForSeconds(3f); 
        isSpecialAttacking = false;
        canMove = true;
    }

    protected IEnumerator TakenDamage(){
        animator.SetBool("hasTakenDamage", true);
        yield return StartCoroutine(ResetSpriteAfterDelay(attackDuration));
    }

    protected void ChangeSpriteDuringAttack()
    {
        animator.SetBool("isAttacking", true); 
        StartCoroutine(ResetSpriteAfterDelay(attackDuration));
    }

    protected IEnumerator ResetSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("isAttacking", false);
        animator.SetBool("hasTakenDamage", false);
    }

    protected bool IsAttacking()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    protected IEnumerator Parry()
    {
        isParrying = true;
        animator.SetBool("isParrying", true);
        yield return new WaitForSeconds(0.7f);
        animator.SetBool("isParrying", false);
        isParrying = false;
    }

    protected IEnumerator Stunned()
    {
        Debug.Log(this.name + " is stunned.");
        isStunned = true;
        animator.SetBool("isStunned", true); 
        yield return new WaitForSeconds(3f);
        animator.SetBool("isStunned", false);
        isStunned = false;
        Debug.Log(this.name + " is no longer stunned.");
    }

    public virtual void HandleDeath(bool isPlayer)
    {
        StartCoroutine(DeathSequence(isPlayer));
    }

    private IEnumerator DeathSequence(bool isPlayer)
    {
        isDefeated = true;
        DisableInputs();
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(2f); 
        
    }

    protected void DisableInputs()
    {
        isSpecialAttacking = false;
        isParrying = false;
        isAttacking = false;
        isBlocking = false;
        isStunned = true; 
        moveSpeed = 0f; 
    }

}
