using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(SphereCollider))]
public class ShipFastTravel : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private PlayableDirector _startFastTravel;
    [SerializeField] private PlayableDirector _endFastTravel;

    [SerializeField] private ParticleSystem _blackHole;

    [SerializeField] private GameObject _mainShip;
    [SerializeField] private GameObject _shipParent;

    #endregion

    #region Getters and Setters

    public bool WantToTravel { get { return _wantToTravel; } set { _wantToTravel = value; } }

    public FastTravelNPC FastTravelNPC { get { return _fastTravelNPC; } set { _fastTravelNPC = value; } }

    #endregion

    #region Private Variables

    private int _playersInShip = 0;
    private int _playersActive = 0;

    private bool _wantToTravel = false;

    private ShipDoor _shipDoor;
    private PlayerInputHandler[] _isPlayerActive;

    private FastTravelNPC _fastTravelNPC;

    private Coroutine _lastRoutine = null;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _isPlayerActive = FindObjectsOfType<PlayerInputHandler>();
        _shipDoor = _mainShip.GetComponentInChildren<ShipDoor>();
        _lastRoutine = StartCoroutine(DetachFromShip());
    }

    #endregion

    private void CheckPlayersInShip()
    {
        _playersActive = 0;
        for (int i = 0; i < _isPlayerActive.Length; i++)
        {
            if (_isPlayerActive[i].IsPlayerActive == true)
            {
                _playersActive++;
            }
        }

        if ((_playersActive != _playersInShip) || (_shipDoor.IsWantedDoorOpen == true)) { return; }

        if (!_wantToTravel) { return; }

        _wantToTravel = false;
        _mainShip.transform.SetParent(_shipParent.transform);
        StartCoroutine(FastTravelCoroutine());
        AttachToShip();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerInputHandler>() == null) { return; }

        _playersInShip++;

        StopCoroutine(_lastRoutine);
        AttachToShip();
        CheckPlayersInShip();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInputHandler>() == null) { return; }

        _lastRoutine = StartCoroutine(DetachFromShip());
        
        _playersInShip--;
    }

    private IEnumerator FastTravelCoroutine()
    {
        //Start the animation for the fast travel
        yield return new WaitForSeconds(4);
        _startFastTravel.Play();
        _blackHole.Play();
        //Stops the animation and takes the main ship out of its parent
        yield return new WaitForSeconds(3);
        _startFastTravel.Stop();
        _endFastTravel.Play();
        _mainShip.transform.SetParent(null);
        _mainShip.transform.position = _fastTravelNPC.TravelToPosition.transform.position;
        //Stops all remaining animations
        yield return new WaitForSeconds(2);
        _endFastTravel.Stop();
        _blackHole.Stop();
    }

    private void AttachToShip()
    {
        for (int i = 0; i < _isPlayerActive.Length; i++)
        {
            if (_isPlayerActive[i].IsPlayerActive == true)
            {
                _isPlayerActive[i].GetComponentInParent<PlayerStateMachine>().AttachToShip(true);
            }
        }
    }

    private IEnumerator DetachFromShip()
    {
        yield return new WaitForSeconds(10);
        for (int i = 0; i < _isPlayerActive.Length; i++)
        {
            if (_isPlayerActive[i].IsPlayerActive == true)
            {
                _isPlayerActive[i].GetComponentInParent<PlayerStateMachine>().AttachToShip(false);
            }
        }
    }
}