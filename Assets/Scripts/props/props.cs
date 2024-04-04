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

    public Player player;
    [Header("��Ҫ����")]
    public LayerMask floorLayerMask;
    public LayerMask foodLayerMask;
    public GameObject mainSprite;
    public GameObject shadow;
    public FoodKind foodKind;
    public bool isMoving;
    public Vector2 startMovingPos;
    public Vector2 endMovingPos;
    // public float movingCurrentDistance;
    // public float movingTargetDistance;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        startPos = mainSprite.transform.localPosition;
        startScale = shadow.transform.localScale;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            PushingObjectWhileMoving();
        }
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
        isMoving = true;
        StartCoroutine(FallingProgress());
    }

    #region �����ƶ��ƶ����
    public void BeingPushedToPoint(Vector2 point)
    {
        isMoving = true;
        StartCoroutine(MovingProgress(point));
    }

    IEnumerator MovingProgress(Vector2 endPoint)
    {
        Vector2 startPos = transform.position;
        Debug.Log("�ƶ�����");
        startMovingPos = startPos;
        endMovingPos = endPoint;
        while ((Vector2)transform.position != endPoint)
        {
            // ��ȡ���Ƶ��ƶ���ʽ
            
            // ���㵱ǰ֡���ƶ�����
            float step = 20f * Time.deltaTime;
            // �ƶ�������Ŀ��λ��
            transform.position = Vector2.MoveTowards(transform.position, endPoint, step);

            // �ȴ���һ֡
            yield return null;
        }

        isMoving = false;

        //if(!CheckFloorBelowFoot())
        //{
        //    player.FoodDisappearFailureStart();
        //}
        
    }

    // �ƶ�·�ϵ����
    private void PushingObjectWhileMoving()
    {
        Collider2D collider;
        Vector2 direction = (endMovingPos - startMovingPos).normalized;
        collider = Physics2D.OverlapPoint((Vector2)transform.position + 0.5f * direction, foodLayerMask);
        if(collider != null)
        {
            if (collider.GetComponent<props>().isMoving == false)
            {
                collider.GetComponent<props>().BeingPushedToPoint(endMovingPos + direction);
            }
        }

    }
    #endregion

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
