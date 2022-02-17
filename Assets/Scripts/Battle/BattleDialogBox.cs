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

        [SerializeField] GameObject ActionBox = null;
        [SerializeField] GameObject MovesBox = null;
        [SerializeField] GameObject MoveInfoBox = null;

        [SerializeField] List<Text> ActionsList;
        [SerializeField] List<Text> MovesList;
        [SerializeField] Text MovePPText = null;
        [SerializeField] Text MoveTypeText = null;


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

        public void SetEnabledDialogBox(bool enabled)
        {
            DialogBoxText.enabled = true;
        }

        public void SetEnabledActionBox(bool enabled)
        {
            ActionBox.SetActive(enabled);
        }

        public void SetEnabledMoveBox(bool enabled)
        {
            MoveInfoBox.SetActive(enabled);
            MovesBox.SetActive(enabled);
        }
    }
}
