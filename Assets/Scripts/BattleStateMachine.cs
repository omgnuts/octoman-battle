using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        Wait, 
        PrepareAction, 
        PerformAction
    }

    public PerformAction battleStates;

    public List<BattleAction> actions = new List<BattleAction>();
    public List<GameObject> heroes = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    
    // Start is called before the first frame update
    private void Start()
    {
        battleStates = PerformAction.Wait;
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        heroes.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        
    }

    // Update is called once per frame
    private void Update()
    {
        switch (battleStates)
        {
            case PerformAction.Wait: break;
            case PerformAction.PrepareAction: break;
            case PerformAction.PerformAction: break;
            default: break;
        }
    }

    public void AddAction(BattleAction action)
    {
        actions.Add(action);
    }
}
