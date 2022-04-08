using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PokemonN;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PoketSoulBase _base;
    [SerializeField] int level;
    private int Hp;
    private List<Move> moves = new List<Move>(4);
    private string name;

    public PoketSoulBase Base { get => _base; }
    public int Level { get => level; }
    public int HP { get => Hp; set => Hp = value; }
    public List<Move> Moves { get => moves; }
    public Dictionary<PokemonStat, int> Stats { get; private set; }
    public Dictionary<PokemonStat, int> StatsBoost { get; private set; }

    public string Name { get => name; }

    public void Init()
    {
        name = _base.Name;

        // add moves in base of the learnedStuff
        foreach (var learnableMove in _base.LearnableMoves)
        {
            if (level >= learnableMove.Level)
            {
                this.moves.Add(new Move(learnableMove.Move));
            }

            if (this.moves.Count >= 4)
            {
                break;
            }
        }

        CalculateAndSaveStats();
        InitializeStatsBoost();

        // Set hp after calculated stats
        Hp = MaxHp;
    }

    public void ApplyBoost(List<StatBoost> statBoosts) 
    {
        foreach (StatBoost statBoost in statBoosts)
        {
            PokemonStat stat = statBoost.stat;
            int boostValue = statBoost.boost;

            this.StatsBoost[stat] = Mathf.Clamp(this.StatsBoost[stat] + boostValue, -6, 6);
        }
    }

    public int Attack
    {
        get { return GetStat(PokemonStat.Attack); }
    }
    public int Defense
    {
        get { return GetStat(PokemonStat.Defense); }
    }
    public int Speed
    {
        get { return GetStat(PokemonStat.Speed); }
    }
    public int SpAttack
    {
        get { return GetStat(PokemonStat.SpAttack); }
    }
    public int SpDefense
    {
        get { return GetStat(PokemonStat.SpDefence); }
    }
    public int MaxHp
    {
        get { return Stats[PokemonStat.Hp]; }
    }

    // this is how works in pokemon
    public DamageDetails TakeDemage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f) {
            critical = 1.5f;
        }
        float typeEffect = TypeCharts.GetTypeEffectiveness(move.Base.Type, Base.Type1, Base.Type2);
        // stab: if the attacker pokemon has the same type tan the attack does 50% more demage;
        float stab = move.Base.Type == attacker.Base.Type1 || move.Base.Type == attacker.Base.Type2 ? 1.5f : 1f;
        bool fainted = false;

        // In case of status change, the damage will be 1
        int attack = 1;
        int defense = 1;
        
        if (move.Base.Category == MoveCategory.Special)
        {
            attack = attacker.SpAttack;
            defense = this.SpDefense;
        }
        else if (move.Base.Category == MoveCategory.Physical)
        {
            attack = attacker.Attack;
            defense = this.Defense;
        }

        int damage = CalculateDamage(move, critical, typeEffect, stab, attacker.Level, attack, defense);

        HP -= damage;

        // if pokemon faints, return false
        if (HP <= 0)
        {
            HP = 0;
            fainted = true;
        }

        // before return, reduce the movePP
        move.PP--;

        return new DamageDetails()
        {
            Critical = critical,
            TypeEffect = typeEffect,
            Fainted = fainted,
        };
    }

    // this function use original pokemon(red, blue, green) calcule
    // TODO: Update to a modern pokemon game
    private int CalculateDamage(Move move, float critical, float typeEffect, float stab, int attackerLevel, int attack, int defense)
    {
        float modifiers = Random.Range(0.85f, 1f) * critical * typeEffect * stab;
        float a = (2 * attackerLevel + 10) / 250f;
        float b = a * move.Base.Power * ((float)attack / defense) + 2;
        return Mathf.FloorToInt(b * modifiers);
    }

    private void CalculateAndSaveStats()
    {
        Stats = new Dictionary<PokemonStat, int>();
        Stats.Add(PokemonStat.Hp, CalculateStat(_base.MaxHp));
        Stats.Add(PokemonStat.Attack, CalculateStat(_base.Attack));
        Stats.Add(PokemonStat.Defense, CalculateStat(_base.Defence));
        Stats.Add(PokemonStat.SpAttack, CalculateStat(_base.SpAttack));
        Stats.Add(PokemonStat.SpDefence, CalculateStat(_base.SpDefence));
        Stats.Add(PokemonStat.Speed, CalculateStat(_base.Speed));
    }

    private void InitializeStatsBoost()
    {
        // cant't boost hp
        StatsBoost = new Dictionary<PokemonStat, int>() {
            { PokemonStat.Hp, 0 },
            { PokemonStat.Attack, 0 },
            { PokemonStat.Defense, 0 },
            { PokemonStat.SpAttack, 0 },
            { PokemonStat.SpDefence, 0 },
            { PokemonStat.Speed, 0 },
        };
    }

    private int CalculateStat(int baseValue) 
    {
        return Mathf.FloorToInt((baseValue * level) / 100f) + 5;
    }


    private int GetStat(PokemonStat stat)
    {
        int value = Stats[stat];
        int statBoost = StatsBoost[stat];

        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };
        
        if (statBoost >= 0)
        {
            value = Mathf.FloorToInt(value * boostValues[statBoost]);
        }
        else
        {
            value = Mathf.FloorToInt(value / boostValues[statBoost * -1]);
        }

        Debug.Log($"return {stat} at | {statBoost}");
        return value;
    }
}
