using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsManager : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _damagePopup;
    [SerializeField] private GameObject _chipPickup;
    [SerializeField] private GameObject _upgradeParticles;
    [SerializeField] private GameObject _electricParticles;
    [SerializeField] private GameObject _fireParticles;

    #endregion

    #region Private Variables

    private static GameAssetsManager _instance;

    #endregion

    #region Public Properties

    public GameObject DamagePopup => _damagePopup;
    public GameObject ChipPickup => _chipPickup;
    public GameObject UpgradeParticles => _upgradeParticles;
    public GameObject ElectricParticles => _electricParticles;
    public GameObject FireParticles => _fireParticles;

    public static GameAssetsManager Instance { get { return _instance; } }

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
