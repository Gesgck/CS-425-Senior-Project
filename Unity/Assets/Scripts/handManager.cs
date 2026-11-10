using UnityEngine;
using cardScripts;
using System.Collections.Generic;



public class handManager : MonoBehaviour
{
    public deckManager deckManager;
    public GameObject cardPrefab;
    public Transform handPosition;
    public float fanSpread = -7.5f;
    public float horCardSpace = 118f;
    public float vertCardSpace = 38f;
    public List<GameObject> hand = new List<GameObject>(); // list of cards in hand

    [HideInInspector] public GameObject draggedCard;

    public void addCardToHand(Card card)
    {
        GameObject newCard = Instantiate(cardPrefab, handPosition.position, Quaternion.identity, handPosition);
        hand.Add(newCard);

        newCard.GetComponent<cardDisplay>().cardData = card;

        updateHandLayout();
    } 

    public void removeCardFromHand(Card card)
    {
        GameObject cardObject = hand.Find(cardInHand => cardInHand.GetComponent<cardDisplay>().cardData == card);
        if (cardObject == null) return;

        cardDisplay display = cardObject.GetComponent<cardDisplay>();
        if (display != null && display.cardData != null) deckManager.discard(display.cardData);

        hand.Remove(cardObject);
        if (Application.isPlaying)
        {
            Destroy(cardObject);
        } else
        {
            DestroyImmediate(cardObject);
        }
        updateHandLayout();

    }

     private float slotX(int i, int n)
    {
        return (i - (n - 1) / 2f) * horCardSpace;
    }

    public void UpdateDragOrder(GameObject card)
    {
        int current = hand.IndexOf(card);
        if (current < 0) return;

        int n = hand.Count;
        float x = card.transform.localPosition.x;

        int best = current;
        float bestDist = float.MaxValue;
        if (hand == null) return;
        for (int i = 0; i < n; i++)
        {
            float d = Mathf.Abs(x - slotX(i, n));
            if (d < bestDist)
            {
                bestDist = d;
                best = i;
            }
        }

        if (best != current)
        {
            hand.RemoveAt(current);
            hand.Insert(best, card);
            updateHandLayout();
        }
    }

    public void updateHandLayout()
    {
        int n = hand.Count;

        for (int i = 0; i < n; i++)
        {
            GameObject card = hand[i];
            if (card == draggedCard) continue; // follows the pointer instead

            float rot = (i - (n - 1) / 2f) * fanSpread;
            float t = n > 1 ? (2f * i / (n - 1)) - 1f : 0f;
            float vert = n > 1 ? (1 - t * t) * vertCardSpace : 0f;

            Vector3 pos = new Vector3(slotX(i, n), vert, 0f);
            Quaternion rotation = n > 1 ? Quaternion.Euler(0, 0, rot) : Quaternion.identity;

            card.GetComponent<cardMovement>().setTarget(pos, rotation);
        }

        refreshLayers();
    }

    public void refreshLayers()
    {
        int cardIndex = 0;
        for (int priority = 0; priority <= 3; priority++)
        {
            foreach (GameObject card in hand)
            {
                cardMovement mv = card.GetComponent<cardMovement>();
                int p = mv != null ? mv.layerPriority : 0;
                if (p != priority) continue;

                card.transform.SetSiblingIndex(cardIndex++);
            }
        }
    }
}
