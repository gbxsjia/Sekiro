using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_ComboAttack : Action_Base
{
    public string[] AnimationNames;
    public GameObject[] HitBox;
    public float[] Prepares;
    public float[] Ends;
    public int Damage;
    public bool canBlock = true;
    public bool canParry = true;

    public float duration = 0.1f;

    private int attackIndex;
    protected override void Effect()
    {
        base.Effect();      
    }
    public override void OnActionStart(Character_Base character)
    {
        base.OnActionStart(character);
        StartCoroutine(AttackProcess());
    }
    private void SingleAttack()
    {
        print(attackIndex);
        direction = Caster.InputDirectionRight ? 1 : -1;
        GameObject g = Instantiate(HitBox[attackIndex], Caster.transform.position, Quaternion.identity);
        Vector3 scale = Vector3.one;
        scale.x = direction;
        g.transform.localScale = scale;
        
        HitBox hb = g.GetComponent<HitBox>();
        hb.InitHitBox(Caster.gameObject, Damage, Caster.camp, canBlock, canParry);
        Destroy(g, duration);
    }
    protected override void TimeUp()
    {
    }
    private IEnumerator AttackProcess()
    {
        while (attackIndex < HitBox.Length)
        {
            Caster.FacePosition(PlayerInput.instance.transform.position);
            Caster.PlayAnimation(AnimationNames[attackIndex]);
            yield return new WaitForSeconds(Prepares[attackIndex]);
            SingleAttack();         
            yield return new WaitForSeconds(Ends[attackIndex]);
            attackIndex++;        
        }
        OnActionEnd();
    }
}
