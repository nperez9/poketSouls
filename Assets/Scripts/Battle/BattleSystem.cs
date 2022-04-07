using System;
using System.Collections;

using UnityEngine;
using Random = UnityEngine.Random;

using PokemonN;


namespace Battle { 
    public enum BattleStates { Start, ActionSelection, MoveSelection, PerformMove, Busy, PartySwitch, BattleOver };
    
    public class BattleSystem : MonoBehaviour
    {
        public event Action<bool> OnBattleEnd;

        [SerializeField] BattleUnit playerUnit = null;
        [SerializeField] BattleUnit enemyUnit = null;
        [SerializeField] BattleDialogBox battleDialogBox = null;

        // Screens
        [SerializeField] PartyScreen partyScreen = null;

        private BattleStates state = BattleStates.Start;
        private int currentAction = 0;
        private int currentMove = 0;
        private int currentPokemonSwitch = 0;

        private PokemonParty pokemonParty;
        private Pokemon enemyPokemon;

        private bool firstFrameMove = false;
        public void StartBattle(PokemonParty pkmParty, Pokemon enemyPkm)
        {
            pokemonParty = pkmParty;
            enemyPokemon = enemyPkm;
            
            partyScreen.Init();
            StartCoroutine(SetupBattle());
        }

        public IEnumerator SetupBattle()
        {
            SetPlayerPokemon(pokemonParty.GetFirstHealtyPokemon());
            SetEnemyPokemon();
            // prevents overlaps
            disableAllScreens();
            RestartCursorPositons();

            yield return battleDialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appears!");
            yield return new WaitForSeconds(1f);

            ActionSelection();
        }

        private void RestartCursorPositons()
        {
            currentAction = 0;
            currentMove = 0;
            currentPokemonSwitch = 0;
         }

        private void disableAllScreens()
        {
            battleDialogBox.SetEnabledDialogBox(true);
            battleDialogBox.SetEnabledActionBox(false);
            battleDialogBox.SetEnabledMoveBox(false);
            partyScreen.gameObject.SetActive(false);
        }

        private void SetPlayerPokemon(Pokemon playerPkm)
        {
            playerUnit.Setup(playerPkm);
            battleDialogBox.SetMovesText(playerUnit.Pokemon.Moves);
        }

        private void SetEnemyPokemon()
        {
            enemyUnit.Setup(enemyPokemon);
        }

        private void ActionSelection()
        {
            state = BattleStates.ActionSelection;
            currentAction = 0;
            StartCoroutine(battleDialogBox.TypeDialog("What you wanna do?"));
            battleDialogBox.SetEnabledDialogBox(true);
            battleDialogBox.SetEnabledActionBox(true);
            battleDialogBox.SetEnabledMoveBox(false);
            partyScreen.gameObject.SetActive(false);
        }

        private void MoveSelection()
        {
            state = BattleStates.MoveSelection;
            firstFrameMove = true;
            currentMove = 0;
            battleDialogBox.SetEnabledDialogBox(false);
            battleDialogBox.SetEnabledActionBox(false);
            battleDialogBox.SetEnabledMoveBox(true);
            partyScreen.gameObject.SetActive(false);
        }

        private void BattleOver(bool won)
        {
            state = BattleStates.BattleOver;
            OnBattleEnd(won);
        }

        private void PokemonChange()
        {
            state = BattleStates.PartySwitch;
            firstFrameMove = true;
            partyScreen.SetPartyMembers(pokemonParty.GetPokemonsList());

            battleDialogBox.SetEnabledDialogBox(true);
            battleDialogBox.SetEnabledActionBox(false);
            battleDialogBox.SetEnabledMoveBox(false);
            partyScreen.gameObject.SetActive(true);
        }

        public void HandleUpdate()
        {
            if (state == BattleStates.ActionSelection)
            {
                HandlePlayerAction();
            }
            if (state == BattleStates.MoveSelection)
            {
                HandlePlayerMove();
            }
            if (state == BattleStates.PartySwitch)
            {
                HandleMemberSwitch();
            }
        }

        private void HandleMemberSwitch()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                currentPokemonSwitch ++;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                currentPokemonSwitch --;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                currentPokemonSwitch += 2;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                currentPokemonSwitch -= 2;

            currentPokemonSwitch = Math.Clamp(currentPokemonSwitch, 0, pokemonParty.GetPokemonsList().Count - 1);
            partyScreen.SetCursor(currentPokemonSwitch, pokemonParty.GetPokemonsList());

            if (Input.GetKeyDown(KeyCode.Z) && !firstFrameMove)
            {
                Pokemon selectedPokemon = pokemonParty.GetPokemonsList()[currentPokemonSwitch];

                if (selectedPokemon.HP <= 0)
                {
                    partyScreen.SetMessage("Can't change to a fainted Pokemon");
                    return;
                }
                if (selectedPokemon == playerUnit.Pokemon)
                {
                    partyScreen.SetMessage($"{playerUnit.Pokemon.Name} is already in Battle");
                    return;
                }


                state = BattleStates.Busy;
                partyScreen.gameObject.SetActive(false);
                StartCoroutine(SwitchPokemon(selectedPokemon));
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                ActionSelection();
            }

