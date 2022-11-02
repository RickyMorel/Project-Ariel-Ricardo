using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelCrewMember : GAgent
{
    public override void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("satiated", 1, false);
        Goals.Add(s1, 3);

        SubGoal s3 = new SubGoal("haveWorked", 1, false);
        Goals.Add(s3, 2);

        SubGoal s2 = new SubGoal("feelRested", 1, false);
        Goals.Add(s2, 4);

        SubGoal s4 = new SubGoal("feelSafe", 1, false);
        Goals.Add(s4, 10);

        GoUseFastTravelMachine();

        Invoke(nameof(GetHungry), Random.Range(10, 20));
        Invoke(nameof(GoShop), Random.Range(10, 20));
        Invoke(nameof(GoRest), Random.Range(10, 20));
    }

    void GoUseFastTravelMachine()
    {
        Beliefs.ModifyState("wantToWork", 0);
        Invoke(nameof(GoUseFastTravelMachine), Random.Range(60, 80));
    }

    void GetHungry()
    {
        Beliefs.ModifyState("hungry", 0);
        Invoke(nameof(GetHungry), Random.Range(10, 20));
    }

    void GoShop()
    {
        Beliefs.ModifyState("needToShop", 0);
        Invoke(nameof(GoShop), Random.Range(10, 20));
    }

    void GoRest()
    {
        Beliefs.ModifyState("needToRest", 0);
        Invoke(nameof(GoRest), Random.Range(10, 20));
    }
}
