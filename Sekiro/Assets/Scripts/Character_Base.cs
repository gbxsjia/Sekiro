using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    private Rigidbody2D rb;
    public Attributes attribute;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float jumpSpeed;
    private bool onGround;

    [SerializeField]
    private Transform foot;

    public int camp;

    public Action_Base currentAction;
    public GameObject[] attackActionPrefabs;
    public GameObject dodgeActionPrefab;
    public GameObject stunActionPrefab;
    public GameObject brokeActionPrefab;
    public GameObject DefendActionPrefab;

    public bool InputDirectionRight=true;
    public bool IsFacingRight=true;

    public bool isDefending;
    public bool isParrying;
    public bool isBroken;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform graphic;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attribute = GetComponent<Attributes>();
        attribute.camp = camp;
        attribute.StanceBreakEvent += Broke;
        attribute.TakeDamageEvent += OnTakeDamage;
    }

    private void OnTakeDamage(ref float damage, GameObject damageSource, bool canBlock,bool canParry)
    {
        if (canBlock)
        {
            if (isParrying)
            {
                damage = 0;
                if (canParry)
                {
                    Character_Base sourceCharacter= damageSource.GetComponent<Character_Base>();
                    sourceCharacter.GetParried();
                }
            }
            else if (isDefending)
            {
                damage *= 0.3f;
                attribute.ReduceStance(damage * 0.7f);
            }
            else
            {
                attribute.ReduceStance(damage * 0.3f);
            }
        }
        else
        {
            attribute.ReduceStance(damage * 0.3f);
        }
    }

    private void Start()
    {

    }
    private void Update()
    {
        FootTrace();
        if (CanMove())
        {
            TurnBody();
        }
        AnimtionUpdate();
    }
    public void AnimtionUpdate()
    {
        if (currentAction == null)
        {
            if (onGround)
            {
                if (rb.velocity.magnitude >= 0.3f)
                {
                    animator.Play("Run");
                }
                else
                {
                    animator.Play("Idle");
                }
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    animator.Play("Jump");
                }
                else
                {
                    animator.Play("Fall");
                }
            }
        }
    }
    private void TurnBody()
    {
        if (IsFacingRight != InputDirectionRight)
        {
            IsFacingRight = InputDirectionRight;
            Vector3 scale = graphic.localScale;
            scale.x *= -1;
            graphic.localScale = scale;
        }
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
        return !currentAction;
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
        direction.Normalize();
        if (direction != Vector3.zero)
        {
            InputDirectionRight = direction.x > 0;
        }
     
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
    public bool StartAction(GameObject actionPrefab)
    {
        int priority = actionPrefab.GetComponent<Action_Base>().priority;
        if (currentAction == null || priority > currentAction.priority)
        {
            GameObject g = Instantiate(actionPrefab);
            Action_Base action = g.GetComponent<Action_Base>();
            if (currentAction != null)
            {
                currentAction.OnActionEnd();
            }
            currentAction = action;
            action.OnActionStart(this);
            return true;
        }
        return false;
    }
    public void ActionEnd(Action_Base action)
    {
        currentAction = null;
        animator.Play("Idle");
    }
    public void Attack()
    {
        GameObject attackPrefab = attackActionPrefabs[Random.Range(0, attackActionPrefabs.Length)];
        if (StartAction(attackPrefab))
        {
            animator.Play("Attack_1");
        }       
    }
    public void Defend()
    {
        if (StartAction(DefendActionPrefab))
        {
            animator.Play("Defend");
        }
    }
    public void CancelDefend()
    {
        if(currentAction)
        {
            Action_Defend defend = currentAction as Action_Defend;
            if (defend)
            {
                defend.OnActionEnd();
            }
        }
    }
    public void Dodge()
    {
        if ( onGround)
        {
            TurnBody();
            if (StartAction(dodgeActionPrefab))
            {
                animator.Play("Run");
            }
        }        
    }
    public void Stun()
    {
        if (StartAction(stunActionPrefab))
        {
            animator.Play("TakeHit");
        }
    }
    public void Broke()
    {
        isBroken = true;
        if (StartAction(brokeActionPrefab))
        {
            animator.Play("Death");
        }
    }
    public void RecoverBroke()
    {
        isBroken = false;
        attribute.RecoverAllStance();
    }
    public void GetParried()
    {
        attribute.ReduceStance(20);
    }
    
}