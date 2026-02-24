using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Fighter
{
    private Player player;
    private Rigidbody rb;

    private bool isInAttackRange = false;       // For trigger zone
    private bool isPhysicallyColliding = false; // For movement stop

    [Range(0, 1)] public float attackProbability = 0.6f;
    [Range(0, 1)] public float blockProbability = 0.30f;
    [Range(0, 1)] public float parryProbability = 0.35f;
    [Range(0, 1)] public float specialAttackProbability = 0.10f;

    public float stopDistance = 1.5f;
    public float decisionCooldown = 1f;

    private enum AIMoveState { Approach, Retreat, Idle }
    private AIMoveState currentMoveState = AIMoveState.Idle;
    private float lastDecisionTime;
    private float moveStateTimer;
    private float moveStateDuration;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(RandomSpecialAttack());
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time - lastDecisionTime >= decisionCooldown)
        {
            MakeDecision();
            lastDecisionTime = Time.time;
        }

        if (player.isAttacking && isParrying && isInAttackRange)
        {
            player.TriggerStun();
        }

        if (isAttacking && player.isParrying && isInAttackRange)
        {
            TriggerStun();
        }
    }

    private void FixedUpdate()
    {
        if (player == null || isStunned || isParrying || isAttacking || isBlocking || isSpecialAttacking)
            return;

        HandleMovement();
    }

    private void MakeDecision()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (Time.time - moveStateTimer > moveStateDuration)
        {
            moveStateTimer = Time.time;
            moveStateDuration = Random.Range(1f, 3f);

            if (distanceToPlayer > stopDistance + 0.5f)
            {
                currentMoveState = AIMoveState.Approach;
            }
            else if (distanceToPlayer < stopDistance - 0.3f)
            {
                currentMoveState = Random.value < 0.6f ? AIMoveState.Retreat : AIMoveState.Idle;
            }
            else
            {
                currentMoveState = Random.value < 0.5f ? AIMoveState.Idle : AIMoveState.Approach;
            }
        }

        if (distanceToPlayer <= stopDistance)
        {
            if (player.isAttacking)
            {
                if (ShouldParry())
                    StartCoroutine(Parry());
                else if (ShouldBlock())
                    StartCoroutine(Block());
            }
            else if (CanUseSpecialAttack())
            {
                StartCoroutine(PerformSpecialAttack(false));
            }
            else if (ShouldAttack())
            {
                StartCoroutine(Attack());
            }
        }
    }

    private void HandleMovement()
    {
        if (!canMove || rb == null || isStunned || isParrying || isAttacking || isBlocking || isSpecialAttacking)
            return;

        if (isPhysicallyColliding)
            return;

        Vector3 directionToPlayer = (player.transform.position - rb.position).normalized;
        Vector3 targetDirection = Vector3.zero;

        switch (currentMoveState)
        {
            case AIMoveState.Approach:
                targetDirection = directionToPlayer;
                break;
            case AIMoveState.Retreat:
                targetDirection = -directionToPlayer;
                break;
            case AIMoveState.Idle:
                targetDirection = Vector3.zero;
                break;
        }

        Vector3 newPosition = rb.position + targetDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }


    private bool ShouldParry()
    {
        return Random.value < parryProbability;
    }

    private bool ShouldAttack()
    {
        return Random.value < attackProbability;
    }

    private bool ShouldBlock()
    {
        return Random.value < blockProbability;
    }

    private bool CanUseSpecialAttack()
    {
        return Random.value < specialAttackProbability &&
               chargeMeterManager != null &&
               chargeMeterManager.IsFullyCharged(false);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        ChangeSpriteDuringAttack();
        yield return new WaitForSeconds(attackDuration);

        if (isInAttackRange && !player.isBlocking)
        {
            player.TakeDamage(3, true);
            chargeMeterManager.AdjustPlayerChargeMeter(0.5f);
            chargeMeterManager.AdjustEnemyChargeMeter(1f);
        }

        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    private IEnumerator Block()
    {
        isBlocking = true;
        float blockDuration = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(blockDuration);
        isBlocking = false;
    }

    private IEnumerator RandomSpecialAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (CanUseSpecialAttack() && !isSpecialAttacking)
            {
                StartCoroutine(PerformSpecialAttack(false));
            }
        }
    }

    // ===== TRIGGER for attack range =====
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInAttackRange = true;
            player = other.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInAttackRange = false;
        }
    }

    // ===== COLLISION for movement blocking =====
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPhysicallyColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPhysicallyColliding = false;
        }
    }

    public void TriggerStun()
    {
        if (!isStunned)
        {
            isParrying = false;
            isAttacking = false;
            StartCoroutine(Stunned());
        }
    }
}
