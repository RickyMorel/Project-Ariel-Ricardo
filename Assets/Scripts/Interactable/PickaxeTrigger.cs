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

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Minable>(out Minable minable)) { return; }

        GameObject impactParticles = Instantiate(_impactParticlesPrefab, _pickaxeTipTransform.position, Quaternion.identity);

        float damage = _pickaxeInteractable.GetHitSpeed();

        minable.Damage(damage);

        _pickaxeInteractable.ApplyImpactForce();
    }
}
