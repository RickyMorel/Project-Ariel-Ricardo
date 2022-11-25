using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private float _speed;
    [SerializeField] private int _damage = 20;

    #endregion

    #region Private Varaibles

    private Rigidbody _rb;

    #endregion

    #region Public Properties

    public int Damage => _damage;

    #endregion

    #region Unity Loops

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(string ownerTag)
    {
        gameObject.tag = ownerTag;
        _rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    #endregion
}
