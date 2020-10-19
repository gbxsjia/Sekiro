using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    protected Rigidbody2D rb;
    public Attributes attribute;

    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;
    [SerializeField]
    protected float jumpSpeed;
    protected bool onGround;

    [SerializeField]
    protected Transform foot;

    public int camp;

    public Action_Base currentAction;
    public GameObject[] attackActionPrefabs;
    public GameObject dodgeActionPrefab;
    public GameObject stunActionPrefab;
    public GameObject brokeActionPrefab;
    public GameObject DefendActionPrefab;
    public GameObject BeExecutedActionPrefab;
    public GameObject BeCounterStabPrefab;

    public bool IsLookingRight = true;
    public bool InputDirectionRight=true;
    public bool IsFacingRight=true;

    public bool isDefending;
    public bool isParrying;
    public bool isBroken;
    public bool isImmortal;
    public bool isDead;
    public bool isCrouch;
    public bool isInBush;
    public bool isStabing;
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Transform graphic;
    [SerializeField]
    protected Transform BodySlot;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attribute = GetComponent<Attributes>();
        attribute.camp = camp;
        attribute.StanceBreakEvent += Broke;
        attribute.TakeDamageEvent += OnTakeDamage;
        attribute.DeathEvent += OnDeath;
    }
 
    private void OnDeath()
    {
        isDead = true;
        isBroken = false;
        attribute.isDead = true;
        GameObject g = EffectManager.instance.CreateEffectByIndex(transform.position + Vector3.up * 0.5f, 3, 3);
        g.transform.SetParent(BodySlot);
        Destroy(gameObject,3);
    }

    private void OnTakeDamage(ref float damage, GameObject damageSource, bool canBlock,bool canParry)
    {
        if (canBlock)
        {
            if (isImmortal)
            {
                damage = 0;
            }
            else if (isParrying)
            {
                FacePosition(damageSource.transform.position);
                damage = 0;
                if (canParry)
                {
                    Character_Base sourceCharacter= damageSource.GetComponent<Character_Base>();
                    sourceCharacter.GetParried();
                    sourceCharacter.AddForce(GetForward());                 
                }
                EffectManager.instance.CreateEffectByIndex(transform.position + GetForward() * 0.6f + Vector3.up * 0.5f, 4, 1);
            }
            else if (isDefending)
            {
                FacePosition(damageSource.transform.position);
                damage *= 0.3f;
                attribute.ReduceStance(damage * 1f);
                AddForce(GetForward() * -1f);
                EffectManager.instance.CreateEffectByIndex(transform.position + GetForward() * 0.6f + Vector3.up * 0.5f, 0, 1);
            }
            else
            {
                attribute.ReduceStance(damage * 1f);
                StartAction(stunActionPrefab);
                EffectManager.instance.CreateEffectByIndex(transform.position + Vector3.up * 0.5f, 2, 1, IsFacingRight);
            }
        }
        else
        {
            attribute.ReduceStance(damage * 0.3f);
            AddForce(GetForward() * -1f);
            EffectManager.instance.CreateEffectByIndex(transform.position +  Vector3.up * 0.5f, 2, 1, IsFacingRight);
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
                    if (isCrouch)
                    {
                        animator.Play("CrouchWalk");
                    }
                    else
                    {
                        animator.Play("Run");
                    }
                }
                else
                {
                    if (isCrouch)
                    {
                        animator.Play("CrouchIdle");
                    }
                    else
                    {
                        animator.Play("Idle");
                    }
                  
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
    protected void TurnBody()
    {
        if (IsFacingRight != InputDirectionRight)
        {
            IsFacingRight = InputDirectionRight;
            Vector3 scale = graphic.localScale;
            scale.x *= -1;
            graphic.localScale = scale;
        }
    }
    public void FacePosition(Vector3 position)
    {
        bool isRight = position.x > transform.position.x;
        if (IsFacingRight != isRight)
        {
            IsFacingRight = isRight;
            InputDirectionRight = isRight;
            IsLookingRight = isRight;
            Vector3 scale = graphic.localScale;
            scale.x *= -1;
            graphic.localScale = scale;
        }
    }
    public Vector3 GetForward()
    {
        if (IsFacingRight)
        {
            return Vector3.right;
        }
        else
        {
            return Vector3.left;
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
        return !isDead && !currentAction;
    }
    public bool CanAction()
    {
        return !isDead && !isBroken;
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
                if (isSprint && onGround)
                {
                    direction *= runSpeed;
                    isCrouch = false;
                }
                else
                {
                    direction *= walkSpeed;
                }
                SetVelocity(direction, false); 
            }
            else
            {
                SetVelocity(Vector3.zero, false);
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
        if ((CanAction() || priority > 20) && (currentAction == null || priority > currentAction.priority))
        {
            GameObject g = Instantiate(actionPrefab);
            Action_Base action = g.GetComponent<Action_Base>();
            if (currentAction != null)
            {
                currentAction.OnActionEnd();
            }
            currentAction = action;
            action.OnActionStart(this);

            string an = g.GetComponent<Action_Base>().AnimationName;
            if (an != "")
            {
                animator.Play(an);
            }

            return true;
        }
        return false;
    }
    public void PlayAnimation(string name)
    {
        animator.Play(name,0,0);
    }
    public void ActionEnd(Action_Base action)
    {
        currentAction = null;
        AnimtionUpdate();
    }
    public virtual void Attack()
    {
        GameObject attackPrefab = attackActionPrefabs[Random.Range(0, attackActionPrefabs.Length)];       
        StartAction(attackPrefab);
    }
    public void Defend()
    {
        StartAction(DefendActionPrefab);
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
    public virtual void Dodge()
    {
        if ( onGround)
        {
            TurnBody();
            StartAction(dodgeActionPrefab);
        }        
    }
    public void Stun()
    {
        StartAction(stunActionPrefab);
    }
    public void Broke()
    {
        isBroken = true;
        StartAction(brokeActionPrefab);

    }
    public void BeExecuted()
    {
        StartAction(BeExecutedActionPrefab);
    }
    public void BeCounterStab()
    {
        StartAction(BeCounterStabPrefab);
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
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force,ForceMode2D.Impulse);
    }
    public void ClearSpeed()
    {
        rb.velocity = Vector3.zero;
    }
    public void Crouch()
    {
        if (CanMove())
        {
            isCrouch = !isCrouch;
        }
    }
}