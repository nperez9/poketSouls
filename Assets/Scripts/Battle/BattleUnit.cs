using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Manage the pokemon sprite of the battle
namespace Battle { 
    public class BattleUnit : MonoBehaviour
    {
        [SerializeField] PoketSoulBase poketSoulBase;
        [SerializeField] int level = 0;
        // Defines the sprite to use
        [SerializeField] bool isPlayerUnit;

        public Pokemon Pokemon { set; get; }
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();    
        }

        public void Setup()
        {
            Pokemon = new Pokemon(poketSoulBase, level);
            image.sprite = isPlayerUnit ? Pokemon.Base.BackSprite : Pokemon.Base.FrontSprite;
        }
    }
}