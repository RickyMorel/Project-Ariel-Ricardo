using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelNPC : NPC
{
    #region Editor Fields

    [SerializeField] private Transform[] _travelPos;

    [SerializeField] private GameObject _fastTravelOptions;

    #endregion

    #region Public Properties

    public Transform TravelToPosition => _travelToPosition;

    #endregion

    #region Private Variables

    private Transform _travelToPosition;

    private PlayerInteractionController _exitInteractable;

    #endregion

    #region Unity Loops

    public virtual void Update()
    {
        DisplayUI();
    }

    #endregion

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!other.gameObject.TryGetComponent<PlayerInteractionController>(out PlayerInteractionController playerInteractionController)) { return; }

        _exitInteractable = playerInteractionController;
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        _exitInteractable = null;
    }

    private void DisplayUI()
    {
        if (_currentPlayer == null)
        {
            _fastTravelOptions.SetActive(false);
        }
        else
        {
            _fastTravelOptions.SetActive(true);
        }
    }

    public void TravelTo(int posIndex)
    {
        ShipFastTravel shipFastTravel = Ship.Instance.GetComponent<ShipFastTravel>();
        shipFastTravel.FastTravelNPC = this;
        _travelToPosition = _travelPos[posIndex];
        shipFastTravel.WantToTravel = true;
        _exitInteractable.CheckExitInteraction();
    }
}