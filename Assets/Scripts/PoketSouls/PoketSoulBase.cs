using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName ="Pokemon/Create new Pokemon", order = 1)]
public class PoketSoulBase : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string name;
    [SerializeField] [TextArea] string description;

    [SerializeField] Sprite littleSprite1;
    [SerializeField] Sprite littleSprite2;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defence;
    [SerializeField] int spAttack;
    [SerializeField] int spDefence;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    public int Id { get => id; }
    public string Name { get => name; }
    public string Description { get => description; }
    public Sprite LittleSprite1 { get => littleSprite1; }
    public Sprite LittleSprite2 { get => littleSprite2; }
    public Sprite FrontSprite { get => frontSprite; }
    public Sprite BackSprite { get => backSprite; }
    public PokemonType Type1 { get => type1; }
    public PokemonType Type2 { get => type2; }
    public int MaxHp { get => maxHp; }
    public int Attack { get => attack; }
    public int Defence { get => defence; }
    public int SpAttack { get => spAttack; }
    public int SpDefence { get => spDefence; }
    public int Speed { get => speed; }
    public List<LearnableMove> LearnableMoves { get => learnableMoves; }

    // Getter - setter shor version
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] int level;
    [SerializeField] MoveBase move;

    public int Level { get => level; }
    public MoveBase Move { get => move; }
} 

public enum PokemonType
{
    None,
    Water,
    Fire,
    Grass,
    Ice,
    Electric,
    Fight,
    Dark,
    Normal,
    Poison,
    Dragon,
    Ghost,
    Flying,
    Ground,
    Rock,
    Steel,
    Psych
}
