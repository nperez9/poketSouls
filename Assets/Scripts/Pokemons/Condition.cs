using System;

public class Condition
{
    public string Name { get; set; }
    public string Descrition { get; set; }
    public string StartMessage { get; set; }

    public Action<Pokemon> OnBeforeMove { get; set; } 
    public Action<Pokemon> OnAferTurn { get; set; }
}