using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class PartyMember : MonoBehaviour
    {
        [SerializeField] Text nameText = null;
        [SerializeField] Text LevelText = null;
        [SerializeField] HpBar hpBar = null;
        
        [SerializeField] Color higthligthedColor;

        private Pokemon _pokemon = null;
        private bool enabled = true;

        public void SetData(Pokemon pkm)
        {
            _pokemon = pkm;

            nameText.text = pkm.Base.Name;
            LevelText.text = "Lv. " + pkm.Level.ToString();
            hpBar.SetHp((float)pkm.HP / pkm.MaxHp);
        }

        public void DisablePartySlot()
        {
            GetComponent<Image>().color = Color.black;
            nameText.text = "";
            LevelText.text = "";
            hpBar.gameObject.SetActive(false);
            enabled = false;
        }

        public void AddCursor()
        {
            nameText.color = higthligthedColor;
        }

        public void RemoveCursor()
        {
            nameText.color = Color.black;
        }

        // Uses the pokemon reference to update the hp.
        public IEnumerator UpdateHP()
        {
            yield return hpBar.SetHpSmooth((float)_pokemon.HP / _pokemon.MaxHp);
        }
    }
}