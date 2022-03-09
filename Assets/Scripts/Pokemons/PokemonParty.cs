using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace PokemonN { 
    public class PokemonParty : MonoBehaviour
    {
        [SerializeField] private List<Pokemon> pokemonParty;

        private void Start()
        {
            pokemonParty.ForEach(pkm => {
                pkm.Init();
            });
        }

        public Pokemon GetFirstHealtyPokemon()
        {
            return pokemonParty.Where(pokemon => pokemon.HP > 0).FirstOrDefault();
        }
    }
}
