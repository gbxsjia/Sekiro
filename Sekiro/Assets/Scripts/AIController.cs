using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Character_Base character;

    public AIState state;

    public float attackRange;
 
    private void Awake()
    {
        character = GetComponent<Character_Base>();
        ChangeState(AIState.Alert);
    }
    private void Update()
    {
        
    }
    public void ChangeState(AIState nextState)
    {
        if (BehaviourProcess != null)
        {
            StopCoroutine(BehaviourProcess);
        }
        switch (nextState)
        {
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


    private IEnumerator AlertBehaviour()
    {
        float time = Random.Range(0.5f, 1.5f);
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
            timer -= Time.deltaTime;
            character.Attack();
            yield return null;
        }
        ChangeState(AIState.Alert);
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
