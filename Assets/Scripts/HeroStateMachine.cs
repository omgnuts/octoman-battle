using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class HeroStateMachine : MonoBehaviour
{
    public BaseHero hero;
    
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
    private float maxCooldown = 5f;

    public Image ProgressFG;
    public GameObject Selector;
     
    private Vector3 startPosition;
    // private readonly object actionLock = new Object();
    private volatile bool actionLock;
    public GameObject enemyToAttack;
    const float animSpeed = 5f; 
    
    // Start is called before the first frame update
    private void Start()
    {
        currentCooldown = Random.Range(0, 2.5f);
        Selector.SetActive(false);
        
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
                battleStateMachine.heroesToManage.Add(this.gameObject);
                currentState = TurnState.Waiting;
                break;
            case (TurnState.Waiting):
                // idlestate    
                break;
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
        var ratio = currentCooldown / maxCooldown;
        
        ProgressFG.transform.localScale = new Vector3(Mathf.Clamp(ratio, 0f, 1f),
            ProgressFG.transform.localScale.y, ProgressFG.transform.localScale.z);

        if (currentCooldown >= maxCooldown)
        {
            currentState = TurnState.ComputeAction;
        }
    }

    public void MakeNewAction(GameObject enemySelected)
    {
        BattleAction action = new BattleAction
        {
            attackerGameObject = gameObject,
            defenderGameObject = enemySelected,
            attackerType = BattleAction.ToonType.Hero
        };
        battleStateMachine.AddAction(action);
    }

    public IEnumerator TimeForAction()
    {
        if (actionLock) yield break;
        actionLock = true;
    
        // animate enemy near hero to attack
        Vector3 enemyPosition = enemyToAttack.transform.position.WithOffset(1.5f);
        while (MoveToHero(enemyPosition)) yield return null;

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
