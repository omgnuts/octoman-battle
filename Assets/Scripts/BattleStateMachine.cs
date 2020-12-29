using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        Waiting, 
        PrepareAction, 
        PerformAction
    }

    [FormerlySerializedAs("battleStates")] public PerformAction battleState;

    public List<BattleAction> actions = new List<BattleAction>();
    public List<GameObject> heroes = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    
    // Start is called before the first frame update
    private void Start()
    {
        battleState = PerformAction.Waiting;
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        heroes.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        
    }

    // Update is called once per frame
    private void Update()
    {
        switch (battleState)
        {
            case PerformAction.Waiting:
                if (actions.Count > 0)
                    battleState = PerformAction.PrepareAction;
                break;
            case PerformAction.PrepareAction:
                BattleAction action = actions[0];
                
                GameObject performer = action.attackerGameObject;
                if (action.attackerType == BattleAction.ToonType.Enemy)
                {
                    EnemyStateMachine esm = performer.GetComponent<EnemyStateMachine>();
                    esm.heroToAttack = action.defenderGameObject;
                    esm.currentState = EnemyStateMachine.TurnState.PerformAction;
                }
                else // if hero
                {
                    
                } 
                
                
                
                break;
            case PerformAction.PerformAction: break;
            default: break;
        }
    }

    public void AddAction(BattleAction action)
    {
        actions.Add(action);
    }
}
