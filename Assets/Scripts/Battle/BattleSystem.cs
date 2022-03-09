using System;
using System.Collections;

using UnityEngine;
using Random = UnityEngine.Random;

using PokemonN;


namespace Battle { 
    public enum BattleStates { Start, PlayerAction, PlayerMove, EnemyMove, Busy };
    
    public class BattleSystem : MonoBehaviour
    {
        public event Action<bool> OnBattleEnd;

        // Units setting
        [SerializeField] BattleUnit playerUnit = null;
        [SerializeField] BattleHud playerHud = null;
        [SerializeField] BattleUnit enemyUnit = null;
        [SerializeField] BattleHud enemyHud = null;
        
        [SerializeField] BattleDialogBox battleDialogBox = null;

        private BattleStates state = BattleStates.Start;
        private int currentAction = 0;
        private int currentMove = 0;

        private PokemonParty pokemonParty;
        private Pokemon enemyPokemon;

        private bool firstFrameMove = false;
        public void StartBattle(PokemonParty pkmParty, Pokemon enemyPkm)
        {
            pokemonParty = pkmParty;
            enemyPokemon = enemyPkm;
            StartCoroutine(SetupBattle());
        }

        public IEnumerator SetupBattle()
        {
            SetPlayerPokemon(pokemonParty.GetFirstHealtyPokemon());
            SetEnemyPokemon();

            yield return battleDialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appears!");
            yield return new WaitForSeconds(1f);

            PlayerAction();
        }

        private void SetPlayerPokemon(Pokemon playerPkm)
        {
            playerUnit.Setup(pokemonParty.GetFirstHealtyPokemon());
            playerHud.SetData(playerUnit.Pokemon);
            battleDialogBox.SetMovesText(playerUnit.Pokemon.Moves);
        }

        private void SetEnemyPokemon()
        {
            enemyUnit.Setup(enemyPokemon);
            enemyHud.SetData(enemyUnit.Pokemon);
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
            firstFrameMove = true;
            battleDialogBox.SetEnabledDialogBox(false);
            battleDialogBox.SetEnabledActionBox(false);
            battleDialogBox.SetEnabledMoveBox(true);
        }

        public void HandleUpdate()
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

            if (Input.GetKeyDown(KeyCode.Z) && !firstFrameMove)
            {
                battleDialogBox.SetEnabledDialogBox(true);
                battleDialogBox.SetEnabledActionBox(false);
                battleDialogBox.SetEnabledMoveBox(false);

                StartCoroutine(ExecutePlayerMove());
            }

            firstFrameMove = false;
        }

        IEnumerator ExecutePlayerMove()
        {
            state = BattleStates.Busy;
            Move move = playerUnit.Pokemon.Moves[currentMove];
            
            yield return battleDialogBox.TypeDialog($"{playerUnit.Pokemon.Name}! Use {move.Base.Name}!");
            yield return new WaitForSeconds(1f);

            playerUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            enemyUnit.PlayHitAnimation();

            DamageDetails EnemyDemageDetails = enemyUnit.Pokemon.TakeDemage(move, playerUnit.Pokemon);
            yield return enemyHud.UpdateHP();
            yield return DamageDialog(EnemyDemageDetails);

            if (EnemyDemageDetails.Fainted)
            {
                yield return battleDialogBox.TypeDialog($"{enemyUnit.Pokemon.Name} Faints!");
                enemyUnit.PlayFaintAnimation();
                yield return new WaitForSeconds(1.5f);
                OnBattleEnd(true);
            } 
            else
            {
                // make this on speed base 
                StartCoroutine(ExecuteEnemyMove());
            }
        }

        IEnumerator ExecuteEnemyMove()
        {
            state = BattleStates.EnemyMove;
            Move move = enemyUnit.Pokemon.Moves[(int)Random.Range(0, enemyUnit.Pokemon.Moves.Count)];

            yield return battleDialogBox.TypeDialog($"{enemyUnit.Pokemon.Name} enemy use {move.Base.Name}");
            yield return new WaitForSeconds(1f);

            enemyUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            playerUnit.PlayHitAnimation();

            DamageDetails PlayerDamageDetails = playerUnit.Pokemon.TakeDemage(move, enemyUnit.Pokemon);
            yield return playerHud.UpdateHP();
            yield return DamageDialog(PlayerDamageDetails);

            if (PlayerDamageDetails.Fainted)
            {
                yield return battleDialogBox.TypeDialog($"Your {playerUnit.Pokemon.Name} faints!");
                playerUnit.PlayFaintAnimation();
                yield return new WaitForSeconds(1.5f);

                Pokemon nextPokemon = pokemonParty.GetFirstHealtyPokemon();

                if(nextPokemon != null)
                {
                    SetPlayerPokemon(nextPokemon);
                    yield return battleDialogBox.TypeDialog($"Go! {nextPokemon.Name}!");
                    yield return new WaitForSeconds(1f);
                    PlayerAction();
                }
                else
                {
                    OnBattleEnd(false);
                }
            } 
            else
            {
                // Make this speed base
                PlayerAction();
            }
        }

        IEnumerator DamageDialog(DamageDetails dd)
        {
            string damageText = "";
            if (dd.Critical > 1f)
            {
                damageText += "A critical hit! ";
            }
            if (dd.TypeEffect > 1f)
            {
                damageText += "It's super effective!";
            } 
            else if (dd.TypeEffect < 1f)
            {
                damageText += "It's not very effective...";
            }

            if (damageText != "")
            {
                yield return battleDialogBox.TypeDialog(damageText);
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }
}
