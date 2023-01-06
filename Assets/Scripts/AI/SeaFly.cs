using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFly : GAgent
{
    public override void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("destroyShip", 1, false);
        Goals.Add(s1, 1);

        SubGoal s2 = new SubGoal("hasFlewAway", 1, false);
        Goals.Add(s2, 3);
    }
}

