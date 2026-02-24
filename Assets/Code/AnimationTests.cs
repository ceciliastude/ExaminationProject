using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AnimationTests : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        bool walking = Mathf.Abs(input.x) > 0.1f;
        animator.SetBool("isWalking", walking);
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        if (Mathf.Abs(input.x) > 0.1f)
        {
            Vector3 newPosition = rb.position + input.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }
}
