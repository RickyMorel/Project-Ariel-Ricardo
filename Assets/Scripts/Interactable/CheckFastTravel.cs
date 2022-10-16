using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CheckFastTravel : MonoBehaviour
{
    #region Public Properties

    public GameObject MainShip;

    public GameObject[] PlayersInScene;

    #endregion

    #region Private Variables

    private int _count = 0;

    private FastTravelUI _travelDestination;
    private ShipDoor _doorState;

    #endregion

    #region Unity Loops

    private void Start()
    {
        PlayersInScene = GameObject.FindGameObjectsWithTag("Player");
        _travelDestination = FindObjectOfType<FastTravelUI>();
        _doorState = FindObjectOfType<ShipDoor>();
    }

    private void Update()
    {
        CheckPlayersInShip();
    }

    #endregion

    private void CheckPlayersInShip()
    {
        if ((PlayersInScene.Length != _count) || (_doorState.IsWantedDoorOpen == true)) { return; }

        StartCoroutine("Loading");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }

        _count++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") { return; }

        _count--;
    }

    private IEnumerator Loading()
    {
        yield return new WaitForSeconds(5);
        MainShip.transform.position = _travelDestination._travelToPosition.transform.position;
    }
}