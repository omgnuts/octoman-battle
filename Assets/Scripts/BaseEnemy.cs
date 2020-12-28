[System.Serializable]
public class BaseEnemy 
{
    public string name;

    public enum Type
    {
        Grass,
        Fire,
        Water,
        Electric
    }

    public enum Rarity
    {
        Common, 
        Uncommon, 
        Rare, 
        SuperRare
    }

    public Type enemyType;
    public Rarity rarityType;

    public float baseHp;
    public float currHp;
    public float baseMp;
    public float currMp;

    public float baseAtk;
    public float currAtk;
    public float baseDef;
    public float currDef; 
    
}
