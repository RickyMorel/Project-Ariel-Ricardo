using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] protected Collider _attackHitbox;
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected Transform _shootTransform;

    #endregion

    public void Hit()
    {
        StartCoroutine(EnableHitboxRoutine());
    }

    public virtual void Shoot()
    {
        GameObject newProjectile = Instantiate(_projectilePrefab, _shootTransform.position, _shootTransform.rotation);
        newProjectile.GetComponent<Projectile>().Initialize(tag);
    }

    private IEnumerator EnableHitboxRoutine()
    {
        _attackHitbox.enabled = true;

        yield return new WaitForSeconds(0.3f);

        _attackHitbox.enabled = false;
    }
}
