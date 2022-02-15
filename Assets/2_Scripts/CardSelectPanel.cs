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
            cardSelectButtons[i].image.sprite = cardSprite[cardInfo[i].id - 2];

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
            1 => new Bash(),
            2 => new BodySlam(),
            3 => new Cleave(),
            4 => new Hemokinesis(),
            5 => new HeavyBlade(),
            6 => new IronWave(),
            7 => new PommelStrike(),
            8 => new ThunderClap(),
            9 => new Clothesline(),
            10 => new Dropkick(),
            11 => new ServerSoul(),
            12 => new Upercut(),
            13 => new Bludgeon(),
            14 => new Defend(),
            15 => new ShrugItOff(),
            16 => new TrueGrit(),
            17 => new Entrench(),
            18 => new SecondWind(),
            19 => new Move(),
            20 => new BloodLetting(),
            21 => new Disarm(),
            22 => new Intimidate(),
            23 => new LimitBreak(),
            24 => new Offering(),
            25 => new Inflame(),
            26 => new BurningPact(),
            27 => new BandageUp(),
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