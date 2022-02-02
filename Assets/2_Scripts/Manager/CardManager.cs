﻿using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public int index;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale, int index)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
        this.index = index;
    }
}

public class CardManager : SingletonMonoDestroy<CardManager>
{
    public List<CardObject> cards;
    public List<Card> deck = new List<Card>();

    [Header("카드 덱 위치")]
    [SerializeField] private Transform cardLeft;
    [SerializeField] private Transform cardRight;

    [Header("카드 드로우 위치")]
    [SerializeField] private Transform drawPos;

    [Header("max카드 수")]
    [SerializeField] private int maxCard = 7;

    [Header("프리팹")]
    [SerializeField] private GameObject cardPrefab;

    private List<Card> _cardBuffer = new List<Card>();


    private void Start()
    {
        deck.Add(new Cards.Strike());
        deck.Add(new Cards.Strike());
        deck.Add(new Cards.Strike());
        deck.Add(new Cards.Strike());
        deck.Add(new Cards.Defend());
        deck.Add(new Cards.Defend());
        deck.Add(new Cards.Defend());
        deck.Add(new Cards.Defend());

        SetupCard();
    }

    //카드 뽑기
    public void DrawCard()
    {
        if (cards.Count >= maxCard) return;

        GameObject cardObject = Instantiate(cardPrefab, drawPos.position, Quaternion.identity);

        if (cardObject.TryGetComponent(out CardObject card))
        {
            card.Setup(PopCard());
        }
        
        CardAlignment();
    }

    private Card PopCard()
    {
        if (_cardBuffer.Count <= 0)
        {
            SetupCard();
        }

        Card temp = _cardBuffer[0];
        _cardBuffer.RemoveAt(0);
        return temp;
    }

    //카드 뽑을때 카드 댁 랜덤 셔플
    void SetupCard()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card card = deck[i];
            _cardBuffer.Add(card);
        }

        for (int i = 0; i < _cardBuffer.Count - 1; i++)
        {
            int rand = Random.Range(i, _cardBuffer.Count);
            (_cardBuffer[i], _cardBuffer[rand]) = (_cardBuffer[rand], _cardBuffer[i]);
        }
    }

    // 정렬하는 함수
    public void CardAlignment()
    {
        // print("CardAlignment");
        var originCardRPS = RondAlignment(cardLeft, cardRight, cards.Count, -0.5f, Vector3.one);

        for (var i = 0; i < cards.Count; i++)
        {
            var targetCard = cards[i];

            targetCard.originRPS = originCardRPS[i];
            targetCard.Alignment();
        }
    }

    //카드 정렬하는 함수
    private List<PRS> RondAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        //카드 로테이션
        switch (objCount)
        {
            case 1:
                objLerps = new float[] {0.5f};
                break;
            case 2:
                objLerps = new float[] {0.27f, 0.73f};
                break;
            case 3:
                objLerps = new float[] {0.1f, 0.5f, 0.9f};
                break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        //카드 위치
        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            Quaternion targetRot = Quaternion.identity;

            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? -curve : curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }

            results.Add(new PRS(targetPos, targetRot, scale, i));
        }

        return results;
    }
}