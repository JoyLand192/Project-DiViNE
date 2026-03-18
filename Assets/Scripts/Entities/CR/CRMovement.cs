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
    public void Move(float moveSpeed)
    {
        if (!IsMovable) return;

        var moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.MovePosition(transform.position + (moveVector * moveSpeed * Time.fixedDeltaTime));
    }
}
