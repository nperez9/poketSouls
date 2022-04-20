using System.Collections.Generic;

using UnityEngine;

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
                    ConditionId = ConditionID.none
                }
            },
            {

                ConditionID.psn,
                new Condition()
                {
                    Name = "Poison",
                    StartMessage = "Has been poisoned",
                    ConditionId = ConditionID.psn,
                    OnAferTurn = (Pokemon pkm) =>
                    {
                        int poisonDamage = pkm.MaxHp / 8;
                        poisonDamage = (poisonDamage < 1) ? 1 : poisonDamage;
                        pkm.UpdateHP(poisonDamage, true);
                        pkm.StatusChanges.Enqueue($"{pkm.Name} it's hurt by poison");
                    }
                }
            },
            {

                ConditionID.brn,
                new Condition()
                {
                    Name = "Burn",
                    StartMessage = "Has been burned",
                    ConditionId = ConditionID.brn,
                    OnAferTurn = (Pokemon pkm) =>
                    {
                        int burnDamage = pkm.MaxHp / 16;
                        burnDamage = (burnDamage < 1) ? 1 : burnDamage;
                        pkm.UpdateHP(burnDamage, true);
                        pkm.StatusChanges.Enqueue($"{pkm.Name} it's hurt by burn");
                    }
                }
            },
            {
                ConditionID.par,
                new Condition()
                {
                    Name = "Parylisis",
                    StartMessage = "Has been parylized",
                    ConditionId = ConditionID.par,
                    OnBeforeMove = (Pokemon pkm) =>
                    {
                        if (Random.Range(1, 5) == 1)
                        {
                            pkm.StatusChanges.Enqueue($"{pkm.Name} it's paralyzed and can't move!");
                            return false;
                        }
                        return true;
                    }
                }
            },
            {
                ConditionID.frz,
                new Condition()
                {
                    Name = "Frozen",
                    StartMessage = "Has been frozen",
                    ConditionId = ConditionID.frz,
                    OnBeforeMove = (Pokemon pkm) =>
                    {
                        // if it's fire type should have 50% of chance of unfrozen, otherwise 25%
                        int chances = (pkm.IsFireType()) ? 2 : 5;
                        if (Random.Range(1, 5) == 1)
                        {
                            pkm.CureStatus();
                            pkm.StatusChanges.Enqueue($"{pkm.Name} is not frozen anymore");
                            return true;
                        }
                        pkm.StatusChanges.Enqueue($"{pkm.Name} it's ice frozen!");
                        return false;
                    }
                }
            },
            {
                ConditionID.slp,
                new Condition()
                {
                    Name = "Sleep",
                    StartMessage = "Has fall to Sleep",
                    ConditionId = ConditionID.slp,
                    OnStart = (Pokemon pkm) =>
                    {
                        // sleep for 1 to 3 turns
                        pkm.StatusTime = Random.Range(1, 4);
                        Debug.Log($"sleep for {pkm.StatusTime}");
                    },
                    OnBeforeMove = (Pokemon pkm) =>
                    {
                        if (pkm.StatusTime <= 0)
                        {
                            pkm.CureStatus();
                            pkm.StatusChanges.Enqueue($"{pkm.Name} woke up!");
                            return true;
                        }
                        pkm.StatusChanges.Enqueue($"{pkm.Name} it's sleeping like a wood");
                        pkm.StatusTime --;
                        return false;
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