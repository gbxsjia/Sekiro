using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Player : Character_Base
{
    [Header("Execute")]
    public GameObject executePrefab;
    public float executeDistance;
    public Character_Base executeTarget;

    public GameObject HooKTarget;
    public GameObject HookActionPrefab;

    public int DangerSignAmount;
    public event System.Action<bool> DangerSignEvent;
    public override void Attack()
    {
        foreach (AIController c in InGameManager.instance.allEnemies)
        {
            if (c.canBeExecute)
            {
                executeTarget = c.character;
            }
        }
        if (executeTarget)
        {
            StartAction(executePrefab); 
        }
        else
        {
            base.Attack();
        }
       
    }
    public void UseHook()
    {
        StartAction(HookActionPrefab);
    }
    public void ShowDangerSign()
    {
        DangerSignAmount++;
        if (DangerSignAmount == 1)
        {
            if (DangerSignEvent != null)
            {
                DangerSignEvent(true);
            }
        }
    }
    public void CancelDangerSign()
    {
        DangerSignAmount--;
        if (DangerSignAmount == 0)
        {
            if (DangerSignEvent != null)
            {
                DangerSignEvent(false);
            }
        }
    }
}
