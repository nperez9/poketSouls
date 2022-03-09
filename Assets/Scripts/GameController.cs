using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Battle;
using PokemonN;

public enum GameState { Adventure, Battle };
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController = null;
    [SerializeField] BattleSystem battleSystem = null;
    [SerializeField] GameObject battleStage = null;
    [SerializeField] Camera mainCamera = null;

    private GameState gameState = GameState.Adventure;

    private void Start()
    {
        playerController.OnBattle += StartBattle;
        battleSystem.OnBattleEnd += FinishBattle;
    }

    private void StartBattle()
    {
        battleStage.SetActive(true);
        mainCamera.enabled = false;
        gameState = GameState.Battle;

        PokemonParty pkmParty = playerController.GetComponent<PokemonParty>();
        Pokemon wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetWildPokemon();
        
        battleSystem.StartBattle(pkmParty, wildPokemon);
    }

    private void FinishBattle(bool isPlayerWin)
    {
        if (isPlayerWin)
        {
            Debug.Log("YOU WON!!!");
        }
        else
        {
            Debug.Log("You loose");
        }
        gameState = GameState.Adventure;
        battleStage.SetActive(false);
        mainCamera.enabled = true;
    }

    private void Update()
    {
        // Handles the game state
        if (gameState == GameState.Adventure)
        {
            playerController.HandleUpdate();
        }
        else if (gameState == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
