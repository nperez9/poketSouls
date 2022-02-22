using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

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
        private Vector3 originalPosition;
        private Color originalColor;

        private void Awake()
        {
            image = GetComponent<Image>();
            originalPosition = image.transform.localPosition;
            originalColor = image.color;
        }

        public void Setup()
        {
            Pokemon = new Pokemon(poketSoulBase, level);
            image.sprite = isPlayerUnit ? Pokemon.Base.BackSprite : Pokemon.Base.FrontSprite;
            PlaySetupAnimation();
        }

        private void PlaySetupAnimation() {
            if (isPlayerUnit) {
                image.transform.localPosition = new Vector3(-700f, image.transform.localPosition.y);
            } else {
                image.transform.localPosition = new Vector3(800f, image.transform.localPosition.y);
            }

            image.transform.DOLocalMoveX(originalPosition.x, 2f);
        }

        public void PlayAttackAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            if (isPlayerUnit) {
                sequence.Append(image.transform.DOLocalMoveX(originalPosition.x + 50f, 0.25f));
            } else {
                sequence.Append(image.transform.DOLocalMoveX(originalPosition.x - 50f, 0.25f));
            }

            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x, 0.25f));
        }

        public void PlayHitAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOColor(Color.gray, 0.1f));
            sequence.Append(image.DOColor(originalColor, 0.1f));
        }

        public void PlayFaintAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.transform.DOLocalMoveY(originalPosition.y - 150f, 0.5f));
            sequence.Join(image.DOFade(0f, 0.5f));
        }
    }
}