using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public BaseEnemy enemy;

    public BattleStateMachine battleStateMachine; 
    
    public enum TurnState
    {
        Processing, 
        MakeAction, 
        Waiting,
        Action, 
        Dead
    }

    public TurnState currentState;

    private float currCooldown = 0f;
    private float maxCooldown = 5f;

    private Vector3 startPosition; 
    
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
            case (TurnState.MakeAction): 
                makeNewAction();
                currentState = TurnState.Waiting;
                break;
            case (TurnState.Waiting): break;

            case (TurnState.Action): break;
            case (TurnState.Dead): break;
            default: break;
        }
    }

    void updateProgressBar() 
    {
        currCooldown += Time.deltaTime;
        
        if (currCooldown >= maxCooldown)
        {
            currentState = TurnState.MakeAction;
        }
    }

    void makeNewAction()
    {
        BattleAction action = new BattleAction();
        action.attackerGameObject = this.gameObject;
        action.defenderGameObject = battleStateMachine.heroes[Random.Range(0, battleStateMachine.heroes.Count)];
        battleStateMachine.AddAction(action);
    }
}
