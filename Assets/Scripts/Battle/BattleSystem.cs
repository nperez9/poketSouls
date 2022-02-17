using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle { 
    public enum BattleStates { Start, PlayerAction, PlayerMove, EnemyMove, Busy };
    public class BattleSystem : MonoBehaviour
    {
        // Units setting
        [SerializeField] BattleUnit playerUnit = null;
        [SerializeField] BattleHud playerHud = null;
        [SerializeField] BattleUnit enemyUnit = null;
        [SerializeField] BattleHud enemyHud = null;
        
        [SerializeField] BattleDialogBox battleDialogBox = null;

        private BattleStates state = BattleStates.Start;
        private void Start()
        {
            StartCoroutine(SetupBattle());
        }

        public IEnumerator SetupBattle()
        {
            playerUnit.Setup();
            playerHud.SetData(playerUnit.Pokemon);

            enemyUnit.Setup();
            enemyHud.SetData(enemyUnit.Pokemon);

            yield return battleDialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appears!");
            yield return new WaitForSeconds(1f);

            PlayerAction();
        }

        private void PlayerAction()
        {
            state = BattleStates.PlayerAction;
            StartCoroutine(battleDialogBox.TypeDialog("What you wanna do?"));
            battleDialogBox.SetEnabledActionBox(true);
        }

        private void Update()
        {
             if (state == BattleStates.PlayerAction)
            {
                HandlePlayerAction();
            }
        }


        private void HandlePlayerAction()
        {

        }
    }
}
