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

    public Image ProgressFG;
     
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
        var ratio = currCooldown / maxCooldown;
        
        ProgressFG.transform.localScale = new Vector3(Mathf.Clamp(ratio, 0f, 1f),
            ProgressFG.transform.localScale.y, ProgressFG.transform.localScale.z);

        if (currCooldown >= maxCooldown)
        {
            currentState = TurnState.AddToList;
        }
    }
}
