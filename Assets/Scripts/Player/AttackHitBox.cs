using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AttackHitBox : MonoBehaviour
{
    #region Public Properties

    public event Action<GameObject> OnHit;

    #endregion

    #region Editor Fields

    [SerializeField] private Damageable _ownHealth;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Damageable>(out Damageable enemyHealth)) { return; }

        if (_ownHealth != null && enemyHealth == _ownHealth) { return; }

        if(gameObject.tag == enemyHealth.tag) { return; }

        Debug.Log("Damage enemy");

        OnHit?.Invoke(other.gameObject);

        if (enemyHealth is AIHealth)
        {
            AIHealth aiHealth = (AIHealth)enemyHealth;
            if (aiHealth.CanKill) { enemyHealth.Damage(20, DamageType.Base); }
            else { aiHealth.Hurt(DamageType.Base); }
        }
        

        if (enemyHealth is PlayerHealth) 
        { 
            PlayerHealth playerHealth = enemyHealth as PlayerHealth;
            playerHealth.Hurt(DamageType.Base); 
        }
    }
}