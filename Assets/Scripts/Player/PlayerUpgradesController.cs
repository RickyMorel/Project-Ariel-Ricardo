using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesController : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _chipPickupPrefab;
    [SerializeField] private Transform _handTransform;

    #endregion

    #region Private Variables

    private PlayerInputHandler _playerInput;
    private BaseInteractionController _interactionController;
    private UpgradeChip _currentUpgradeChip;
    private GameObject _currentChipObjInstance;

    #endregion

    private void Start()
    {
        _playerInput = GetComponent<PlayerInputHandler>();
        _interactionController = GetComponent<BaseInteractionController>();

        _chipPickupPrefab = GameAssetsManager.Instance.ChipPickup;

        _playerInput.OnUpgrade += HandleUpgrade;
    }

    private void OnDestroy()
    {
        _playerInput.OnUpgrade -= HandleUpgrade;
    }

    public void CarryChip(ChipPickup chipPickup)
    {
        DropChip();

        _currentUpgradeChip = chipPickup.ChipSO;
        _currentChipObjInstance = Instantiate(_currentUpgradeChip.Prefab, _handTransform);
        _currentChipObjInstance.transform.localPosition = Vector3.zero;

        Destroy(chipPickup.gameObject);
    }

    public void DropChip()
    {
        if(_currentUpgradeChip == null) { return; }

        GameObject chipPickupInstance = Instantiate(_chipPickupPrefab, transform.position, Quaternion.identity);
        chipPickupInstance.GetComponent<ChipPickup>().Initialize(_currentUpgradeChip);

        _currentUpgradeChip = null;
        Destroy(_currentChipObjInstance.gameObject);
    }

    private void HandleRemoveUpgrades()
    {
        if (_interactionController.CurrentInteractable == null) { return; }

        if ((_interactionController.CurrentInteractable is Upgradable) == false) { return; }

        Upgradable upgradable = _interactionController.CurrentInteractable as Upgradable;

        upgradable.RemoveUpgrades();
    }


    //This calls when the player presses the upgrade button
    private void HandleUpgrade()
    {
        if (_interactionController.CurrentInteractable == null) { return; }

        if ((_interactionController.CurrentInteractable is Upgradable) == false) { return; }

        Upgradable upgradable = _interactionController.CurrentInteractable as Upgradable;
        bool didUpgrade = upgradable.TryUpgrade(_currentUpgradeChip);

        if (didUpgrade)
        {
            _currentUpgradeChip = null;
            Destroy(_currentChipObjInstance.gameObject);
        }
    }
}
