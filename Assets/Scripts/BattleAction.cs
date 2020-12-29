using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleAction
{
    public GameObject attackerGameObject;
    public GameObject defenderGameObject;

    public ToonType attackerType; 
    
    public enum ToonType
    {
        Enemy, Hero
    }
}
