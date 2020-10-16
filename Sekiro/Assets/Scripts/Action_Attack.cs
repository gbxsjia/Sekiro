using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Attack : Action_Base
{
    public GameObject StartEffect;
    public GameObject HitBox;
    public int Damage;
    public bool canBlock=true;
    public bool canParry=true;
    protected override void Effect()
    {
        base.Effect();
        GameObject g = Instantiate(HitBox, Caster.transform.position, Quaternion.identity);
        Vector3 scale = Vector3.one;
        scale.x = direction;
        g.transform.localScale = scale;

        HitBox hb= g.GetComponent<HitBox>();
        hb.InitHitBox(Caster.gameObject, Damage, Caster.camp, canBlock, canParry);
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
