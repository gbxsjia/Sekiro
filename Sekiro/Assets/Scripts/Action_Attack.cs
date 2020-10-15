using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Attack : Action_Base
{
    public GameObject StartEffect;
    public GameObject HitEffect;
    public int Damage;
    protected override void Effect()
    {
        base.Effect();
        GameObject g = Instantiate(HitEffect, Caster.transform.position, Quaternion.identity);
        Vector3 scale = Vector3.one;
        scale.x = direction;
        g.transform.localScale = scale;
        Destroy(g, 0.1f);
    }
    public override void OnActionStart(Character_Base character)
    {
        base.OnActionStart(character);
        GameObject g = Instantiate(StartEffect, Caster.transform.position, Quaternion.identity);
        Vector3 scale = Vector3.one;
        scale.x = direction;
        g.transform.localScale = scale;
        Destroy(g, prepareTime);
    }
}
