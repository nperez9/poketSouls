using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class BattleDialogBox : MonoBehaviour
    {
        [SerializeField] Text DialogBoxText = null;
        [SerializeField] int letterForSecond = 30;

        public void SetDialogBoxText(string content)
        {
            DialogBoxText.text = content;
        }

        // Use for type typing animations
        public IEnumerator TypeDialog(string dialog)
        {
            DialogBoxText.text = "";
            foreach (char letter in dialog.ToCharArray())
            {
                DialogBoxText.text += letter;
                yield return new WaitForSeconds(1f / letterForSecond);
            }
        }
    }
}
