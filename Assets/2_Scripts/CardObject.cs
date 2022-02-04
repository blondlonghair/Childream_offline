using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
{
    public PRS originRPS;
    public Card originCard;

    [Header("카드 정보")] 
    public int id;
    public string cardName;
    public int cost;
    [TextArea(5, 10)] public string cardDesc;
    public Sprite cardImage;
    public Sprite cardBG;

    [Header("카드 요소")] 
    [SerializeField] TextMeshPro nameText;
    [SerializeField] TextMeshPro costText;
    [SerializeField] TextMeshPro descText;
    [SerializeField] SpriteRenderer CardImage;
    [SerializeField] SpriteRenderer CardImageBG;
    
    public void Setup(Card card)
    {
        originCard = card;
        
        id = card.id;
        cardName = card.name;
        cost = card.cost;
        cardDesc = card.desc;
        // cardImage = card.cardImage;
        // cardBG = card.cardImageBG;

        nameText.text = cardName;
        costText.text = cost.ToString();
        descText.text = cardDesc;
        // CardImage.sprite = cardImage;
        // CardBG.sprite = cardImageBG;
    }

    private void OnEnable()
    {
        CardManager.Instance.cards.Add(this);
    }

    public void Destroy()
    {
        CardManager.Instance.cards.Remove(this);
        Destroy(gameObject);
    }

    public void Alignment()
    {
        StartCoroutine(Co_Alignment());
    }

    private IEnumerator Co_Alignment()
    {
        while (!(Mathf.Approximately(transform.position.x, originRPS.pos.x) && 
               Mathf.Approximately(transform.rotation.z, originRPS.rot.z) && 
               Mathf.Approximately(transform.localScale.x, originRPS.scale.x)))
        {
            print($"{gameObject.name} Lerping");
            transform.position = Vector3.Lerp(transform.position, originRPS.pos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRPS.rot, 0.2f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.2f);

            yield return null;
        }
    }

    public void CardZoomIn()
    {
        OrderInLayer(100);
        transform.localScale = new Vector3(2, 2, 2);
        transform.position = new Vector3(transform.position.x, -5, -9);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void CardZoomOut()
    {
        OrderInLayer(originRPS.index);
        transform.localScale = Vector3.one;
        transform.position = originRPS.pos;
        transform.rotation = originRPS.rot;
    }
    
    private void OrderInLayer(int index)
    {
        nameText.sortingOrder = index;
        costText.sortingOrder = index;
        descText.sortingOrder = index;
        CardImage.sortingOrder = index - 1;
        CardImageBG.sortingOrder = index - 1;
    }
}