using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName ="Pokemon/Create new Move", order = 2)]
public class MoveBase : ScriptableObject
{
    [SerializeField] int number;
    [SerializeField] string name;
    [SerializeField] [TextArea] string description;

    [SerializeField] PokemonType type;
    [SerializeField] MoveCategory category;
    [SerializeField] MoveEffect effect;
    [SerializeField] MoveTarget target;
    
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    // Getter - setter shor version
    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public MoveCategory Category { get => category; }
    public MoveEffect Effect { get => effect; }
    public MoveTarget Target { get => target; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int Pp { get => pp; }
}


public enum MoveCategory
{
    Physical,
    Special,
    Status,
}

[System.Serializable]
public class MoveEffect
{
    public List<StatBoost> statBoosts;
}

// todo, apply this to statboost
[System.Serializable]
public class StatBoost
{
    public PokemonStat stat;
    public int boost;
}

public enum MoveTarget
{
    Foe, Self
}