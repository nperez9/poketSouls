using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PoketSoulBase _base;
    int level;
    int Hp;
    List<Move> moves = new List<Move>(4);

    public PoketSoulBase Base { get => _base; }
    public int Level { get => level; }
    public int HP { get => Hp; }
    public List<Move> Moves { get => moves; }

    public Pokemon(PoketSoulBase _Base, int lvl)
    {
        _base = _Base;
        level = lvl;
        Hp = MaxHp;

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
}
