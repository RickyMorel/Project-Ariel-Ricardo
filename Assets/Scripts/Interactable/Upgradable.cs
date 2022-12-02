using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable : Interactable
{
    #region Editor Fields

    [Header("Upgrades")]
    [SerializeField] private GameObject[] _upgradeSockets;
    [SerializeField] private Upgrade[] _upgrades;

    [Header("FX")]
    [SerializeField] private ParticleSystem _upgradeParticles;

    #endregion

    #region Private Variables

    private ChipType[] _upgradeSocketsTypes = { ChipType.None, ChipType.None };
    private List<GameObject> _chipInstances = new List<GameObject>();
    private int _currentLevel = 0;

    #endregion

    #region Unity Loops

    public virtual void Start()
    {
        //TODO: read current level from save data

        EnableUpgradeMesh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            RemoveUpgrades();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        //_upgradesCanvas.enabled = true;
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        //_upgradesCanvas.enabled = false;
    }

    #endregion

    public void RemoveUpgrades()
    {
        Debug.Log("RemoveUpgrades");
        _upgradeSocketsTypes[0] = ChipType.None;
        _upgradeSocketsTypes[1] = ChipType.None;

        foreach (GameObject chip in _chipInstances)
        {
            Destroy(chip.gameObject);
        }
        _chipInstances.Clear();

        EnableUpgradeMesh();
        PlayUpgradeFX();
    }

    public bool TryUpgrade(UpgradeChip upgradeChip)
    {
        int socketIndex = -1;
        bool foundEmptySocket = false;
        foreach (ChipType socket in _upgradeSocketsTypes)
        {
            socketIndex++;

            if (socket != ChipType.None) { continue; }

            foundEmptySocket = true;

            break;
        }

        if (!foundEmptySocket) { return false; }

        _upgradeSocketsTypes[socketIndex] = upgradeChip.ChipType;

        Upgrade(upgradeChip, socketIndex);

        return true;
    }

    public void Upgrade(UpgradeChip upgradeChip, int socketIndex)
    {
        PlaceChip(upgradeChip, socketIndex);
        EnableUpgradeMesh();
        PlayUpgradeFX();
    }

    private void PlayUpgradeFX()
    {
        _upgradeParticles.Play();
    }

    private void PlaceChip(UpgradeChip upgradeChip, int socketIndex)
    {
        GameObject newChip = Instantiate(upgradeChip.Prefab, _upgradeSockets[socketIndex].transform);
        newChip.transform.localPosition = new Vector3(-0.025f, 0.15f, 0f);
        newChip.transform.localEulerAngles = new Vector3(90f, 0f,-90f);
        _chipInstances.Add(newChip);
    }

    public void EnableUpgradeMesh()
    {
        foreach (Upgrade upgrade in _upgrades)
        {
            upgrade.UpgradeMesh.SetActive(false);
        }

        int upgradeMeshIndex = -1;
        foreach (Upgrade upgrade in _upgrades)
        {
            upgradeMeshIndex++;
            if(upgrade._socket_1_ChipType == _upgradeSocketsTypes[0] && upgrade._socket_2_ChipType == _upgradeSocketsTypes[1])
            {
                break;
            }
        }

        _upgrades[upgradeMeshIndex].UpgradeMesh.SetActive(true);
    }
}

#region Helper Classes

[System.Serializable]
public class Upgrade
{
    public GameObject UpgradeMesh;
    public ChipType _socket_1_ChipType;
    public ChipType _socket_2_ChipType;
}

#endregion