            firstFrameMove = false;
        }


        private void HandlePlayerAction()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                currentAction += 2;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                currentAction -= 2;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                ++currentAction;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                --currentAction;
            
            /**
            this will make the nearest value between 0 and 3 in base of currentAction
            Probably a better solution than HandlePlayerMove
            **/
            currentAction = Math.Clamp(currentAction, 0, 3);
            battleDialogBox.UpdateActionSelection(currentAction);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                switch (currentAction)
                {
                    case 0:
                        MoveSelection();
                        break;
                    case 1:
                        Debug.Log("Object");
                        break;
                    case 2:
                        Debug.Log("Switch");
                        PokemonChange();
                        break;
                    case 3:
                        Debug.Log("Run");
                        break;
                }
            }
        }

        private void HandlePlayerMove()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                currentMove ++;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                currentMove --;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                currentMove +=2;
            else if (Input.GetKeyDown(KeyCode.UpArrow))          
                currentMove-= 2;

            if (Input.GetKeyDown(KeyCode.X))
            {
                ActionSelection();
            }

            currentAction = Math.Clamp(currentAction, 0, playerUnit.Pokemon.Moves.Count - 1);
            battleDialogBox.UpdateMovesSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

            // TODO: FirstFrameMove doesn't look like a very nice solution
            if (Input.GetKeyDown(KeyCode.Z) && !firstFrameMove)
            {
                battleDialogBox.SetEnabledDialogBox(true);
                battleDialogBox.SetEnabledActionBox(false);
                battleDialogBox.SetEnabledMoveBox(false);

                StartCoroutine(ExecutePlayerMove());
            }

            firstFrameMove = false;
        }

        IEnumerator SwitchPokemon(Pokemon swicthedPokemon)
        {
            bool isDefeatChange = playerUnit.Pokemon.HP <= 0;
            
            if (!isDefeatChange)
            {
                yield return battleDialogBox.TypeDialog($"{playerUnit.Pokemon.Name}! Get back!");
                playerUnit.PlayFaintAnimation();
                yield return new WaitForSeconds(1.5f);
            }

            SetPlayerPokemon(swicthedPokemon);
            yield return battleDialogBox.TypeDialog($"Go {swicthedPokemon.Name}!");
            yield return new WaitForSeconds(1.5f);

            if (isDefeatChange)
            {
                ActionSelection();
            }
            else
            {
                yield return ExecuteEnemyMove();
            }
        }

        IEnumerator ExecutePlayerMove()
        {
            state = BattleStates.PerformMove;
            Move move = playerUnit.Pokemon.Moves[currentMove];

            yield return ExecuteMove(playerUnit, enemyUnit, move);
            // make this on speed base 

            if (state == BattleStates.PerformMove)
            {
                StartCoroutine(ExecuteEnemyMove());
            }
        }

        IEnumerator ExecuteEnemyMove()
        {
            state = BattleStates.PerformMove;
            Move move = enemyUnit.Pokemon.Moves[(int)Random.Range(0, enemyUnit.Pokemon.Moves.Count)];

            yield return ExecuteMove(enemyUnit, playerUnit, move);

            if (state == BattleStates.PerformMove)
            {                
                ActionSelection();
            }
        }

        IEnumerator ExecuteMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
        {
            yield return battleDialogBox.TypeDialog($"{sourceUnit.Pokemon.Name}! Use {move.Base.Name}!");
            yield return new WaitForSeconds(1f);

            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            targetUnit.PlayHitAnimation();

            if (move.Base.Category == MoveCategory.Status)
            {
                // TODO: Do something
                var effect = move.Base.Effect;
                if (effect.statBoosts != null)
                {
                    if (move.Base.Target == MoveTarget.Self)
                    {
                        sourceUnit.Pokemon.ApplyBoost(effect.statBoosts);
                    }
                    else
                    {
                        targetUnit.Pokemon.ApplyBoost(effect.statBoosts);
                    }
                }
            } 
            else 
            { 
                DamageDetails TargetDemageDetails = targetUnit.Pokemon.TakeDemage(move, sourceUnit.Pokemon);
                yield return targetUnit.HUD.UpdateHP();
                yield return DamageDialog(TargetDemageDetails);

                if (TargetDemageDetails.Fainted)
                {
                    if (targetUnit.IsPlayerUnit)
                    {
                        yield return battleDialogBox.TypeDialog($"Your {playerUnit.Pokemon.Name} faints!");
                    }
                    else
                    {
                        yield return battleDialogBox.TypeDialog($"{targetUnit.Pokemon.Name} Faints!");
                    }

                    targetUnit.PlayFaintAnimation();
                    yield return new WaitForSeconds(1.5f);

                    CheckForBattleEnd(targetUnit);
                }
            }
        }

        void CheckForBattleEnd(BattleUnit faintedUnit)
        {
            if (faintedUnit.IsPlayerUnit)
            {
                Pokemon nextPokemon = pokemonParty.GetFirstHealtyPokemon();

                if (nextPokemon != null)
                {
                    PokemonChange();
                }
                else
                {
                    BattleOver(false);
                }
            }
            else
            {

                BattleOver(true);
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
