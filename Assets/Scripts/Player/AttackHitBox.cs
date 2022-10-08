using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class AttackHitBox : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private string _enemyTag = "Player";
    [SerializeField] private PlayerHealth _ownHealth;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != _enemyTag) { return; }

        Debug.Log("Got player Tag: " + other.gameObject.name);

        if(!other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth)) { return; }

        Debug.Log("Got player health: " + other.gameObject.name);

        if (playerHealth == _ownHealth) { return; }

        Debug.Log("Is not self: " + other.gameObject.name);

        playerHealth.Hurt();
    }
}
