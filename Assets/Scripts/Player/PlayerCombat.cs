using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Collider _attackHitbox;
    [SerializeField] private Collider _dashHitbox;

    #endregion

    public void Hit()
    {
        StartCoroutine(EnableHitboxRoutine());
    }

    public void DashHit()
    {
        StartCoroutine(EnableDashHitboxRoutine());
    }

    private IEnumerator EnableHitboxRoutine()
    {
        _attackHitbox.enabled = true;

        yield return new WaitForSeconds(0.3f);

        _attackHitbox.enabled = false;
    }

    private IEnumerator EnableDashHitboxRoutine()
    {
        _dashHitbox.enabled = true;

        yield return new WaitForSeconds(0.6f);

        _dashHitbox.enabled = false;
    }
}
