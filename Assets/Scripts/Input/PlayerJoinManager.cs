using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private PlayerInputHandler[] _playerInputs;

    #endregion

    #region Private Variables

    private PlayerJoinNPC[] _playerJoinNPC;

    private int _playerJoinNPCIndex = -1;
    private int _amountOfPlayersActive = 0;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _playerJoinNPC = FindObjectsOfType<PlayerJoinNPC>();

        foreach (PlayerInputHandler _playerInput in _playerInputs)
        {
            _playerInput.OnTrySpawn += HandleSpawn;
            _playerInput.OnJump += HandleJump;
        }
    }

    private void OnDestroy()
    {
        foreach (PlayerInputHandler _playerInput in _playerInputs)
        {
            _playerInput.OnTrySpawn -= HandleSpawn;
            _playerInput.OnJump -= HandleJump;
        }
    }

    #endregion

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
            StartCoroutine(PlayerJoinAnimation(0, playerInput, 2));
        }
        else if (_amountOfPlayersActive == 2)
        {
            StartCoroutine(PlayerJoinAnimation(1, playerInput, 3));
        }
        else if (_amountOfPlayersActive == 3)
        {
            StartCoroutine(PlayerJoinAnimation(2, playerInput, 0));
        }
    }

    private void FindAmountOfPlayersActive()
    {
        _amountOfPlayersActive = 0;
        for (int i = 0; i < _playerInputs.Length; i++)
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
    private IEnumerator PlayerJoinAnimation(int index, PlayerInputHandler playerInput, int nextPlayerIndex)
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
        _playerInputs[_playerJoinNPCIndex].IsPlayerActive = true;

        if (nextPlayerIndex < 2) { yield break; }

        _playerInputs[nextPlayerIndex].gameObject.SetActive(true);
    }

    private void HandleJump(InputAction.CallbackContext playerInput)
    {
        for (int i = 0; i < _playerInputs.Length; i++)
        {
            if (!_playerInputs[i].IsPlayerActive && !_playerInputs[i].CanPlayerSpawn)
            {
                _playerInputs[i].IsPlayerActive = true;
            }
        }
    }
}