using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeTrigger : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Pickaxe _pickaxeInteractable;
    [SerializeField] private GameObject _impactParticlesPrefab;
    [SerializeField] private Transform _pickaxeTipTransform;

    #endregion

    #region Private Varaibles

    private DamageType _damageType;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _damageType = DamageType.Base;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        CheckForMinable(other);
        CheckForEnemy(other);
    }

    private void CheckForMinable(Collider other)
    {
        if (!other.TryGetComponent<Minable>(out Minable minable)) { return; }

        GameObject impactParticles = Instantiate(_impactParticlesPrefab, _pickaxeTipTransform.position, Quaternion.identity);

        float damage = _pickaxeInteractable.GetHitSpeed();

        minable.Damage(damage);

        _pickaxeInteractable.ApplyImpactForce();
    }

    private void CheckForEnemy(Collider other)
    {
        if (!other.TryGetComponent<Damageable>(out Damageable damageable)) { return; }

        float damage = _pickaxeInteractable.GetHitSpeed();

        damageable.Damage((int)damage, _damageType);

        _pickaxeInteractable.ApplyImpactForce();
    }
}
