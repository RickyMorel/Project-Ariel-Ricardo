using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GStateMonitor : MonoBehaviour
{
    public string State;
    public float StateStrength;
    public float StateDecayRate;
    public WorldStates Beliefs;
    public GameObject ResourcePrefab;
    public string QueueName;
    public string WorldState;
    public GAction Action;

    private bool _stateFound = false;
    private float _initialStrength;

    private void Awake()
    {
        Beliefs = GetComponent<GAgent>().Beliefs;
        _initialStrength = StateStrength;
    }

    private void LateUpdate()
    {
        if (Action.IsRunning)
        {
            _stateFound = false;
            StateStrength = _initialStrength;
        }

        if (!_stateFound && Beliefs.HasState(State))
            _stateFound = true;

        if (_stateFound)
        {
            StateStrength -= StateDecayRate * Time.deltaTime;

            if(StateStrength <= 0)
            {
                Vector3 location = new Vector3(transform.position.x, ResourcePrefab.transform.position.y, transform.position.z);
                GameObject resourceInstance = Instantiate(ResourcePrefab, location, ResourcePrefab.transform.rotation);
                _stateFound = false;
                StateStrength = _initialStrength;
                Beliefs.RemoveState(State);
                GWorld.Instance.GetQueue(QueueName).AddResource(resourceInstance);
                GWorld.Instance.GetWorld().ModifyState(WorldState, 1);
            }
        }
    }
}
