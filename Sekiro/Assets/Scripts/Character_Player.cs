using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Player : Character_Base
{
    [Header("Execute")]
    public GameObject executePrefab;
    public float executeDistance;
    public Character_Base executeTarget;
    public override void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, executeDistance);
        
        foreach (Collider2D c in colliders)
        {
            executeTarget = c.GetComponent<Character_Base>();
            if (executeTarget && executeTarget.isBroken && executeTarget.camp!=camp)
            {
                break;
            }
            else
            {
                executeTarget = null;
            }
        }
        if (executeTarget)
        {
            if (StartAction(executePrefab))
            {
                animator.Play("Attack_1");
            }
        }
        else
        {
            base.Attack();
        }
       
    }
}
