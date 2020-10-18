using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Execute : Action_Base
{
    Character_Base target;
    public override void OnActionStart(Character_Base character)
    {
        base.OnActionStart(character);
        Caster.isImmortal = true;

        Character_Player player = Caster as Character_Player;
        target = player.executeTarget;
        if (player && target)
        {
            player.executeTarget.BeExecuted();
            player.executeTarget.FacePosition(player.transform.position);
        }
        else
        {
            OnActionEnd();
        }
    }
    protected override void Update()
    {
        base.Update();
        target.transform.position = Vector3.Lerp(target.transform.position, Caster.transform.position + Caster.GetForward() * 0.5f, 0.1f);
    }
    protected override void Effect()
    {
        Character_Player player = Caster as Character_Player;
        if(player && target)
        {
            target.attribute.TakeDamage(100, Caster.gameObject, false, false);
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
