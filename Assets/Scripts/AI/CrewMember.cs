using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : GAgent
{
    public override void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("satiated", 1, false);
        Goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("satisfyAdmire", 1, false);
        Goals.Add(s2, 1);

        SubGoal s3 = new SubGoal("satisfyShopping", 1, false);
        Goals.Add(s3, 2);

        SubGoal s4 = new SubGoal("feelSafe", 1, false);
        Goals.Add(s4, 10);

        Invoke(nameof(GetHungry), Random.Range(10, 20));
        Invoke(nameof(GoAdmireObject), Random.Range(10, 20));
        Invoke(nameof(GoShop), Random.Range(10, 20));
    }

    void GetHungry()
    {
        Beliefs.ModifyState("hungry", 0);
        Invoke(nameof(GetHungry), Random.Range(10, 20));
    }

    void GoAdmireObject()
    {
        Beliefs.ModifyState("wantToAdmire", 0);
        Invoke(nameof(GoAdmireObject), Random.Range(10, 20));
    }

    void GoShop()
    {
        Beliefs.ModifyState("needToShop", 0);
        Invoke(nameof(GoShop), Random.Range(10, 20));
    }
}
