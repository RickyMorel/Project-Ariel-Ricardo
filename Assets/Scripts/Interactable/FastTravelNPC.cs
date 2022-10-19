using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelNPC : NPC
{
    #region Editor Fields

    [SerializeField] private Transform[] _travelPos;

    [SerializeField] private GameObject _fastTravelOptions;

    #endregion

    #region Getters and Setters

    public bool WantToTravel { get { return _wantToTravel; } set { _wantToTravel = value; } }

    #endregion

    #region Public Properties

    public Transform TravelToPosition;

    public PlayerInteractionController _exitInteractable;

    public static FastTravelNPC Instance { get; private set; }

    #endregion

    #region Private Varible

    private bool _wantToTravel;

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
        Instance = this;
        TravelToPosition = _travelPos[posIndex];
        WantToTravel = true;
        _exitInteractable.CheckExitInteraction();
    }
}
