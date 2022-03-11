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
    private Action listener;

    [Header("카드 정보")] 
    public int id;
    public string cardName;
    public int cost;
    [TextArea(5, 10)] public string cardDesc;
    public Sprite cardImage;
    public Sprite cardBG;

    [Header("카드 요소")] 
    [SerializeField] private GameObject cardRenderer;
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
        cardImage = card.sprite;
        cardBG = card.backGround;

        nameText.text = cardName;
        costText.text = cost.ToString();
        descText.text = cardDesc;
        CardImage.sprite = cardImage;
        CardImageBG.sprite = cardBG;
    }

    private void OnEnable()
    {
        CardManager.Instance.cards.Add(this);
    }

    public void Alignment()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Co_Alignment());
        OrderInLayer(originRPS.index);
    }

    private IEnumerator Co_Alignment()
    {
        while (!Helper.Approximately(transform, originRPS))
        {
            transform.position = Vector3.Lerp(transform.position, originRPS.pos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRPS.rot, 0.2f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.2f);

            yield return YieldCache.WaitForSeconds(0.01f);
        }

        // cardRenderer.transform.localPosition = Vector3.zero;
        // cardRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        // cardRenderer.transform.localScale = Vector3.one;
        // yield return null;
    }
    
    public void CardZoomIn()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        OrderInLayer(100);
        _coroutine = StartCoroutine(Co_Zoom(new Vector3(0, -3, -9), Quaternion.Euler(0, 0, 0), new Vector3(2, 2, 1)));

        // var hashCode = Animator.StringToHash("SizeUp");
        // _animator.SetTrigger(hashCode);
    }

    public void CardZoomOut()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        OrderInLayer(originRPS.index);
        _coroutine = StartCoroutine(Co_Zoom(originRPS.pos, originRPS.rot, originRPS.scale));

        // transform.position = cardRenderer.transform.position;
        // transform.rotation = cardRenderer.transform.rotation;
        // transform.localScale = cardRenderer.transform.localScale;
    }

    private IEnumerator Co_Zoom(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Vector3 pos = cardRenderer.transform.position;
        Quaternion rot = cardRenderer.transform.rotation;
        Vector3 sca = cardRenderer.transform.localScale;
        
        transform.position = originRPS.pos;
        transform.rotation = originRPS.rot;
        transform.localScale = originRPS.scale;
        
        cardRenderer.transform.position = pos;
        cardRenderer.transform.rotation = rot;
        cardRenderer.transform.localScale = sca;
        
        while (!Helper.Approximately(cardRenderer.transform, new PRS(position, rotation, scale, 0)))
        {
            cardRenderer.transform.position = Vector3.Lerp(cardRenderer.transform.position, position, 0.2f);
            cardRenderer.transform.rotation = Quaternion.Lerp(cardRenderer.transform.rotation, rotation, 0.2f);
            cardRenderer.transform.localScale = Vector3.Lerp(cardRenderer.transform.localScale, scale, 0.2f);

            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }

    public void StopCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        cardRenderer.transform.localPosition = Vector3.zero;
        cardRenderer.transform.localRotation = Quaternion.Euler(0,0,0);
        cardRenderer.transform.localScale = Vector3.one;
    }
    
    private void OrderInLayer(int index)
    {
        nameText.sortingOrder = index * 2;
        costText.sortingOrder = index * 2;
        descText.sortingOrder = index * 2;
        CardImage.sortingOrder = index * 2 - 1;
        CardImageBG.sortingOrder = index * 2 - 1;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        listener?.Invoke();
    }

    public void AddCollisionListener(Action action)
    {
        listener = action;
    }
}