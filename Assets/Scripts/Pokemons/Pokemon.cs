using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PoketSoulBase _base;
    int level;
    int Hp;
    List<Move> moves = new List<Move>(4);
    string name;

    public PoketSoulBase Base { get => _base; }
    public int Level { get => level; }
    public int HP { get => Hp; set => Hp = value;  }
    public List<Move> Moves { get => moves; }
    public string Name { get => name; }

    public Pokemon(PoketSoulBase _Base, int lvl)
    {
        _base = _Base;
        level = lvl;
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
    public int SpDefence
    {
        get { return Mathf.FloorToInt((_base.SpDefence * level) / 100f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((_base.MaxHp * level) / 100f) + 10; }
    }

    // this is how works in pokemon
    public bool TakeDemage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
        {
            critical = 1.5f;
        }
        float typeEffect = TypeCharts.GetTypeEffectiveness(move.Base.Type, Base.Type1, Base.Type2);

        float modifiers = Random.Range(0.85f, 1f) * critical * typeEffect;
        float a = (2 * attacker.Level + 10) / 250f;
        float b = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(b * modifiers);

        HP -= damage;

        // if pokemon faints, return false
        if (HP < 0)
        {
            HP = 0;
            return false;
        }
        return true;
    }
}
