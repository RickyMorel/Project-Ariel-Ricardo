using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerJoinManager : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _playerPrefab;

    #endregion

    #region Private Variables

    private List<PlayerInputHandler> _playerInputs = new List<PlayerInputHandler>();
    private PlayerJoinNPC[] _playerJoinNPC;

    private int _playerJoinNPCIndex = -1;
    private int _amountOfPlayersActive = 0;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _playerJoinNPC = FindObjectsOfType<PlayerJoinNPC>();
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

    public void SpawnPlayer(Player playerInputs, int playerID)
    {
        GameObject player = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        PlayerInputHandler playerInput = player.GetComponent<PlayerInputHandler>();
        _playerInputs.Add(playerInput);
        playerInput.OnTrySpawn += HandleSpawn;
        playerInput.OnJump += HandleJump;
        playerInput.CanPlayerSpawn = true;
        playerInput.PlayerId = playerID;
        playerInput.PlayerInputs = playerInputs;
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

    private void HandleJump()
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