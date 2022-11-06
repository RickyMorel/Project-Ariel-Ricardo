using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : GAgent
{
    public override void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("satiated", 1, false);
        Goals.Add(s1, 3);

        SubGoal s3 = new SubGoal("soldStuff", 1, false);
        Goals.Add(s3, 2);

        SubGoal s4 = new SubGoal("feelSafe", 1, false);
        Goals.Add(s4, 10);

        GoSellStuff();

        Invoke(nameof(GetHungry), Random.Range(20, 40));
    }

    void GetHungry()
    {
        Beliefs.ModifyState("hungry", 0);
        Invoke(nameof(GetHungry), Random.Range(20, 40));
    }

    void GoSellStuff()
    {
        Beliefs.ModifyState("wantToSell", 0);
        Invoke(nameof(GoSellStuff), Random.Range(10, 20));
    }
}
