using UnityEngine;
using cardScripts;
using UnityEngine.UI;
using TMPro;

public class cardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image cardImage;
    public TMP_Text cardName;
    public TMP_Text health;
    public TMP_Text damage;
    public Image type;

    void Start()
    {
        updateCardDisplay();
    }

    void updateCardDisplay()
    {
       cardName.text = cardData.cardName;
       health.text = cardData.cardHealth.ToString();
       damage.text = $"{cardData.cardDmgMin}-{cardData.cardDmgMax}";

    }
}
