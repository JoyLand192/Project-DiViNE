using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer graphic;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Vector2 currentDirection = new(1, 0);
    public Vector2 CurrentDirection
    {
        get => currentDirection;
        set
        {
            currentDirection = value;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg);
        }
    }
    [SerializeField] protected float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    [SerializeField] protected bool isMoving;
    public bool IsMoving
    {
        get => isMoving;
        set => isMoving = value;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public async void Launch(Vector2? startPos = null, Vector2? directionVector = null, float? speed = null, float lifeTime = 0.5f)
    {
        if (startPos != null) transform.position = startPos.Value;
        if (directionVector != null) CurrentDirection = directionVector.Value;
        if (speed != null) MoveSpeed = speed.Value;

        IsMoving = true;
        rb.velocity = CurrentDirection * MoveSpeed;

        await UniTask.Delay(System.TimeSpan.FromSeconds(lifeTime));

        gameObject.SetActive(false);
    }
}
