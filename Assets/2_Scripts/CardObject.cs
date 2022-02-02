using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
{
    public PRS originRPS;

    [Header("카드 정보")] 
    public int id;
    public string cardName;
    public int cost;
    [TextArea(5, 10)] public string cardDesc;
    public Sprite cardImage;
    public Sprite cardImageBG;

    [Header("카드 요소")] 
    [SerializeField] TextMeshPro nameText;
    [SerializeField] TextMeshPro costText;
    [SerializeField] TextMeshPro descText;
    [SerializeField] SpriteRenderer CardImage;
    [SerializeField] SpriteRenderer CardImageBG;
    
    public void Setup(Card card)
    {
        id = card.id;
        cardName = card.name;
        cost = card.cost;
        cardDesc = card.desc;
        // cardImage = card.cardImage;
        // cardImageBG = card.cardImageBG;

        nameText.text = cardName;
        costText.text = cost.ToString();
        descText.text = cardDesc;
        // CardImage.sprite = cardImage;
        // CardImageBG.sprite = cardImageBG;
    }

    private void OnEnable()
    {
        CardManager.Instance.cards.Add(this);
    }

    private void OnDestroy()
    {
        CardManager.Instance.cards.Remove(this);
    }

    public void Alignment()
    {
        StartCoroutine(Co_Alignment());
    }

    private IEnumerator Co_Alignment()
    {
        while (!Mathf.Approximately(transform.position.x, originRPS.pos.x))
        {
            transform.position = Vector3.Lerp(transform.position, originRPS.pos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRPS.rot, 0.2f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.2f);

            yield return null;
        }
    }
}