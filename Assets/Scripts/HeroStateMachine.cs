using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    private float currCooldown = 0f;
    private float maxCooldown = 5f;

    public Image ProgressFG;
    public GameObject Selector;
     
    // Start is called before the first frame update
    private void Start()
    {
        currCooldown = Random.Range(0, 2.5f);
        Selector.SetActive(false);
        currentState = TurnState.Processing;
        battleStateMachine = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
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
            
            case (TurnState.PerformAction): break;
            case (TurnState.Dead): break;
            default: break;
        }
    }

    void updateProgressBar()
    {
        currCooldown += Time.deltaTime;
        var ratio = currCooldown / maxCooldown;
        
        ProgressFG.transform.localScale = new Vector3(Mathf.Clamp(ratio, 0f, 1f),
            ProgressFG.transform.localScale.y, ProgressFG.transform.localScale.z);

        if (currCooldown >= maxCooldown)
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

}
