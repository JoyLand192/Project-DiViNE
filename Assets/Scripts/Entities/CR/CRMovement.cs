using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRMovement : EntityMovement
{ 
    protected Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        if (!IsMovable) return;

        var moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.MovePosition(transform.position + (moveVector * MoveSpeed * Time.fixedDeltaTime));
    }
}
