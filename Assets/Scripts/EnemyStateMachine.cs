using System.Collections;
using System.Collections.Generic;
using common;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public BaseEnemy enemy;

    public BattleStateMachine battleStateMachine; 
    
    public enum TurnState
    {
        Processing, 
        ComputeAction, 
        Waiting,
        PerformAction, 
        Dead
    }

    public TurnState currentState;

    private float currentCooldown = 0f;
    const float maxCooldown = 5f;

    private Vector3 startPosition;

    // private readonly object actionLock = new Object();
    private volatile bool actionLock;
    
    public GameObject heroToAttack;
    const float animSpeed = 5f; 
    
    // Start is called before the first frame update    
    private void Start()
    {
        currentState = TurnState.Processing;
        battleStateMachine = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case (TurnState.Processing):
                updateProgressBar();
                break;
            case (TurnState.ComputeAction): 
                makeNewAction();
                currentState = TurnState.Waiting;
                break;
            case (TurnState.Waiting): break;

            case (TurnState.PerformAction): 
                // TryLock.Execute(actionLock, () => StartCoroutine(TimeForAction()));
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.Dead): break;
            default: break;
        }
    }

    void updateProgressBar() 
    {
        currentCooldown += Time.deltaTime;
        
        if (currentCooldown >= maxCooldown)
        {
            currentState = TurnState.ComputeAction;
        }
    }

    void makeNewAction()
    {
        BattleAction action = new BattleAction
        {
            attackerGameObject = gameObject,
            defenderGameObject = battleStateMachine.heroes[Random.Range(0, battleStateMachine.heroes.Count)],
            attackerType = BattleAction.ToonType.Enemy
        };
        battleStateMachine.AddAction(action);
    }
    
    public IEnumerator TimeForAction()
    {
        if (actionLock) yield break;
        actionLock = true;
    
        // animate enemy near hero to attack
        Vector3 heroPosition = heroToAttack.transform.position.WithOffset(-1.5f);
        while (MoveToHero(heroPosition)) yield return null;

        // wait a bit
        yield return new WaitForSeconds(0.2f);

        // do damage & animation attack

        // animate back to start position
        while (MoveToStart(startPosition)) yield return null;

        // remove performer from the list
        battleStateMachine.actions.RemoveAt(0);
        
        // reset battle state machine to wait
        battleStateMachine.battleState = BattleStateMachine.PerformAction.Waiting;

        currentCooldown = 0f;
        currentState = TurnState.Processing;
        
        // end coroutine
        actionLock = false;
    }

    private bool MoveToHero(Vector3 destPosition)
    {
        return destPosition != (transform.position =
            Vector3.MoveTowards(transform.position, destPosition, animSpeed * Time.deltaTime));
    }

    private bool MoveToStart(Vector3 startPosition)
    {
        return startPosition != (transform.position =
            Vector3.MoveTowards(transform.position, startPosition, animSpeed * Time.deltaTime));
    }
}
