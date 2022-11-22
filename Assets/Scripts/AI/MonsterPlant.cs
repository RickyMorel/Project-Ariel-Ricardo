using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlant : GAgent
{
    public override void Start()
    {
        base.Start();

        SubGoal s4 = new SubGoal("feelSafe", 1, false);
        Goals.Add(s4, 10);
    }
}
