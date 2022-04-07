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
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fight,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dark,
    Dragon,
    Steel,
    Fairy
}

public enum PokemonStat
{
    Hp,
    Attack,
    Defense,
    SpAttack,
    SpDefence,
    Speed
}

public class TypeCharts
{
    public static float[][] chart = {
        /*                    NOR|FIR|WAT|ELE|GRA|ICE|FIG|POI|GRO|FLY|PSY|BUG|ROC|GHO|DAR|DRA|STE|FAI|              */
        /*NOR*/ new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 0f, 1f, 1f, 0.5f, 1f },
        /*FIR*/ new float[] { 1f, 0.5f, 0.5f, 1f, 2f, 2f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 0.5f, 2f, 1f },
        /*WAT*/ new float[] { 1f, 2f, 0.5f, 1f, 0.5f, 1f, 1f, 1f, 2f, 1f, 1f, 1f, 2f, 1f, 1f, 0.5f, 1f, 1f },
        /*ELE*/ new float[] { 1f, 1f, 2f, 0.5f, 0.5f, 1f, 1f, 1f, 0f, 2f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f },
        /*GRA*/ new float[] { 1f, 0.5f, 2f, 1f, 0.5f, 1f, 1f, 0.5f, 2f, 0.5f, 1f, 0.5f, 2f, 1f, 1f, 0.5f, 0.5f, 1f },
        /*ICE*/ new float[] { 1f, 0.5f, 0.5f, 1f, 2f, 0.5f, 1f, 1f, 2f, 2f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f },
        /*FIG*/ new float[] { 2f, 1f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0.5f, 0.5f, 0.5f, 2f, 0f, 2f, 1f, 2f, 0.5f },
        /*POI*/ new float[] { 1f, 1f, 1f, 1f, 2f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, 1f, 0.5f, 0.5f, 1f, 1f, 0f, 2f },
        /*GRO*/ new float[] { 1f, 2f, 1f, 2f, 0.5f, 1f, 1f, 2f, 0f, 1f, 0.5f, 2f, 1f, 1f, 1f, 1f, 2f, 1f },
        /*FLY*/ new float[] { 1f, 1f, 1f, 0.5f, 2f, 1f, 2f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 0.5f, 1f },
        /*PSY*/ new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 2f, 2f, 1f, 1f, 0.5f, 1f, 1f, 1f, 0f, 1f, 0.5f, 1f },
        /*BUG*/ new float[] { 1f, 0.5f, 1f, 1f, 2f, 1f, 0.5f, 0.5f, 1f, 0.5f, 2f, 1f, 1f, 0.5f, 2f, 1f, 0.5f, 0.5f },
        /*ROC*/ new float[] { 1f, 2f, 1f, 1f, 1f, 2f, 0.5f, 1f, 0.5f, 2f, 1f, 2f, 1f, 1f, 1f, 1f, 0.5f, 1f },
        /*GHO*/ new float[] { 0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f },
        /*DAR*/ new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f, 2f, 1f, 1f, 2f, 0.5f, 1f, 1f, 0.5f },
        /*DRA*/ new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 0.5f, 0f },
        /*STE*/ new float[] { 1f, 0.5f, 0.5f, 0.5f, 1f, 2f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 1f, 0.5f, 2f },
        /*FAI*/ new float[] { 1f, 0.5f, 1f, 1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 2f, 0.5f, 1f }
    };

    public static float GetTypeEffectiveness(PokemonType attackType, PokemonType defenceType1, PokemonType defenceType2)
    {
        if (attackType == PokemonType.None ||  defenceType1 == PokemonType.None)
        {
            return 1f;
        }

        // minus one because the none type
        int row = (int)attackType - 1;
        int col1 = (int)defenceType1 - 1;

        float typeEffectiveness1 = chart[row][col1];
        float typeEffectiveness2 = 1f;
        
        if (defenceType2 != PokemonType.None)
        {
            int col2 = (int)defenceType2 - 1;
            typeEffectiveness2 = chart[row][col2];
        }

        float totalModifier = typeEffectiveness1 * typeEffectiveness2;
        return totalModifier;
    }
}   
    
