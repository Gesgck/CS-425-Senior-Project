using UnityEngine;
using cardScripts;
using System.Collections.Generic;

public class deckManager : MonoBehaviour
{
    public List<Card> cards = new List<Card>();

    private int currentCardIndex = 0;

    void Start()
    {
        // Initialize the deck with all cards
        Card[] cards = Resources.LoadAll<Card>("cardData");
        this.cards.AddRange(cards);

        handManager hand = FindObjectOfType<handManager>();
        for (int i = 0; i < 5; i++)
        {
            drawCard(hand);
        }
    }

    public Card drawCard(handManager handManager)
    {
        if (cards.Count == 0)
        {
            return null;
        } 

        Card newCard = cards[currentCardIndex];
        handManager.addCardToHand(newCard);
        currentCardIndex = (currentCardIndex + 1) % cards.Count;
        return newCard;
    }
}
