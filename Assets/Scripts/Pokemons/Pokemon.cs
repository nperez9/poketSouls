using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PokemonN;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PoketSoulBase _base;
    [SerializeField] int level;
    int Hp;
    List<Move> moves = new List<Move>(4);
    string name;

    public PoketSoulBase Base { get => _base; }
    public int Level { get => level; }
    public int HP { get => Hp; set => Hp = value; }
    public List<Move> Moves { get => moves; }
    public string Name { get => name; }

    //public Pokemon(PoketSoulBase _Base, int lvl)
    //{
    //    _base = _Base;
    //    level = lvl;
    //    Init();
    //}

    public void Init()
    {
        Hp = MaxHp;
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
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((_base.Defence * level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((_base.SpAttack * level) / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((_base.SpDefence * level) / 100f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((_base.MaxHp * level) / 100f) + 10; }
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
}
