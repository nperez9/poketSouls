using System.Collections.Generic;

namespace Data {   
    public class ConditionsDB
    {
        public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
        {
            {
                ConditionID.none,
                new Condition()
                {
                    Name = "None",
                    StartMessage = "",
                }
            },
            {

                ConditionID.psn,
                new Condition()
                {
                    Name = "Poison",
                    StartMessage = "Has been poisoned",
                    OnAferTurn = (Pokemon pkm) =>
                    {
                        int poisonDamage = pkm.MaxHp / 8;
                        poisonDamage = (poisonDamage < 1) ? 1 : poisonDamage;
                        pkm.UpdateHP(poisonDamage, true);
                        pkm.StatusChanges.Enqueue($"{pkm.Name} it's hurt by poison");
                    }
                }
            }
        };
    }

    public enum ConditionID
    {
        none, psn, brn, slp, par, frz
    }
}