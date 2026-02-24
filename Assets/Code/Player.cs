using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : Fighter
{
    private Vector3 input;
    private Opponent enemy; 
    private bool isCollidingWithEnemy = false; 
    private Rigidbody rb;

protected override void Start()
{
    base.Start();
    rb = GetComponent<Rigidbody>();
    enemy = FindObjectOfType<Opponent>();
}

        private void FixedUpdate()
        {
            if (rb == null) return;

            if (Mathf.Abs(input.x) > 0.1f)
            {
                isWalking = true;
                Vector3 newPosition = rb.position + new Vector3(input.x * moveSpeed * Time.fixedDeltaTime, 0f, 0f);
                rb.MovePosition(newPosition);

                // Optional: flip sprite to face direction
                if (input.x > 0)
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                else if (input.x < 0)
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                isWalking = false;
            }
        }

    protected override void Update()
    {
        base.Update(); // Calls Fighter's shared Update logic

        if (isSpecialAttacking || isParrying || isAttacking || isStunned) return;

        // Handle block input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isBlocking = true;
            
        }
        else
        {
            isBlocking = false;
        }

        //Parry logic
        if (Input.GetKey(KeyCode.F)){

            if (enemy != null){
                StartCoroutine(Parry());
            
                if (isAttacking && enemy.isParrying && isCollidingWithEnemy)
                {
                    TriggerStun(); 
                }
                if (enemy.isAttacking && isParrying && isCollidingWithEnemy)
                {
                    enemy.TriggerStun(); 
                }
            }

        }

        if (!isBlocking)
        {
            input.x = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            input.x = 0;
            isWalking = false;
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
            ChangeSpriteDuringAttack();
            if (isCollidingWithEnemy && enemy != null && !enemy.isBlocking) 
            {
                enemy.TakeDamage(3, false);
                chargeMeterManager.AdjustPlayerChargeMeter(1f);
                chargeMeterManager.AdjustEnemyChargeMeter(0.5f);
            }
            StartCoroutine(ResetAttackState());
        }
        if (Input.GetKeyDown(KeyCode.E) && enemy.isStunned)
        {
            isAttacking = true;
            ChangeSpriteDuringAttack();
            if (isCollidingWithEnemy && enemy != null) 
            {
                enemy.TakeDamage(5, false); // Stronger attack
                chargeMeterManager.AdjustPlayerChargeMeter(2f);
                chargeMeterManager.AdjustEnemyChargeMeter(1f);
            }
            StartCoroutine(ResetAttackState());
        }

        // Special attack input
        if (Input.GetKeyDown(KeyCode.Q) && chargeMeterManager.IsFullyCharged(true))
        {
            StartCoroutine(PerformSpecialAttack(true));
        }
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyDummy"))
        {
            isCollidingWithEnemy = true; 
            enemy = other.GetComponent<Opponent>(); 

            if (enemy == null)
            {
                Debug.LogError("Enemy Dummy does not have an Opponent component!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyDummy"))
        {
            isCollidingWithEnemy = false; 
            enemy = null; 
        }
    }

    public void TriggerStun() {
        if (!isStunned) {
            isParrying = false;
            isAttacking = false;
            StartCoroutine(Stunned());
        }
    }


}
