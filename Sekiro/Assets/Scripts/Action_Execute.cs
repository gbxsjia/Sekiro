using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Execute : Action_Base
{
    public override void OnActionStart(Character_Base character)
    {
        base.OnActionStart(character);
        Caster.isImmortal = true;

        Character_Player player = Caster as Character_Player;
        if (player && player.executeTarget)
        {
            player.executeTarget.BeExecuted();
        }
        else
        {
            OnActionEnd();
        }
    }
    protected override void Effect()
    {
        Character_Player player = Caster as Character_Player;
        if(player && player.executeTarget)
        {
            player.executeTarget.attribute.TakeDamage(100, Caster.gameObject, false, false);
        }
        else
        {
            OnActionEnd();
        }
    }
    public override void OnActionEnd()
    {
        base.OnActionEnd();
        Caster.isImmortal = false;
    }
}
