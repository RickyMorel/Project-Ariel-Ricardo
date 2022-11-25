using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Collider _attackHitbox;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _shootTransform;

    #endregion

    public void Hit()
    {
        StartCoroutine(EnableHitboxRoutine());
    }

    public void Shoot()
    {
        GameObject newProjectile = Instantiate(_projectilePrefab, _shootTransform.position, _shootTransform.rotation);
    }

    private IEnumerator EnableHitboxRoutine()
    {
        _attackHitbox.enabled = true;

        yield return new WaitForSeconds(0.3f);

        _attackHitbox.enabled = false;
    }
}
