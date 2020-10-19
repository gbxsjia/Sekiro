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
    public GameObject CounterStabPrefab;

    public int DangerSignAmount;
    public event System.Action<bool> DangerSignEvent;
    public override void Attack()
    {
        foreach (AIController c in InGameManager.instance.allEnemies)
        {
            if (c && c.canBeExecute && Vector3.Distance(c.transform.position, transform.position) <= 2f && !c.character.isDead)
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
    public override void Dodge()
    {
        if (onGround)
        {
            TurnBody();

            bool hasTarget = false;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position +Vector3.up*0.5f, GetForward(), 4f);
            foreach (RaycastHit2D hit in hits)
            {
                Character_Base c = hit.collider.GetComponent<Character_Base>();        
                if (c && c.isStabing && !c.isDead && c.camp != camp)
                {
                    hasTarget = true;
                    break;
                }
            }
            if (hasTarget)
            {
                StartAction(CounterStabPrefab);
            }
            else
            {            
                StartAction(dodgeActionPrefab);
            }
        }
    }
}
