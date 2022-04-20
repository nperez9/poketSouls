using System;
using Data;
public class Condition
{
    public string Name { get; set; }
    public string Descrition { get; set; }
    public string StartMessage { get; set; }
    public Action<Pokemon> OnStart { get; set; }
    public Func<Pokemon, bool> OnBeforeMove { get; set; }
    public Action<Pokemon> OnAferTurn { get; set; }
    public ConditionID ConditionId { get; set; }
}
