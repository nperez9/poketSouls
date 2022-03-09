using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] private List<Pokemon> wildPokemons;

    public Pokemon GetWildPokemon()
    {   
        if (wildPokemons.Count < 1)
        {
            return null;
        }
        
        Pokemon wildPkm = wildPokemons[Random.Range(0, wildPokemons.Count)];
        wildPkm.Init();
        return wildPkm;
    }
}
