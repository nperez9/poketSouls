using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit = null;
    [SerializeField] BattleHud playerHud = null;

    [SerializeField] BattleUnit enemyUnit = null;
    [SerializeField] BattleHud enemyHud = null;


    private void Start()
    {
        playerUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);

        enemyUnit.Setup();
        enemyHud.SetData(enemyUnit.Pokemon);
    }
}
