using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PoketSoulBase _base;
    int level;
    int Hp;
    List<Move> moves;

    public Pokemon(PoketSoulBase _Base, int lvl)
    {
        _base = _Base;
        level = lvl;
        Hp = _base.MaxHp;

        // add moves in base of the learnedStuff
        foreach (var learnableMove in _base.LearnableMoves)
        {
            if (learnableMove.Level >= level)
            {
                this.moves.Add(new Move(learnableMove.Move));
            }

            if (moves.Count >= 4)
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
