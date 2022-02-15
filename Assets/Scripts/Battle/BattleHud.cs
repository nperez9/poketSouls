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
    
        public void SetData(Pokemon pkm)
        {
            nameText.text = pkm.Base.Name;
            LevelText.text = "Lv. " + pkm.Level.ToString();
            hpBar.SetHp((float) pkm.HP / pkm.MaxHp);
        }
    }
}