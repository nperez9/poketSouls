using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle { 
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] BattleUnit playerUnit = null;
        [SerializeField] BattleHud playerHud = null;
       
        [SerializeField] BattleUnit enemyUnit = null;
        [SerializeField] BattleHud enemyHud = null;

        [SerializeField] BattleDialogBox battleDialogBox = null;


        private void Start()
        {
            playerUnit.Setup();
            playerHud.SetData(playerUnit.Pokemon);

            enemyUnit.Setup();
            enemyHud.SetData(enemyUnit.Pokemon);

            StartCoroutine(battleDialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appears!"));
        }
    }
}
