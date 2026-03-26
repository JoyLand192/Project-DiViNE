using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyMovement : EntityMovement
{
    public GameObject DebugTarget { get; set; }
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
            agent.isStopped = !value;
            isMovable = value;
        }
    }
    NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Update()
    {
        Move();
    }
    public void ChangeSpeed(float value) => agent.speed = value;
    void Move()
    {
        if (agent == null || DebugTarget == null || !IsMovable) return;

        agent.SetDestination(DebugTarget.transform.position);
    }
}
