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

    public bool WantToTravel;

    public Transform TravelToPosition;

    public PlayerInteractionController _exitInteractable;

    #endregion

    #region Unity Loops

    public virtual void Update()
    {
        DisplayUI();
    }

    #endregion

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
        TravelToPosition = _travelPos[posIndex];
        WantToTravel = true;
        _exitInteractable.CheckExitInteraction();
    }
}
