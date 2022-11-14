/* Prerequisites:
 * 1. Create "JoinGame" Action.
 * 2. Create Map Categories "Assignment" and "UI".
 * 3. Create Joystick and Keyboard Maps in the "Assignment" category that map "JoinGame"
 *    to some buttons/keys such as "Start" on in the Gamepad Template and "Space"
 *    and/or "Enter" in the Keyboard Map.
 * 4. Create Joystick and Keyboard Maps in the "UI" category for controlling the UI.
 * 5. Assign the "Assignment" maps to each Player (assign the Keyboard Map to only 1 Player unless you've
 *    created multiple in different layouts for each Player to use). Ensure they are set to be enabled on start.
 * 6. Assign the "UI" maps to each Player setting them to be disabled on start.
 * 7. Leave joystick auto-assignment enabled.
 */
using UnityEngine;
using System.Collections.Generic;

namespace Rewired.Demos
{
    using Rewired;

    public class PressStartToJoinPlayerSelector : MonoBehaviour
    {
        #region Editor Fields

        [SerializeField] private PlayerJoinManager _playerJoinManager;
        [SerializeField] private int _maxPlayers = 4;

        #endregion

        #region Private Variables

        private List<PlayerMap> _playerMap; // Maps Rewired Player ids to game player ids
        private PlayerInputHandler[] _initialPlayers;
        private int _gamePlayerIdCounter = 0;

        #endregion

        #region Unity Loops

        void Awake()
        {
            _playerMap = new List<PlayerMap>();
            _initialPlayers = FindObjectsOfType<PlayerInputHandler>();
        }

        private void Start()
        {
            int i = 0;
            foreach (PlayerInputHandler player in _initialPlayers)
            {
                AssignNextPlayer(i, player);
                i++;
            }
        }

        void Update()
        {

            // Watch for JoinGame action in each Player
            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
                {
                    AssignNextPlayer(i, null);
                }
            }
        }

        #endregion

        void AssignNextPlayer(int rewiredPlayerId, PlayerInputHandler alreadySpawnedPlayer)
        {
            if (_playerMap.Count >= _maxPlayers)
            {
                Debug.LogError("Max player limit already reached!");
                return;
            }

            int gamePlayerId = GetNextGamePlayerId();

            // Add the Rewired Player as the next open game player slot
            _playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

            Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

            // Disable the Assignment map category in Player so no more JoinGame Actions return
            rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");

            // Enable Default control for this Player now that he has joined
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

            if(alreadySpawnedPlayer == null)
            {
                _playerJoinManager.SpawnPlayer(rewiredPlayer, rewiredPlayerId);
            }
            else
            {
                alreadySpawnedPlayer.PlayerId = rewiredPlayerId;
                alreadySpawnedPlayer.PlayerInputs = rewiredPlayer;
            }
        }

        private int GetNextGamePlayerId()
        {
            return _gamePlayerIdCounter++;
        }

        // This class is used to map the Rewired Player Id to your game player id
        private class PlayerMap
        {
            public int rewiredPlayerId;
            public int gamePlayerId;

            public PlayerMap(int rewiredPlayerId, int gamePlayerId)
            {
                this.rewiredPlayerId = rewiredPlayerId;
                this.gamePlayerId = gamePlayerId;
            }
        }
    }
}