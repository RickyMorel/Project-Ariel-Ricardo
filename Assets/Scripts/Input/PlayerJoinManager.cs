using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private List<PlayerInputHandler> _playerInputs = new List<PlayerInputHandler>();
    private PlayerJoinNPC[] _playerJoinNPC;
    private PlayerInputManager _playerInputManager;
    private ShipFastTravel _shipFastTravel;

    private int _playerJoinNPCIndex = -1;
    private int _amountOfPlayersActive = 0;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _playerJoinNPC = FindObjectsOfType<PlayerJoinNPC>();
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
        _shipFastTravel = FindObjectOfType<ShipFastTravel>();
        _playerInputs = FindObjectsOfType<PlayerInputHandler>().ToList<PlayerInputHandler>();
        _playerInputManager.onPlayerJoined += HandlePlayerJoined;
    }

    private void OnDestroy()
    {
        foreach (PlayerInputHandler playerInput in _playerInputs)
        {
            playerInput.OnTrySpawn -= HandleSpawn;
            playerInput.OnJump -= HandleJump;
        }
    }

    #endregion

    private void HandlePlayerJoined(PlayerInput player)
    {
        PlayerInputHandler playerInput = player.GetComponent<PlayerInputHandler>();
        _playerInputs.Add(playerInput);
        playerInput.OnTrySpawn += HandleSpawn;
        playerInput.OnJump += HandleJump;
        playerInput.CanPlayerSpawn = true;

        player.transform.position = transform.position;
    }

    private void HandleSpawn(PlayerInputHandler playerInput)
    {
        if (!playerInput.CanPlayerSpawn) { return; }

        _playerJoinNPCIndex = FindCorrectPlayerJoinNPC();

        if (_playerJoinNPCIndex < 0) { return; }

        FindAmountOfPlayersActive();

        TransportToSpawnLocation(playerInput);

    }

    private void TransportToSpawnLocation(PlayerInputHandler playerInput)
    {
        if (_amountOfPlayersActive == 1)
        {
            StartCoroutine(PlayerJoinAnimation(0, playerInput));
        }
        else if (_amountOfPlayersActive == 2)
        {
            StartCoroutine(PlayerJoinAnimation(1, playerInput));
        }
        else if (_amountOfPlayersActive == 3)
        {
            StartCoroutine(PlayerJoinAnimation(2, playerInput));
        }
    }

    private void FindAmountOfPlayersActive()
    {
        _amountOfPlayersActive = 0;
        for (int i = 0; i < _playerInputs.Count; i++)
        {
            if(_playerInputs[i].IsPlayerActive)
            {
                _amountOfPlayersActive++;
            }
        }
    }

    private int FindCorrectPlayerJoinNPC()
    {
        for (int i = 0; i < _playerJoinNPC.Length; i++)
        {
            if (_playerJoinNPC[i].CurrentPlayer != null)
            {
                return i;
            }
        }
        return -1;
    }
    private IEnumerator PlayerJoinAnimation(int index, PlayerInputHandler playerInput)
    {
        int indexAux = index;
        _playerInputs[_playerJoinNPCIndex].IsPlayerActive = false;
        playerInput.CanPlayerSpawn = false;
        _playerJoinNPC[_playerJoinNPCIndex].PlayerJoinTimelines[indexAux].Play();

        yield return new WaitForSeconds(0.5f);

        _playerJoinNPC[_playerJoinNPCIndex].SteamParticles[indexAux].Play();

        yield return new WaitForSeconds(2.5f);

        playerInput.transform.position = _playerJoinNPC[_playerJoinNPCIndex].SpawnLocations[indexAux].transform.position;

        yield return new WaitForSeconds(1);

        _playerJoinNPC[_playerJoinNPCIndex].SteamParticles[indexAux].Stop();
        playerInput.IsPlayerActive = true;
        _playerInputs[_playerJoinNPCIndex].IsPlayerActive = true;

        _shipFastTravel.Cameras = FindObjectsOfType<CinemachineBrain>(true);
        _shipFastTravel.Cameras.OrderBy(p => p.name).ToList();
        _shipFastTravel.VCams = FindObjectsOfType<CinemachineVirtualCamera>(true);
        _shipFastTravel.VCams.OrderBy(p => p.name).ToList();
        _shipFastTravel.ToggleCamera(false);
        if (index == 0)
        {
            _shipFastTravel.VCams[0].gameObject.layer = 29;
            _shipFastTravel.Cameras[1].OutputCamera.rect = new Rect(0, 0.5f, 1, 0.5f);
            _shipFastTravel.Cameras[0].OutputCamera.rect = new Rect(0, 0, 1, 0.5f);
        }
        else if (index == 1)
        {
            _shipFastTravel.VCams[0].gameObject.layer = 30;
            _shipFastTravel.Cameras[2].OutputCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            _shipFastTravel.Cameras[1].OutputCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            _shipFastTravel.Cameras[0].OutputCamera.rect = new Rect(0, 0, 1, 0.5f);
        }
        else if (index == 2)
        {
            _shipFastTravel.VCams[0].gameObject.layer = 31;
            _shipFastTravel.Cameras[3].OutputCamera.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            _shipFastTravel.Cameras[2].OutputCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            _shipFastTravel.Cameras[1].OutputCamera.rect = new Rect(0, 0, 0.5f, 0.5f);
            _shipFastTravel.Cameras[0].OutputCamera.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
        }
    }

    private void HandleJump(InputAction.CallbackContext playerInput)
    {
        for (int i = 0; i < _playerInputs.Count; i++)
        {
            if (!_playerInputs[i].IsPlayerActive && !_playerInputs[i].CanPlayerSpawn)
            {
                _playerInputs[i].IsPlayerActive = true;
            }
        }
    }
}