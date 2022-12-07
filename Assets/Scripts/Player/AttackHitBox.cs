using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AttackHitBox : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Damageable _ownHealth;
    [SerializeField] private DamageType _damageType;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.TryGetComponent<Damageable>(out Damageable enemyHealth)) { return; }

        if (enemyHealth == _ownHealth) { return; }

        enemyHealth.Damage(20, _damageType);

        if (enemyHealth is PlayerHealth) 
        { 
            PlayerHealth playerHealth = enemyHealth as PlayerHealth;
            playerHealth.Hurt(); 
        }
    }
}