using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Character_Base character;
    private void Awake()
    {
        character = GetComponent<Character_Base>();
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
        if (Input.GetKeyDown(KeyCode.J))
        {
            character.Attack();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            character.Dodge();
        }
    }
}
