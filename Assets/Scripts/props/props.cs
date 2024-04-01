using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class props : MonoBehaviour
{
    private float amplitude = 0.15f; // ���������
    private float frequency = 1f;
    private float v = 1f;
    private Vector2 startPos;
    private Vector2 startScale;

    [Header("��Ҫ����")]
    public LayerMask floorLayerMask;
    public GameObject mainSprite;
    public GameObject shadow;
    public FoodKind foodKind;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        startPos = mainSprite.transform.localPosition;
        startScale = shadow.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool CheckFloorBelowFoot()
    {
        Collider2D collider;
        collider = Physics2D.OverlapPoint(transform.position, floorLayerMask);
        if(collider != null)
        {
            return true;
        }
        else
        {
            Debug.Log("ʳ����¿յ�");
            return false;
        }
    }

    public void FoodFallingFailure()
    {
        StartCoroutine(FallingProgress());
    }

    IEnumerator FallingProgress()
    {
        yield return new WaitForSeconds(0.5f);
        mainSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Back";
        Debug.Log("�������");
        while (transform.position.y >= -50f)
        {
            // ���㵱ǰ֡���ƶ�����
            float step = 40f * Time.deltaTime;

            // �ƶ�������Ŀ��λ��
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position - new Vector2(0, 50f + transform.position.y), step);

            // �ȴ���һ֡
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        floatingAnimation();
    }

    private void floatingAnimation()
    {
        float Offset = Mathf.Sin(Time.time * frequency) * amplitude;

        Vector2 curPos = startPos + new Vector2(0, Offset+0.25f);
        Vector2 curScale = startScale + new Vector2(-Offset+0.5f, -Offset + 0.5f);

        mainSprite.transform.localPosition = Vector2.Lerp(mainSprite.transform.localPosition, curPos, Time.deltaTime * v);
        shadow.transform.localScale = Vector2.Lerp(shadow.transform.localScale, curScale, Time.deltaTime * v);
    }
}
