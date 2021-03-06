﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attribute : MonoBehaviour
{
    public Image HealthBar;
    public Image HealthDamageBar;
    public Image StanceBar;

    public Attributes attributes;

    protected virtual void Start()
    {
        attributes = GetComponentInParent<Attributes>();
        attributes.UIChangeEvent += OnUIChange;
    }

    protected void OnUIChange()
    {
        HealthDamageBar.fillAmount = HealthBar.fillAmount;
        HealthBar.fillAmount = attributes.HealthCurrent / attributes.HealthMax;
        StanceBar.fillAmount = attributes.StanceCurrent / attributes.StanceMax;
    }

}
