using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float HealthMax;
    public float StanceMax;

    public float HealthCurrent;
    public float StanceCurrent;

    public delegate void DamageDelegate(ref float damage, GameObject damageSource,bool canBlock, bool canParry);
    public event DamageDelegate TakeDamageEvent;
    public event System.Action DeathEvent;
    public event System.Action StanceBreakEvent;
    public event System.Action UIChangeEvent;

    public int camp;
    public bool isDead;
    private void Start()
    {
        HealthCurrent = HealthMax;
        StanceCurrent = 0;
        if (UIChangeEvent != null)
        {
            UIChangeEvent();
        }
    }
    public void TakeDamage(float amount, GameObject source, bool canBlock=true, bool canParry=true)
    {
        if (isDead)
        {
            return;
        }

        float healthDamage=amount;
        if (TakeDamageEvent != null)
        {
            TakeDamageEvent(ref healthDamage, source, canBlock, canParry);
        }

        HealthCurrent -= healthDamage;
        

        if (HealthCurrent <= 0)
        {
            HealthCurrent = 0;
            if (DeathEvent != null)
            {
                DeathEvent();
            }
        }

        if (UIChangeEvent != null)
        {
            UIChangeEvent();
        }        
    }
    public void ReduceStance(float amount)
    {
        StanceCurrent += amount;
        if (StanceCurrent >= StanceMax)
        {
            StanceCurrent = StanceMax;
            if (StanceBreakEvent != null)
            {
                StanceBreakEvent();
            }
        }
        if (UIChangeEvent != null)
        {
            UIChangeEvent();
        }
    }

    public void RecoverAllStance()
    {
        StanceCurrent = 0;
        if (UIChangeEvent != null)
        {
            UIChangeEvent();
        }
    }
    private void Update()
    {

    }
}
