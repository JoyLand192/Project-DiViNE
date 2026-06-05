using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CRInteractionManager : MonoBehaviour
{
    const KeyCode interactionKey = KeyCode.E;
    [SerializeField] CollisionDetector collisionDetector;
    [SerializeField] List<IInteraction> interactions = new();
    IInteraction currentActiveInteraction;
    public event System.Action<IInteraction> OnInteracted;
    void Awake()
    {
        collisionDetector.OnEnter += CollisionDetected;
        collisionDetector.OnExit += CollisionExit;
    }
    void Update()
    {
        if (interactions.Count > 0) UpdateInteractions();
        if (currentActiveInteraction != null && currentActiveInteraction.IsAvailable && Input.GetKeyDown(interactionKey))
        {
            OnInteracted?.Invoke(currentActiveInteraction);
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
