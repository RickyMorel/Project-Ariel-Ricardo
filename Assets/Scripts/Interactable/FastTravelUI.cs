using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelUI : NPC
{
    #region Public Properties

    public GameObject FastTravelOptions;

    public Transform[] TravelPos;

    public Transform _travelToPosition;

    public bool WantToTravel;

    #endregion

    #region Private Properties

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
            FastTravelOptions.SetActive(false);
        }
        else
        {
            FastTravelOptions.SetActive(true);
        }
    }

    public void TravelTo(int posIndex)
    {
        _travelToPosition = TravelPos[posIndex];
        WantToTravel = true;
        _exitInteractable.CheckExitInteraction();
    }
}