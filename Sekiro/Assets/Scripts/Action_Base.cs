using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Base : MonoBehaviour
{
    public Character_Base Caster;
    public float prepareTime;
    public float endTime;
    public int priority;

    protected float timer;
    private bool casted;

    protected int direction;

    public bool useActionProgressBar=false;
    public virtual void OnActionStart(Character_Base character)
    {
        Caster = character;
        direction = character.InputDirectionRight ? 1 : -1;
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
    public virtual void OnActionEnd()
    {
        Caster.ActionEnd(this);
        Destroy(gameObject);
    }
    public float GetProgress()
    {
        return timer / prepareTime;
    }
}
