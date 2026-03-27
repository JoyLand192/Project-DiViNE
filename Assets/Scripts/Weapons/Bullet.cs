using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public float MoveSpeed { get; set; }
    public bool IsMoving { get; set; }
    public Bullet Key { get; set; }
    public CRShooter Shooter { get; set; }
    public float Damage { get; set; }
    public bool Hitable { get; set; }
    protected CancellationTokenSource cts = new();
    public void Reset()
    {
        rb.isKinematic = false;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Hitable) return;

        Hitable = false;
        Shooter.OnBulletHit(this, CurrentDirection, other, Damage);

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        // 파티클 또는 타격 연출

        cts.Cancel();
        cts.Dispose();
        cts = new();
    }
    public async void Launch(Vector2? startPos = null, Vector2? directionVector = null, float? speed = null, float? lifeTime = null)
    {
        if (startPos != null) transform.position = startPos.Value;
        if (directionVector != null) CurrentDirection = directionVector.Value;
        if (speed != null) MoveSpeed = speed.Value;

        IsMoving = true;
        rb.velocity = CurrentDirection * MoveSpeed;

        if (lifeTime == null) return;

        await UniTask.Delay(delayTimeSpan: System.TimeSpan.FromSeconds(lifeTime.Value), cancellationToken: cts.Token).SuppressCancellationThrow();

        Shooter.OnBulletBreak(this);
    }
}
