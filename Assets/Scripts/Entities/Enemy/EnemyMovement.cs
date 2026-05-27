using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyMovement : EntityMovement
{
    [SerializeField] float range;
    public float Range
    {
        get => range;
        set
        {
            agent.stoppingDistance = value;
            range = value;
        }
    }
    public override bool IsMovable
    {
        get => isMovable;
        set
        {
            isMovable = value;
            if (agent != null) agent.isStopped = !isChasing || !isMovable;
        }
    }
    protected bool isChasing;
    public bool IsChasing
    {
        get => isChasing;
        set
        {
            isChasing = value;
            if (agent != null) agent.isStopped = !isChasing || !isMovable;
        }
    }
    [SerializeField] NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = !isChasing || !isMovable;
    }
    public void ChangeSpeed(float value)
    {
        agent.speed = value;
        agent.acceleration = value * 2;
    }
    public void Move(CR cr) => Move(cr == null ? null : cr.transform);
    public void Move(Transform target)
    {
        if (agent == null || target == null || !IsMovable) return;
        agent.SetDestination(target.transform.position);
    }
}
