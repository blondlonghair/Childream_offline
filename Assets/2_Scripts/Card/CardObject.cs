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

    [Header("카드 정보")] public int id;
    public string cardName;
    public int cost;
    [TextArea(5, 10)] public string cardDesc;
    public Sprite cardImage;
    public Sprite cardBG;

    [Header("카드 요소")] [SerializeField] private GameObject cardRenderer;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro costText;
    [SerializeField] private TextMeshPro descText;
    [SerializeField] private SpriteRenderer CardImage;
    [SerializeField] private SpriteRenderer CardImageBG;

    [Header("등등")] [SerializeField] private LineRenderer lineRenderer;

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
        _coroutine = StartCoroutine(Co_Zoom(new Vector3(0, -1, -9), Quaternion.Euler(0, 0, 0), new Vector3(2, 2, 1)));
    }

    public void CardZoomOut()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        OrderInLayer(originRPS.index);
        _coroutine = StartCoroutine(Co_Zoom(originRPS.pos, originRPS.rot, originRPS.scale));
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
        cardRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
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

    public void UpdateLine(Vector3 mousePos)
    {
        Vector3 pos = new Vector3(transform.position.x, mousePos.y, 0);

        lineRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);

        for (int i = 1; i < 10; i++)
        {
            var position = transform.position;
            Vector3 p1 = Vector3.Lerp(position - position, pos - position, (float) i / 10);
            Vector3 p2 = Vector3.Lerp(pos - position, mousePos - position, (float) i / 10);

            lineRenderer.SetPosition(i, Vector3.Lerp(p1, p2, (float) i / 10));
        }
    }

    public void CloseLine()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, Vector3.zero);
        }
        
        // StartCoroutine(Co_CloseLine(mousePos));
    }

    private IEnumerator Co_CloseLine(Vector3 mousePos)
    {
        lineRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);

        for (int i = 10; i >= 1; i--)
        {
            var position = Vector3.Lerp(transform.position, mousePos, (float)i / 10);

            for (int j = 1; j < 10; j++)
            {
                Vector3 pos = new Vector3(position.x, position.y, 0);
                Vector3 p1 = Vector3.Lerp(position - position, pos - position, (float) j / 10);
                Vector3 p2 = Vector3.Lerp(pos - position, mousePos - position, (float) j / 10);

                lineRenderer.SetPosition(j, Vector3.Lerp(p1, p2, (float) j / 10));
            }

            yield return YieldCache.WaitForSeconds(0.01f);
        }
        
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, Vector3.zero);
        }

        yield return null;
    }
}