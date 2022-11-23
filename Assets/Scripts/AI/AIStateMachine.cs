using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine : BaseStateMachine
{
    private GAgent _gAgent;
    private NavMeshAgent _agent;

    public override void Start()
    {
        base.Start();

        _gAgent = GetComponent<GAgent>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public override void Move()
    {
        _moveDirection.x = _gAgent.IsMoving ? 1f : 0f;
    }

    public void BasicAttack()
    {
        StartCoroutine(SetIsShootingCoroutine());
    }

    public IEnumerator SetIsShootingCoroutine()
    {
        IsShooting = true;

        yield return new WaitForSeconds(0.5f);

        IsShooting = false;
    }
}
