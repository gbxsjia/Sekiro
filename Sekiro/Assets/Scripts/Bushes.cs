using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character_Base character = collision.GetComponent<Character_Base>();
        if (character)
        {
            character.isInBush = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Character_Base character = collision.GetComponent<Character_Base>();
        if (character)
        {
            character.isInBush = false;
        }
    }
}
