using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private float _speed;

    #endregion

    #region Private Varaibles

    private Rigidbody _rb;

    #endregion

    #region Unity Loops

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
