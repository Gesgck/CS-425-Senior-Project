using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using cardScripts;

public class cardSystemTests
{
    private GameObject deckObject;
    private deckManager deck;
    private GameObject handObject;
    private handManager hand;

    [SetUp]
    public void setup()
    {
        deckObject = new GameObject();
        deck = deckObject.AddComponent<deckManager>();

        handObject = new GameObject();
        hand = handObject.AddComponent<handManager>();

        hand.cardPrefab = new GameObject();

        GameObject tempPos = new GameObject();
        tempPos.transform.SetParent(handObject.transform);
        hand.handPosition = tempPos.transform;
        
        hand.cardPrefab.AddComponent<cardDisplay>();
        hand.cardPrefab.AddComponent<cardMovement>();

        deck.cards.Clear();
        deck.discards.Clear();
        hand.hand.Clear();

    }
    
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(deckObject);
        Object.DestroyImmediate(handObject);
    }

    [Test]
    public void drawCardFromEmptyDeckReturnsNull()
    {
        var result = deck.drawCard(hand);
        Assert.IsNull(result);
    }

    [Test]
    public void drawCardFromNonEmptyDeckReturnsCard()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        deck.cards.Push(card);

        var result = deck.drawCard(hand);
        Assert.AreEqual(card, result);
    }

    [Test]
    public void drawCardFromEmptyDeckReshufflesCards()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        deck.discards.Add(card);

        var result = deck.drawCard(hand);
        Assert.AreEqual(card, result);
        Assert.AreEqual(0, deck.discards.Count);
    }

    [Test]
    public void discardCardAddsCardToDiscardPile()
    {
        var card = ScriptableObject.CreateInstance<Card>();

        deck.discard(card);

        Assert.Contains(card, deck.discards);
    }

    [Test]
    public void peekTopReturnsCardWithoutRemovingIt()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        deck.cards.Push(card);

        var result = deck.peekTop();
        Assert.AreEqual(card, result);
        Assert.AreEqual(card, deck.cards.Peek());
    }

    [Test]
    public void peekTopReturnsNullWhenDeckIsEmpty()
    {
        var result = deck.peekTop();
        Assert.IsNull(result);
    }

    [Test]
    public void addingCardToHandIncreasesHandCount()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        int initialCount = hand.hand.Count;

        hand.addCardToHand(card);

        Assert.AreEqual(initialCount + 1, hand.hand.Count);
    }

    [Test]
    public void singleCardIsCentered()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        hand.addCardToHand(card);

        Assert.AreEqual(0f, hand.hand[0].transform.position.x, 0.01f);
        Assert.AreEqual(0f, hand.hand[0].transform.position.y, 0.01f);
        Assert.AreEqual(1, hand.hand.Count);
    }

    [Test]
    public void multipleCardsAreSpreadOut()
    {
        var card1 = ScriptableObject.CreateInstance<Card>();
        var card2 = ScriptableObject.CreateInstance<Card>();
        
        if (hand.cardPrefab.GetComponent<RectTransform>() == null)
        {
            hand.cardPrefab.AddComponent<RectTransform>();
        }

        hand.addCardToHand(card1);
        hand.addCardToHand(card2);

        var movement1 = hand.hand[0].GetComponent<cardMovement>();
        var movement2 = hand.hand[1].GetComponent<cardMovement>();

        Assert.AreNotEqual(movement1.targetPos.x, movement2.targetPos.x);
        Assert.AreEqual(2, hand.hand.Count);
    }

    [Test]
    public void draggingCardChangesHandOrder()
     {
        var card1 = ScriptableObject.CreateInstance<Card>();
        var card2 = ScriptableObject.CreateInstance<Card>();
        
        if (hand.cardPrefab.GetComponent<RectTransform>() == null)
        {
            hand.cardPrefab.AddComponent<RectTransform>();
        }

        hand.addCardToHand(card1);
        hand.addCardToHand(card2);

        GameObject cardObj1 = hand.hand[0];
        GameObject cardObj2 = hand.hand[1];

        cardObj1.transform.localPosition = new Vector3(59f, 0f, 0f);

        hand.UpdateDragOrder(cardObj1);

        Assert.AreEqual(cardObj2, hand.hand[0]);
        Assert.AreEqual(cardObj1, hand.hand[1]);
    }

    [Test]
    public void invalidDraggedCardDoesNothing()
    {
        var card = ScriptableObject.CreateInstance<Card>();
        hand.addCardToHand(card);

        GameObject evilCard = new GameObject();
        hand.UpdateDragOrder(evilCard);

        Object.DestroyImmediate(evilCard);

        Assert.AreEqual(1, hand.hand.Count);

    }

    [Test]
    public void removingCardFromHandDecreasesHandCount()
    {
        hand.deckManager = deck;
        
        var card = ScriptableObject.CreateInstance<Card>();
        hand.addCardToHand(card);
        int initialCount = hand.hand.Count;

        Assert.AreEqual(1, hand.hand.Count);
        hand.removeCardFromHand(card);
        Assert.Contains(card, deck.discards);
        Assert.AreEqual(initialCount - 1, hand.hand.Count);
    }
}