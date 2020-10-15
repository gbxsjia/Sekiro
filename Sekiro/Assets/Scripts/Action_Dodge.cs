using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Action_Dodge : Action_Base
{
    public float DodgeSpeed;
    public float DodgeDuration;
    public AnimationCurve curve;
    
    public override void OnActionStart(Character_Base character)
    {
        base.OnActionStart(character);
    
        StartCoroutine(DodgeProcess());
    }
    protected override void Effect()
    {
        base.Effect();
    }
    private IEnumerator DodgeProcess()
    {
        float timer=0;
        while (timer <= DodgeDuration)
        {
            Caster.SetVelocity(DodgeSpeed * curve.Evaluate(timer / (DodgeDuration)) * Vector3.right * direction, false);
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(endTime);
        OnActionEnd();
    }
    protected override void TimeUp()
    {
    }
}
