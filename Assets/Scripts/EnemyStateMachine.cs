using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public BaseEnemy enemy; 

    public enum TurnState
    {
        Processing, 
        AddToList, 
        Waiting, 
        Selecting, 
        Action, 
        Dead
    }

    public TurnState currentState;

    private float currCooldown = 0f;
    private float maxCooldown = 5f;
    
    // Start is called before the first frame update
    private void Start()
    {
        currentState = TurnState.Processing;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case (TurnState.Processing):
                updateProgressBar();
                break;
            case (TurnState.AddToList): break;
            case (TurnState.Waiting): break;
            case (TurnState.Selecting): break;
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
            currentState = TurnState.AddToList;
        }
    }
}
