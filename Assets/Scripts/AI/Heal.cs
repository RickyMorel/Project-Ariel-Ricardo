using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : GAction
{
    public override bool PrePerform()
    {
        Target = GWorld.Instance.GetQueue(GWorld.HEAL_POINTS).RemoveResource();

        if (Target == null) { return false; }

        Inventory.AddItem(Target);

        GWorld.Instance.GetWorld().ModifyState(GWorld.FREE_HEAL_POINTS, -1);

        Debug.Log("PrePerform Heal");

        return true;
    }

    public override bool PostPeform()
    {
        Debug.Log("PostPeform Heal");
        GWorld.Instance.GetQueue(GWorld.HEAL_POINTS).AddResource(Target);

        Inventory.RemoveItem(Target);

        GWorld.Instance.GetWorld().ModifyState(GWorld.FREE_HEAL_POINTS, 1);

        Beliefs.RemoveState("hurt");

        return true;
    }
}
