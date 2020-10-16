using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Defend : Action_Base
{
    public float parryDuration;
    protected override void Effect()
    {
        base.Effect();
    }
    protected override void TimeUp()
    {
    }
    protected override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= parryDuration)
        {
            Caster.isParrying = false;
        }
    }
    public override void OnActionStart(Character_Base character)
    {
        base.OnActionStart(character);
        Caster.isDefending = true ;
        if (parryDuration > 0)
        {       
            Caster.isParrying = true;
        }
    }
    public override void OnActionEnd()
    {
        Caster.isDefending = false;
        Caster.isParrying = false;
        base.OnActionEnd();
    }
}
