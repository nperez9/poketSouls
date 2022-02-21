using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle { 
    public class BattleHud : MonoBehaviour
    {
        [SerializeField] Text nameText = null;
        [SerializeField] Text LevelText = null;
        [SerializeField] HpBar hpBar = null;

        private Pokemon _pokemon = null;

        public void SetData(Pokemon pkm)
        {
            _pokemon = pkm;

            nameText.text = pkm.Base.Name;
            LevelText.text = "Lv. " + pkm.Level.ToString();
            hpBar.SetHp((float) pkm.HP / pkm.MaxHp);
        }

        // Uses the pokemon reference to update the hp.
        public IEnumerator UpdateHP()
        {
            yield return hpBar.SetHpSmooth((float)_pokemon.HP / _pokemon.MaxHp);
        }
    }
}