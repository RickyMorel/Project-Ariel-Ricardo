using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsManager : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _damagePopup;
    [SerializeField] private GameObject _repairPopup;
    [SerializeField] private GameObject _repairCostsCanvas;
    [SerializeField] private GameObject _chipPickup;
    [SerializeField] private GameObject _upgradeParticles;
    [SerializeField] private GameObject _interactableFriedParticles;
    [SerializeField] private GameObject _electricParticles;
    [SerializeField] private GameObject _fireParticles;
    [SerializeField] private GameObject _meleeFloorHitParticles;

    #endregion

    #region Private Variables

    private static GameAssetsManager _instance;

    #endregion

    #region Public Properties

    public GameObject DamagePopup => _damagePopup;
    public GameObject RepairPopup => _repairPopup;
    public GameObject RepairCostsCanvas => _repairCostsCanvas;
    public GameObject ChipPickup => _chipPickup;
    public GameObject UpgradeParticles => _upgradeParticles;
    public GameObject InteractableFriedParticles => _interactableFriedParticles;
    public GameObject ElectricParticles => _electricParticles;
    public GameObject FireParticles => _fireParticles;
    public GameObject MeleeFloorHitParticles => _meleeFloorHitParticles;

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
