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
    protected Bullet key;
    public Bullet Key
    {
        get => key;
        set => key = value;
    }
    public event System.Action OnBulletDeath;
    public event System.Action<Entity> OnBulletHit;
    protected CancellationTokenSource cts = new();
    public void Reset()
    {
        rb.isKinematic = false;
        OnBulletHit = null;
        OnBulletDeath = null;
    }
    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Entity>(out var target))
        {
            var handler = OnBulletHit;
            OnBulletHit = null;
            handler?.Invoke(target);
        }

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        // 파티클 또는 타격 연출

        cts.Cancel();
        cts.Dispose();
        cts = new();
        OnBulletDeath?.Invoke();
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

        OnBulletDeath?.Invoke();
    }
}
