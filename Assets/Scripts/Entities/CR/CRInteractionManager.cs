using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CRInteractionManager : MonoBehaviour
{
    const float interactionCooldown = 0.25f;
    const KeyCode interactionKey = KeyCode.E;
    [SerializeField] CollisionDetector collisionDetector;
    [SerializeField] List<IInteraction> interactions = new();
    [SerializeField] Transform navigator;
    float timer;
    IInteraction currentActiveInteraction;
    public event System.Action<IInteraction> OnInteracted;
    void Awake()
    {
        collisionDetector.OnEnter += CollisionDetected;
        collisionDetector.OnExit += CollisionExit;
    }
    void Update()
    {
        if (timer > 0) timer -= Time.unscaledDeltaTime;
        if (navigator != null) navigator.gameObject.SetActive(currentActiveInteraction != null);

        if (interactions.Count > 0) UpdateInteractions();
        if (currentActiveInteraction != null && currentActiveInteraction.IsAvailable && timer <= 0)
        {
            if (navigator != null)
            {
                var direction = (currentActiveInteraction.CurrentPosition - navigator.position).normalized;
                var degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var fixedDegree = navigator.transform.rotation.eulerAngles;
                fixedDegree.z = degree;

                navigator.transform.rotation = Quaternion.Euler(fixedDegree);
            }
            if (Input.GetKeyDown(interactionKey))
            {
                OnInteracted?.Invoke(currentActiveInteraction);
                timer += interactionCooldown;
            }
        }
    }
    void OnDestroy()
    {
        collisionDetector.OnEnter -= CollisionDetected;
        collisionDetector.OnExit -= CollisionExit;
    }
    void CollisionDetected(Collider2D collider)
    {
        if (!collider.TryGetComponent<IInteraction>(out var interaction)) return;

        interactions.Add(interaction);
    }
    void CollisionExit(Collider2D collider)
    {
        if (!collider.TryGetComponent<IInteraction>(out var interaction)) return;
        if (!interactions.Contains(interaction)) return;

        interactions.Remove(interaction);
        if (interactions.Count == 0)
        {
            currentActiveInteraction.IsDisplayingUI = false;
            currentActiveInteraction = null;
        }
    }
    public void UpdateInteractions()
    {
        interactions = interactions
            .OrderByDescending(i => i.IsAvailable)
            .ThenBy(i => Vector2.Distance(transform.position, i.CurrentPosition))
            .ToList();

        var first = interactions.FirstOrDefault();
        if (first == currentActiveInteraction) return;

        if (currentActiveInteraction != null) currentActiveInteraction.IsDisplayingUI = false;
        if (first != null) first.IsDisplayingUI = true;
        currentActiveInteraction = first;
    }
} 
