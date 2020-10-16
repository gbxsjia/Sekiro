using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public GameObject owner;
    public int ownerCamp;
    public float damage;
    public bool canBlock;
    public bool canParry;

    public void InitHitBox(GameObject _owner,float _damage,int _ownerCamp,bool _canBlock,bool _canParry)
    {
        owner = _owner;
        ownerCamp = _ownerCamp;
        damage = _damage;
        canBlock = _canBlock;
        canParry = _canParry;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attributes attributes = collision.GetComponent<Attributes>();
        if (attributes && attributes.camp!=ownerCamp)
        {
            attributes.TakeDamage(damage, owner,canBlock, canParry);
        }
    }
}
