using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(SphereCollider))]
public class ShipFastTravel : MonoBehaviour
{
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

    private Ship _mainShip;

    private CameraManager _cameraManager;

    private Coroutine _lastRoutine = null;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _mainShip = FindObjectOfType<Ship>();
        _isPlayerActive = FindObjectsOfType<PlayerInputHandler>();
        _shipDoor = _mainShip.GetComponentInChildren<ShipDoor>();
        _lastRoutine = StartCoroutine(DetachFromShip());
        _cameraManager = FindObjectOfType<CameraManager>();
        TimelinesManager.Instance.BlackHoleParticle.gameObject.transform.SetParent(_mainShip.transform);
        TimelinesManager.Instance.BlackHoleParticle.gameObject.transform.localPosition =new Vector3(2.5f,0,0);
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

        if (_playersActive != _playersInShip) { return; }

        _cameraManager.ToggleCamera(true);

        if (_shipDoor.IsWantedDoorOpen == true) { return; }

        if (!_wantToTravel) { return; }

        _wantToTravel = false;
        _mainShip.gameObject.transform.SetParent(TimelinesManager.Instance.MainShipParentForTheTimeline.transform);
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

        _cameraManager.ToggleCamera(false);
        _lastRoutine = StartCoroutine(DetachFromShip());
        _playersInShip--;
    }

    private IEnumerator FastTravelCoroutine()
    {
        //Start the animation for the fast travel
        yield return new WaitForSeconds(4);
        TimelinesManager.Instance.StartFastTravelTimeline.Play();
        TimelinesManager.Instance.BlackHoleParticle.Play();
        //Stops the animation and takes the main ship out of its parent
        yield return new WaitForSeconds(2);
        TimelinesManager.Instance.StartFastTravelTimeline.Stop();
        TimelinesManager.Instance.EndFastTravelTimeline.Play();
        _mainShip.gameObject.transform.SetParent(null);
        _mainShip.gameObject.transform.position = _fastTravelNPC.TravelToPosition.transform.position;
        //Stops all remaining animations
        yield return new WaitForSeconds(2);
        TimelinesManager.Instance.EndFastTravelTimeline.Stop();
        TimelinesManager.Instance.BlackHoleParticle.Stop();
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
        yield return new WaitForSeconds(2);
        for (int i = 0; i < _isPlayerActive.Length; i++)
        {
            if (_isPlayerActive[i].IsPlayerActive == true)
            {
                _isPlayerActive[i].GetComponentInParent<PlayerStateMachine>().AttachToShip(false);
            }
        }
    }
}