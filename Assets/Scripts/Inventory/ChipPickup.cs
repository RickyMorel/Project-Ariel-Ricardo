using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipPickup : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private UpgradeChip _chipSO;
    [SerializeField] private TextMeshPro _nameText;

    #endregion

    #region Public Properties

    public UpgradeChip ChipSO => _chipSO;
    public Rigidbody Rb => _rb;
    public PlayerUpgradesController PrevPlayerUpgradesController => _prevPlayerUpgradesController;

    #endregion

    #region Private Variables

    private Rigidbody _rb;
    private PlayerUpgradesController _prevPlayerUpgradesController = null;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if(_chipSO != null) { Initialize(_chipSO); }
    }

    #endregion

    public void Initialize(UpgradeChip chip)
    {
        _chipSO = chip;
        GameObject chipObj = Instantiate(_chipSO.Prefab, transform);
        chipObj.transform.localPosition = Vector3.zero;
        chipObj.transform.localEulerAngles = Vector3.zero;

        _nameText.text = _chipSO.ChipName;
    }

    public void PickUp(PlayerUpgradesController playerUpgradesController)
    {
        _prevPlayerUpgradesController = playerUpgradesController;
        _prevPlayerUpgradesController.CarryChip(this);
    }
}
