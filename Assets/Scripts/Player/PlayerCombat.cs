using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Collider _attackHitbox;

    #endregion

    public void Hit()
    {
        StartCoroutine(EnableHitboxRoutine());
    }

    private IEnumerator EnableHitboxRoutine()
    {
        _attackHitbox.enabled = true;

        yield return new WaitForSeconds(0.3f);

        _attackHitbox.enabled = false;
    }
}
