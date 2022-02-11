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
    
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    // Getter - setter shor version
    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public MoveCategory Category { get => category; }
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