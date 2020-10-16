using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    Character_Base character;
    private void Awake()
    {
        character = GetComponent<Character_Base>();
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
    }
}
