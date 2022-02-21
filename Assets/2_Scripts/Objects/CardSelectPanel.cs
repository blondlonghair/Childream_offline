using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardSelectPanel : MonoBehaviour
{
    [SerializeField] private List<Button> cardSelectButtons;
    [SerializeField] private Sprite[] cardSprite;
    private Card[] cardInfo = new Card[3];

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            cardInfo[i] = RandomCard();
            cardSelectButtons[i].image.sprite = cardSprite[cardInfo[i].id - 1];

            var i1 = i;
            cardSelectButtons[i].onClick.AddListener(delegate
            {
                // cardSelectButtons.ForEach(button1 => button1.gameObject.SetActive(false));
                CardManager.Instance.AddCard(cardInfo[i1]);
                GameManager.Instance.NextStage();
            });

            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerEnter;
            entry1.callback.AddListener( (eventData) =>
            {
                cardSelectButtons[i1].transform.localScale = new Vector3(2, 2, 2);
                cardSelectButtons[i1].transform.SetAsLastSibling();
            });
            
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener((eventData) =>
            {
                cardSelectButtons[i1].transform.localScale = new Vector3(1, 1, 1);
            });
            
            cardSelectButtons[i].GetComponent<EventTrigger>().triggers.Add(entry1);
            cardSelectButtons[i].GetComponent<EventTrigger>().triggers.Add(entry2);
        }
    }

    private Card RandomCard()
    {
        return Random.Range(0, 30) switch
        {
            0 => new Strike(),
            1 => new RepeatedHit(),
            2 => new Squash(),
            3 => new DeathFault(),
            4 => new Brimstone(),
            5 => new HeavyBlade(),
            6 => new Wave(),
            7 => new PommelStrike(),
            8 => new Voltage(),
            9 => new Clothesline(),
            10 => new Dropkick(),
            11 => new SoulCutter(),
            12 => new Upercut(),
            13 => new Bash(),
            14 => new Defend(),
            15 => new ShrugItOff(),
            16 => new Grit(),
            17 => new Duplication(),
            18 => new Sacrifice(),
            19 => new Move(),
            20 => new BloodLetting(),
            21 => new Disarm(),
            22 => new Fear(),
            23 => new Adrenalin(),
            24 => new Adjustment(),
            25 => new Inflame(),
            26 => new Contract(),
            27 => new Hemostasis(),
            28 => new Blind(),
            29 => new Trip()
        };
    }

    public void NextStage()
    {
        GameManager.Instance.NextStage();
    }

    public void SizeUp()
    {
        transform.localScale = new Vector3(2, 2, 2);
    }
}