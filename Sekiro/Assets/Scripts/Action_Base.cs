﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Base : MonoBehaviour
{
    public Character_Base Caster;
    public float prepareTime;
    public float endTime;
    public int priority;
    public string AnimationName;

    protected float timer;
    private bool casted;

    protected int direction;

    public bool useActionProgressBar=false;
    public bool isStab;
    public bool isBottomAttack;
    public float Impluse;
    public virtual void OnActionStart(Character_Base character)
    {
        Caster = character;
        direction = character.InputDirectionRight ? 1 : -1;
        if (isStab || isBottomAttack)
        {
            PlayerInput.instance.GetComponent<Character_Player>().ShowDangerSign();
        }
        Caster.isStabing = isStab;
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
        if (Impluse > 0)
        {
            Caster.AddForce(Caster.GetForward() * Impluse);
        }
    }
   protected virtual void TimeUp()
    {        
        OnActionEnd();
    }
    public virtual void OnActionEnd()
    {
        if (isStab || isBottomAttack)
        {
            PlayerInput.instance.GetComponent<Character_Player>().CancelDangerSign();
        }
        if (Caster && Caster.gameObject)
        {
            Caster.ActionEnd(this);
        }
        Caster.isStabing = isStab;
        Destroy(gameObject);
    }
    public float GetProgress()
    {
        return timer / prepareTime;
    }
}
