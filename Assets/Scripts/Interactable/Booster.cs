using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : RotationalInteractable
{

    #region Editor Fields

    [SerializeField] private float _boosterSpeed = 1.0f;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _boosterParticle;

    #endregion

    #region Private Variables

    #endregion

    #region Unity Loops

    public override void Update()
    {
        base.Update();
       
        if (_currentPlayer == null) { return; }
        
        if (!_currentPlayer.IsShooting) { return; }
        
        BoostShip();

        _boosterParticle.Stop();
    }
    #endregion

    private void BoostShip()
    {
        _boosterParticle.Play();
        _rb.AddForce(-(RotatorTransform.transform.up * _boosterSpeed));
    }
}