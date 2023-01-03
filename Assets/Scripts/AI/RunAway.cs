using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : GAction
{
    #region Editor Fields

    [Header("Action Specific Variables")]
    [SerializeField] private float _maxDistance = 40f;

    #endregion

    #region Private Variables

    private GameObject _currentRunAwayObj;

    #endregion

    public override bool PrePerform()
    {
        Vector3 destination = RandomNavmeshLocation(_maxDistance);

        GameObject newTargetObj = new GameObject();
        newTargetObj.transform.position = destination;
        newTargetObj.name = "RunAwayTargetObj";

        _currentRunAwayObj = newTargetObj;

        Target = newTargetObj;

        if (Target == null) { return false; }

        Debug.Log("Will Run: " + Target);

        return true;
    }

    public override bool PostPeform()
    {
        if(_currentRunAwayObj != null) { Destroy(_currentRunAwayObj); }

        Debug.Log("PostPeform Run");

        return true;
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        int foundNavmeshMask = NavMesh.AllAreas;
        int crashPreventionCounter = 0;

        //Get current mesh area mask
        Agent.SamplePathPosition(NavMesh.AllAreas, 0f, out NavMeshHit navMeshHit);

        //while current mask != the sampled mask, look again
        while(navMeshHit.mask != foundNavmeshMask)
        {
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
            {
                Debug.Log("Sample mask: " + hit.mask);
                Debug.Log("Current mask: " + navMeshHit.mask);
                finalPosition = hit.position;
                foundNavmeshMask = hit.mask;
            }

            crashPreventionCounter++;

            //prevents from looping while infinitly if doesn't find position
            if(crashPreventionCounter > 30) { finalPosition = transform.position; break; }
        }

        return finalPosition;
    }
}
