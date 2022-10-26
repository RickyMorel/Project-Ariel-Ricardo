using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : GAgent
{
    public override void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("satiated", 1, false);
        Goals.Add(s1, 1);

        Invoke(nameof(GetHungry), Random.Range(10, 20));
    }

    void GetHungry()
    {
        Beliefs.ModifyState("hungry", 0);
        Invoke(nameof(GetHungry), Random.Range(10, 20));
    }
}
