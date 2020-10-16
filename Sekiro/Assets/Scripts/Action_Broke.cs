using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Broke : Action_Base
{
    protected override void Effect()
    {
        base.Effect();
        Caster.RecoverBroke();
    }
}
