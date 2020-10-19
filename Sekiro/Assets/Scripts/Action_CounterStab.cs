using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_CounterStab : Action_Base
{
    Character_Base target;
    public override void OnActionStart(Character_Base character)
    {
        base.OnActionStart(character);
        Caster.isImmortal = true;

        RaycastHit2D[] hits = Physics2D.RaycastAll(Caster.transform.position + Vector3.up * 0.5f, Caster.GetForward(), 1.5f);
        foreach(RaycastHit2D hit in hits)
        {
            Character_Base c=hit.collider.GetComponent<Character_Base>();
            if(c && c.isStabing && !c.isDead && c.camp != character.camp)
            {
                target = c;
            }
        }
        if (target)
        {
            target.BeCounterStab();
        }
        else
        {
            OnActionEnd();
        }      
    }
    protected override void Update()
    {
        base.Update();
        target.transform.position = Vector3.Lerp(target.transform.position, Caster.transform.position + Caster.GetForward() * 1f, 0.1f);
    }
    protected override void Effect()
    {
        Character_Player player = Caster as Character_Player;
        if (player && target)
        {
            target.attribute.ReduceStance(25);
        }
        else
        {
            OnActionEnd();
        }
        EffectManager.instance.CreateEffectByIndex(Caster.transform.position + Caster.GetForward() * 0.6f, 1, 5);
    }
    public override void OnActionEnd()
    {
        base.OnActionEnd();
        Caster.isImmortal = false;
    }
}
