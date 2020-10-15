using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Base : MonoBehaviour
{
    public Character_Base Caster;
    public float prepareTime;
    public float endTime;

    private float timer;
    private bool casted;

    protected int direction;
    public virtual void OnActionStart(Character_Base character)
    {
        Caster = character;
        direction = character.IsFacingRight ? 1 : -1;
    }
    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer > prepareTime && !casted)
        {
            casted = true;
            Effect();
        }
        if (timer > endTime + prepareTime)
        {
            TimeUp();
        }
    }
    protected virtual void Effect()
    {

    }
   protected virtual void TimeUp()
    {
        OnActionEnd();
    }
    public void OnActionEnd()
    {
        Caster.ActionEnd(this);
        Destroy(gameObject);
    }
}
