using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Character_Base character;

    public AIState state;

    public float attackRange;

    private float AlertValue;
    [SerializeField]
    private float AlertRange;
    [SerializeField]
    private float HearRange = 1.5f;
    [SerializeField]
    private float AlertIncreaseSpeed;

    private Vector3 direction;
    private float distance;
    public bool canBeExecute;

    private Character_Player player;
    [SerializeField]
    private LayerMask SightLayer;
    private void Awake()
    {
        character = GetComponent<Character_Base>();
    }
    private void Start()
    {
        player = PlayerInput.instance.GetComponent<Character_Player>();
        InGameManager.instance.RegistEnemy(this);
        ChangeState(AIState.Idle);
    }
    private void Update()
    {
        direction = player.transform.position - transform.position;
        distance = direction.magnitude;
        UpdateSight();
        UpdateExecuteState();
    }
    public void ChangeState(AIState nextState)
    {
        if (BehaviourProcess != null)
        {
            StopCoroutine(BehaviourProcess);
        }
        switch (nextState)
        {
            case AIState.Idle:
                BehaviourProcess = StartCoroutine(IdleBehaviour());
                break;
            case AIState.Alert:
                BehaviourProcess = StartCoroutine(AlertBehaviour());
                break;
            case AIState.Chasing:
                BehaviourProcess = StartCoroutine(ChaseBehaviour());
                break;
            case AIState.Attack:
                BehaviourProcess = StartCoroutine(AttackBehaviour());
                break;
            default:
                break;
        }
        state = nextState;
    }
    Coroutine BehaviourProcess;

    private IEnumerator IdleBehaviour()
    {
        while (true)
        {
            if (AlertValue >= 1)
            {
                ChangeState(AIState.Alert);
            }
            yield return null;
        } 
    }
    private IEnumerator AlertBehaviour()
    {
        float time = Random.Range(0.3f, 0.7f);
        yield return new WaitForSeconds(time);
        ChangeState(AIState.Chasing);
    }
    private IEnumerator ChaseBehaviour()
    {
        float distance = Vector3.Distance(transform.position, PlayerInput.instance.transform.position);
        while (distance >= attackRange)
        {
            character.Move(PlayerInput.instance.transform.position - transform.position, true);
            distance = Vector3.Distance(transform.position, PlayerInput.instance.transform.position);
            yield return null;
        }
        ChangeState(AIState.Attack);
    }
    private IEnumerator AttackBehaviour()
    {
        float timer = 3;
        while (timer > 0)
        {
            character.FacePosition(player.transform.position);
            timer -= Time.deltaTime;
            character.Attack();
            yield return new WaitForSeconds(0.5f);
        }
        ChangeState(AIState.Alert);
    }

    private void UpdateSight()
    {     
        if (distance <= AlertRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, SightLayer);
            if (!hit)
            {
                bool sameDirection = character.IsFacingRight == player.transform.position.x > transform.position.x;
                SetAlertValue(distance, sameDirection);
            }
            else
            {
                SetAlertValue(distance, false);
            }
        }
        else
        {
            SetAlertValue(distance, false);
        }
    }
    public float GetAlertPercent()
    {
        return AlertValue / 1;
    }
    private void SetAlertValue(float distance,bool inSight)
    {
        if (inSight)
        {
            float n = 1 - distance / AlertRange;
            if (player.isCrouch)
            {
                if (player.isInBush && distance > 1)
                {
                    n = 0;        
                }
                else
                {
                    n /= 2;
                }
            }
            AlertValue += AlertIncreaseSpeed * n * Time.deltaTime;
            if (AlertValue > 1)
            {
                AlertValue = 1;
            }
        }
        else
        {
            if (distance <= HearRange)
            {
                AlertValue += AlertIncreaseSpeed * 0.5f * Time.deltaTime;
                if (AlertValue > 1)
                {
                    AlertValue = 1;
                }
            }
            else if (distance > AlertRange)
            {
                AlertValue -= AlertIncreaseSpeed * Time.deltaTime;
                if (AlertValue <= 0)
                {
                    AlertValue = 0;
                }
            }
        }
    }
    private void UpdateExecuteState()
    {
        bool isInRange = distance < 1.5f;
        bool isValidTarget = character && !character.isDead && character.camp != 0;
        bool canAssassin = state == AIState.Idle && character.IsFacingRight != transform.position.x < player.transform.position.x;
        if((canBeExecute != (canAssassin || character.isBroken)) && isInRange)
        {
            canBeExecute = !canBeExecute;
        }

    }
}
public enum AIState 
{
    Idle,
    Alert,
    Chasing,
    Attack,
    Dead
}
