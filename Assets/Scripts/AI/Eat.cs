using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPeform()
    {
        Beliefs.RemoveState("hungry");
        return true;
    }
}
