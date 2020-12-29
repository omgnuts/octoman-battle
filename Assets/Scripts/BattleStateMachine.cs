using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        Waiting, 
        PrepareAction, 
        PerformAction
    }

    public PerformAction battleState;

    public List<BattleAction> actions = new List<BattleAction>();
    public List<GameObject> heroesAlive = new List<GameObject>();
    public List<GameObject> enemiesAlive = new List<GameObject>();

    public GameObject enemyButton;
    public Transform Spacer; 
    
    public enum HeroGui
    {
        Activate, 
        Waiting, 
        Input1, // basic attack
        Input2, // select enemy
        Done
    }

    public HeroGui HeroInput;
    public List<GameObject> heroesToManage = new List<GameObject>();
    private BattleAction HeroSelection;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        battleState = PerformAction.Waiting;
        enemiesAlive.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        heroesAlive.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        CreateEnemyButtons(); 
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

    private void CreateEnemyButtons()
    {
        foreach (GameObject enemy in enemiesAlive)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject; 
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();
            EnemyStateMachine currEsm = enemy.GetComponent<EnemyStateMachine>();
            
            newButton.GetComponentInChildren<Text>().text = currEsm.enemy.name;
            button.EnemyPrefab = enemy;
            newButton.transform.SetParent(Spacer);
        }
    }
}
