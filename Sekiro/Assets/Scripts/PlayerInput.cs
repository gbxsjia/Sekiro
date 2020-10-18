using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    Character_Player character;

    private Vector3 MousePosition;
    private void Awake()
    {
        character = GetComponent<Character_Player>();
        instance = this;
    }
    void Update()
    {
        Vector3 direction = Vector3.zero;
        direction.Set(Input.GetAxisRaw("Horizontal"), 0, 0);

        character.Move(direction, Input.GetKey(KeyCode.LeftShift));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.Jump();
        }
        if (Input.GetMouseButtonDown(0))
        {
            character.Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            character.Defend();
        }
        if (Input.GetMouseButtonUp(1))
        {
            character.CancelDefend();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            character.Dodge();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            character.UseHook();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            character.Crouch();
        }
        GetMousePosition();
        FindHook();
    }
    private void GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        MousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        character.IsLookingRight = MousePosition.x > character.transform.position.x;
    }
    private void FindHook()
    {
        float dis = 99999;
        foreach (HookTarget hook in HookTarget.Hooks)
        {
            if (hook.Active)
            {
                float newDis = Vector3.Distance(hook.transform.position, MousePosition);
                if (newDis < dis)
                {
                    dis = newDis;
                    character.HooKTarget = hook.gameObject;
                }
            }
        }

    }
}
