using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float moveSpeed = 1.0f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        Vector2 newPos = rb.position + (movementInput * Time.fixedDeltaTime * moveSpeed);
        rb.MovePosition(newPos);
    }
}
