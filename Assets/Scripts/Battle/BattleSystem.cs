using System.Collections;
using UnityEngine;

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
        private int currentAction = 0;
        private int currentMove = 0;

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

            battleDialogBox.SetMovesText(playerUnit.Pokemon.Moves);

            yield return battleDialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appears!");
            yield return new WaitForSeconds(1f);

            PlayerAction();
        }

        private void PlayerAction()
        {
            state = BattleStates.PlayerAction;
            StartCoroutine(battleDialogBox.TypeDialog("What you wanna do?"));
            battleDialogBox.SetEnabledDialogBox(true);
            battleDialogBox.SetEnabledActionBox(true);
            battleDialogBox.SetEnabledMoveBox(false);
        }

        private void PlayerMove()
        {
            state = BattleStates.PlayerMove;
            battleDialogBox.SetEnabledDialogBox(false);
            battleDialogBox.SetEnabledActionBox(false);
            battleDialogBox.SetEnabledMoveBox(true);
        }

        private void Update()
        {
            if (state == BattleStates.PlayerAction)
            {
                HandlePlayerAction();
            }
            if (state == BattleStates.PlayerMove)
            {
                HandlePlayerMove();
            }
        }


        private void HandlePlayerAction()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentAction < 1)
                {
                    currentAction++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentAction > 0)
                {
                    currentAction--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentAction < 2)
                {
                    currentAction += 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentAction > 1)
                {
                    currentAction -= 2;
                }
            }

            battleDialogBox.UpdateActionSelection(currentAction);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                switch (currentAction)
                {
                    case 0:
                        PlayerMove();
                        break;
                    case 1:
                        Debug.Log("");
                        break;
                }
            }
        }

        private void HandlePlayerMove()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
                {
                    currentMove += 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentMove > 1)
                {
                    currentMove -= 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
                {
                    currentMove++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentMove > 0)
                {
                    currentMove--;
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                PlayerAction();
            }

            // TODO: check this, may be some bugs
            battleDialogBox.UpdateMovesSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                battleDialogBox.SetEnabledDialogBox(true);
                battleDialogBox.SetEnabledActionBox(false);
                battleDialogBox.SetEnabledMoveBox(false);

                StartCoroutine(ExecutePlayerMove());
            }
        }

        IEnumerator ExecutePlayerMove()
        {
            Move move = playerUnit.Pokemon.Moves[currentMove];
            yield return battleDialogBox.TypeDialog($"{playerUnit.Pokemon.Name}! Use {move.Base.Name}!");
            
            yield return new WaitForSeconds(1f);
        }
    }
}
