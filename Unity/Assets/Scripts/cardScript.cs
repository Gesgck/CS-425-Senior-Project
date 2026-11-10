using UnityEngine;

namespace cardScripts
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public CardType cardType;
        public int cardCost;
        public int cardHealth;
        public int cardDmgMin;
        public int cardDmgMax;
        public DamageType damageType;
        public Sprite cardSprite;


        public enum CardType
        {
            Attack,
            Support,
            Buff
        }
        public enum DamageType
        {
            Fire
        }
    }
}