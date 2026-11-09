using UnityEngine;
using cardScripts;
using System.Collections.Generic;



public class handManager : MonoBehaviour
{
    public deckManager deckManager;
    public GameObject cardPrefab;
    public Transform handPosition;
    public float fanSpread = 7.5f;
    public float horCardSpace = -118f;
    public float vertCardSpace = 38f;
    public List<GameObject> hand = new List<GameObject>(); // list of cards in hand
    void Start()
    {

    }

    public void addCardToHand(Card card)
    {
        GameObject newCard = Instantiate(cardPrefab, handPosition.position, Quaternion.identity, handPosition);
        hand.Add(newCard);

        newCard.GetComponent<cardDisplay>().cardData = card;

        updateHandLayout();
    } 

    void Update()
    {
        //updateHandLayout();
    }

    private void updateHandLayout()
    {
        int cardQuantity = hand.Count;

        if (cardQuantity == 1)
        {
            hand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            hand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            return;
        }

        for (int i = 0; i < cardQuantity; i++)
        {
            float rot = (i - (cardQuantity - 1) / 2f) * fanSpread;
            hand[i].transform.localPosition = handPosition.localPosition + Quaternion.Euler(0, 0, rot) * Vector3.up * 2f;
            hand[i].transform.localRotation = Quaternion.Euler(0, 0, rot);

            float horizontalOffset = (i - (cardQuantity - 1) / 2f) * horCardSpace;

            float normalVertPos = (2f * i / (cardQuantity - 1)) - 1f;
            float verticalOffset = (1 - normalVertPos * normalVertPos) * vertCardSpace;
            hand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
