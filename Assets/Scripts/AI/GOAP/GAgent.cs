using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

#region Helper Classes

public class SubGoal
{
    public Dictionary<string, int> Sgoals;
    public bool Remove;

    public SubGoal(string key, int value, bool remove)
    {
        Sgoals = new Dictionary<string, int>();
        Sgoals.Add(key, value);
        Remove = remove;
    } 
}

#endregion

public class GAgent : MonoBehaviour
{
    #region Public Properties

    public List<GAction> Actions = new List<GAction>();
    public Dictionary<SubGoal, int> Goals = new Dictionary<SubGoal, int>();
    public GInventory Inventory = new GInventory();
    public WorldStates Beliefs = new WorldStates();
    public GAction CurrentAction;

    #endregion

    #region Private Variables

    private GPlanner _planner;
    private Queue<GAction> _actionQueue;
    private SubGoal _currentGoal;

    private Vector3 _destination = Vector3.zero;
    private bool _invoked = false;

    #endregion

    #region Unity Loops

    public virtual void Start()
    {
        GAction[] acts = GetComponents<GAction>();

        foreach (GAction act in acts)
        {
            Actions.Add(act);
        }
    }

    private void LateUpdate()
    {
        if (CurrentAction != null && CurrentAction.IsRunning)
        {
            float distanceToTarget = Vector3.Distance(_destination, transform.position);
            if (distanceToTarget < 2f)
            {
                if (!_invoked)
                {
                    Invoke(nameof(CompleteAction), CurrentAction.Duration);
                    _invoked = true;
                }
            }
            return;
        }

        CalculateCurrentGoal();

        CheckIfRemoveGoal();

        TryPerformGoal();
    }

    private void TryPerformGoal()
    {
        if (_actionQueue == null || _actionQueue.Count < 1) { return; }

        CurrentAction = _actionQueue.Dequeue();
        if (CurrentAction.PrePerform())
        {
            if (CurrentAction.Target == null && CurrentAction.TargetTag != "")
                CurrentAction.Target = GameObject.FindWithTag(CurrentAction.TargetTag);

            if (CurrentAction.Target != null)
            {
                CurrentAction.IsRunning = true;

                _destination = CurrentAction.Target.transform.position;
                Transform dest = CurrentAction.Target.transform.Find("Destination");
                if (dest != null) { _destination = dest.position; }

                CurrentAction.Agent.SetDestination(_destination);
            }
        }
        else
        {
            _actionQueue = null;
        }
    }

    private void CheckIfRemoveGoal()
    {
        if (_actionQueue != null && _actionQueue.Count == 0)
        {
            if (_currentGoal.Remove)
            {
                Goals.Remove(_currentGoal);
            }
            _planner = null;
        }
    }

    private void CalculateCurrentGoal()
    {
        if (_planner != null && _actionQueue != null) { return; }

        _planner = new GPlanner();

        var sortedGoals = from entry in Goals orderby entry.Value descending select entry;

        foreach (KeyValuePair<SubGoal, int> subGoal in sortedGoals)
        {
            _actionQueue = _planner.Plan(Actions, subGoal.Key.Sgoals, Beliefs);
            if (_actionQueue != null)
            {
                _currentGoal = subGoal.Key;
                break;
            }
        }
    }

    #endregion

    private void CompleteAction()
    {
        CurrentAction.IsRunning = false;
        CurrentAction.PostPeform();
        _invoked = false;
    }
}
