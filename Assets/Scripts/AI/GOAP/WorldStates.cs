using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Helper Classes

[System.Serializable]
public class WorldState
{
    public string key;
    public int value;
}

#endregion

public class WorldStates
{
    #region Public Properties

    public Dictionary<string, int> States;

    #endregion

    public WorldStates()
    {
        States = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return States.ContainsKey(key);
    }

    public void AddState(string key, int value)
    {
        States.Add(key, value);
    }

    public void RemoveState(string key)
    {
        if (!States.ContainsKey(key)) { return; }

        States.Remove(key);
    }

    public void SetState(string key, int setValue)
    {
        if (!States.ContainsKey(key)) { AddState(key, setValue); return; }

        States[key] = setValue;
    }

    public void ModifyState(string key, int modifiedValue)
    {
        if (!States.ContainsKey(key)) { AddState(key, modifiedValue); return; }

        States[key] += modifiedValue;

        if (States[key] <= 0)
            RemoveState(key);
    }

    public Dictionary<string, int> GetStates()
    {
        return States;
    }
}
