using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
{
    public PRS originRPS;
    public Card originCard;

    private Coroutine _coroutine;

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

    // public void Destroy()
    // {
    //     if (_coroutine != null)
    //     {
    //         StopCoroutine(_coroutine);
    //     }
    //
    //     // await AsyncDestroy();
    //
    //     _coroutine = StartCoroutine(Co_Destroy());
    //     CardManager.Instance.cards.Remove(this);
    // }

    // private async Task AsyncDestroy()
    // {
    //     while (!Helper.Approximately(transform, CardManager.Instance.destroyPos))
    //     {
    //         transform.position =
    //             Vector3.Lerp(transform.position, CardManager.Instance.destroyPos.position, Time.deltaTime);
    //     }
    //     
    //     Destroy(gameObject);
    // }

    public void Alignment()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(Co_Alignment());
        OrderInLayer(originRPS.index);
    }

    // private IEnumerator Co_Destroy()
    // {
    //     while (!Helper.Approximately(transform, CardManager.Instance.destroyPos))
    //     {
    //         transform.position =
    //             Vector3.Lerp(transform.position, CardManager.Instance.destroyPos.position, 0.2f);
    //         yield return YieldCache.WaitForSeconds(0.01f);
    //     }
    //     
    //     Destroy(gameObject);
    //     yield return null;
    // }

    private IEnumerator Co_Alignment()
    {
        while (!Helper.Approximately(transform, originRPS))
        {
            transform.position = Vector3.Lerp(transform.position, originRPS.pos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRPS.rot, 0.2f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.2f);

            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }

    public void CardZoomIn()
    {
        print("zoomin");

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        OrderInLayer(100);
        transform.localScale = new Vector3(2, 2, 2);
        transform.position = new Vector3(transform.position.x, -5, -9);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void CardZoomOut()
    {
        print("zoomout");
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        OrderInLayer(originRPS.index);
        transform.localScale = Vector3.one;
        transform.position = originRPS.pos;
        transform.rotation = originRPS.rot;
    }
    
    private void OrderInLayer(int index)
    {
        nameText.sortingOrder = index * 2;
        costText.sortingOrder = index * 2;
        descText.sortingOrder = index * 2;
        CardImage.sortingOrder = index * 2 - 1;
        CardImageBG.sortingOrder = index * 2 - 1;
    }
}