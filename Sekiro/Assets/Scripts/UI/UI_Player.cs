using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : UI_Attribute
{
    public static UI_Player instance;
    public Character_Player player;
    public SpriteRenderer DangerSign;
    private void Awake()
    {
        instance = this;
    }
    protected override void Start()
    {
        player = PlayerInput.instance.GetComponent<Character_Player>();
        attributes = PlayerInput.instance.GetComponent<Attributes>();
        attributes.UIChangeEvent += OnUIChange;
        player.DangerSignEvent += OnDangerSign;
    }

    private void OnDangerSign(bool obj)
    {
        DangerSign.enabled = obj;
    }
}
