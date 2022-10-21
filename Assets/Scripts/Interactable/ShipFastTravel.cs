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

    #endregion

    #region Public Properties

    public GameObject MainShip;
    public GameObject ShipParent;
    public FastTravelNPC FastTravelNPC;

    #endregion

    #region Getters and Setters

    public bool WantToTravel { get { return _wantToTravel; } set { _wantToTravel = value; } }

    #endregion

    #region Private Variables

    private int _playersInShip = 0;

    private bool _wantToTravel = false;

    private ShipDoor _doorState;
    private PlayerInputHandler[] _playersInScene;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _playersInScene = FindObjectsOfType<PlayerInputHandler>();
        _doorState = FindObjectOfType<ShipDoor>();
    }

    #endregion

    private void CheckPlayersInShip()
    {
        if ((_playersInScene.Length != _playersInShip) || (_doorState.IsWantedDoorOpen == true)) { return; }

        if (!_wantToTravel) { return; }

        _wantToTravel = false;
        MainShip.transform.SetParent(ShipParent.transform);
        StartCoroutine(FastTravelCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }

        _playersInShip++;
        CheckPlayersInShip();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") { return; }

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
        MainShip.transform.SetParent(null);
        MainShip.transform.position = FastTravelNPC.TravelToPosition.transform.position;
        //Stops all remaining animations
        yield return new WaitForSeconds(1);
        _endFastTravel.Stop();
        _blackHole.Stop();
    }
}
