using UnityEngine;
using cardScripts;
using System.Collections.Generic;
using System.Diagnostics;

public class deckManager : MonoBehaviour
{
    public Stack<Card> cards = new Stack<Card>();
    public List<Card> discards = new List<Card>();

    [SerializeField] private int startingHandSize = 5;


    [Header("Dev Cards List View")]
    [SerializeField] private bool showDebugView = true;
    [SerializeField] private List<Card> debugDrawPile = new List<Card>();
    [SerializeField] private List<Card> debugDiscardPile = new List<Card>();

    public int drawPileQuant => cards.Count;
    public int discardPileQuant => discards.Count;

    void Start()
    {
        if (Application.isBatchMode || !Application.isPlaying) return;
        // Initialize the deck with all cards
        Card[] loaded = Resources.LoadAll<Card>("cardData");
        buildDrawPile(new List<Card>(loaded));

        handManager hand = FindAnyObjectByType<handManager>();
        for (int i = 0; i < startingHandSize; i++)
        {
            drawCard(hand);
        }
    }

    public Card drawCard(handManager hand)
    {
        if (cards.Count == 0)
        {
            if (discards.Count == 0) return null;
            buildDrawPile(discards);
            discards.Clear();
        } 

        Card newCard = cards.Pop();
        hand.addCardToHand(newCard);

        refreshDebug();
        return newCard;
    }

    public Card peekTop()
    {
        return cards.Count > 0 ? cards.Peek() : null;
    }

    public void discard(Card card)
    {
        discards.Add(card);
        refreshDebug();
    }

    private void buildDrawPile(List<Card> cardList)
    {
        shuffle(cardList);
        cards = new Stack<Card>(cardList);
        refreshDebug();
    }

    private void shuffle(List<Card> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card tempCard = deck[i];
            deck[i] = deck[j];
            deck[j] = tempCard;
        }
    }

    private void refreshDebug()
    {
        if (!showDebugView)
        {
            debugDrawPile.Clear();
            debugDiscardPile.Clear();
            return;
        }

        debugDrawPile = new List<Card>(cards);
        debugDiscardPile = new List<Card>(discards);
    }


    // debug
    [ContextMenu("Show Draw Pile")]
    private void showDrawPile()
    {
        int i = 0;
        foreach (Card c in cards)
            UnityEngine.Debug.Log($"{i++}: {c.cardName}");
    }
}
