using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Data;

namespace Battle {
    public class BattleHud : MonoBehaviour
    {
        [SerializeField] Text nameText = null;
        [SerializeField] Text LevelText = null;
        [SerializeField] HpBar hpBar = null;
        [SerializeField] Image statusImage = null;
        [SerializeField] List<StatusMap> statusMaps;

        private Pokemon _pokemon = null;

        public void SetData(Pokemon pkm)
        {
            _pokemon = pkm;

            nameText.text = pkm.Base.Name;
            LevelText.text = "Lv. " + pkm.Level.ToString();
            hpBar.SetHp((float)pkm.HP / pkm.MaxHp);
            UpdateStatus(pkm.Status.ConditionId);
        }

        // Uses the pokemon reference to update the hp.
        public IEnumerator UpdateHP()
        {
            yield return hpBar.SetHpSmooth((float)_pokemon.HP / _pokemon.MaxHp);
        }

        public void UpdateStatus(ConditionID condition)
        {
            if (condition == ConditionID.none)
            {
                statusImage.enabled = false;
            }
            else
            {
                statusImage.enabled = true;
                statusImage.sprite = statusMaps.Where(statusM => statusM.Condition == ConditionID.none).FirstOrDefault().Image;
            }
        }
    }

    [System.Serializable]
    public class StatusMap
    {
        [SerializeField] public ConditionID Condition;
        [SerializeField] public Sprite Image;
    }
}