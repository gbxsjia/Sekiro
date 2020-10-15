using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float jumpSpeed;
    private bool onGround;

    [SerializeField]
    private Transform foot;

    public CharacterState state;

    public Action_Base currentAction;
    public GameObject[] attackActionPrefabs;
    public GameObject dodgeActionPrefab;

    public bool IsFacingRight
    {
        get { return isFacingRight; }
        set
        {
            isFacingRight = value;
        }
    }
    private bool isFacingRight=true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {

    }
    private void Update()
    {
        FootTrace();
    }
    private void FootTrace()
    {
        RaycastHit2D hit = Physics2D.Raycast(foot.position, Vector2.down, 0.1f);
        if (hit)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }
    public bool CanMove()
    {
        return state == CharacterState.Idle;
    }
    public bool CanAction()
    {
        return state == CharacterState.Idle;
    }
    public void SetVelocity(Vector3 velocity, bool IncludeY)
    {
        if (IncludeY)
        {
            rb.velocity = velocity;
        }
        else
        {
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }
    }
    public void Move(Vector3 direction, bool isSprint)
    {
        if (CanMove())
        {
            if (direction != Vector3.zero)
            {
                if (isSprint)
                {
                    direction *= runSpeed;
                }
                else
                {
                    direction *= walkSpeed;
                }
                SetVelocity(direction, false);
                isFacingRight = direction.x > 0;
            }
            else
            {
                SetVelocity(Vector3.zero,false);
            }
        }
    }
    public void Jump()
    {
        if (CanMove() && onGround)
        {
            Vector3 v = rb.velocity;
            v.y = jumpSpeed;
            rb.velocity = v;
        }
    }
    public void StartAction(GameObject actionPrefab, bool forceAction)
    {
        if (CanAction() || forceAction)
        {
            GameObject g = Instantiate(actionPrefab);
            Action_Base action = g.GetComponent<Action_Base>();
            if (currentAction != null)
            {
                currentAction.OnActionEnd();
            }
            action.OnActionStart(this);
            state = CharacterState.Action;
        } 
    }
    public void ActionEnd(Action_Base action)
    {
        state = CharacterState.Idle;
    }
    public void Attack()
    {
        if (CanAction())
        {
            GameObject attackPrefab = attackActionPrefabs[Random.Range(0, attackActionPrefabs.Length)];
            StartAction(attackPrefab, false);
        }
    }
    public void Dodge()
    {
        if (CanAction() && CanMove())
        {
            StartAction(dodgeActionPrefab, false);
        }        
    }
}
public enum CharacterState
{
    Idle,
    Action,
    Broken
}