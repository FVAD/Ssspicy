using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControl playerControl;
    private Rigidbody2D rb;
    // private bool GameIsOver;
    public GameOverState overState;

    public List<Sprite> playerHeadSprite;
    public List<Sprite> playerBodySprite;
    public List<Sprite> playerTailSprite;
    
    public GameObject playerBody;
    public GameObject playerHead;
    public GameObject playerTail;
    [Header("��Ļ�ڱ�Ե����")]
    public float leftX;
    public float rightX;
    public float upY;
    public float downY;


    [Header("�������")]
    public Vector2 currentHeadDirection;
    public int playerLength = 5; // ����������Ӧ���ɹؿ�����
    public List<PlayerBodyData> playerBodyDataList = new();
    [Header("��Ϸ����")]
    public LayerMask obstacleLayerMask; // �ϰ������ڵ�ͼ��
    public LayerMask foodLayerMask; // ʳ�����ڵ�ͼ��
    public LayerMask floorLayerMask; // �ذ����ڵ�ͼ��
    public GameObject gameOverUI; // ʧ�ܽ���ҳ��
    // ���±����Ͻ��ṹ��,��������ṹ�壬position�����ǲ���Ҫ�ģ����ǲ��ų�֮��������������ԵĿ����ԣ����Ǿ���������
    //public List<Vector2> playerBodyPosList = new(3); // Body��λ������������Ҳ�ɹؿ�����
    //public List<GameObject> playerBodyList = new(3-1);

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerBodyData tmpData = new PlayerBodyData();
        
        
        tmpData.body = GameObject.Instantiate(playerBody);
        tmpData.body.transform.SetParent(transform);
        tmpData.body.transform.localPosition = new Vector2(-1, 0);
        playerBodyDataList.Add(tmpData);

        tmpData = new PlayerBodyData();
        tmpData.body = GameObject.Instantiate(playerBody);
        tmpData.body.transform.SetParent(transform);
        tmpData.body.transform.localPosition = new Vector2(-2, 0);
        playerBodyDataList.Add(tmpData);

        tmpData = new PlayerBodyData();
        tmpData.body = GameObject.Instantiate(playerBody);
        tmpData.body.transform.SetParent(transform);
        tmpData.body.transform.localPosition = new Vector2(-3, 0);
        playerBodyDataList.Add(tmpData);

        tmpData = new PlayerBodyData();
        tmpData.body = GameObject.Instantiate(playerBody);
        tmpData.body.transform.SetParent(transform);
        tmpData.body.transform.localPosition = new Vector2(-4, 0);
        playerBodyDataList.Add(tmpData);

        tmpData = new PlayerBodyData();
        tmpData.body = GameObject.Instantiate(playerBody);
        tmpData.body.transform.SetParent(transform);
        tmpData.body.transform.localPosition = new Vector2(-5, 0);
        playerBodyDataList.Add(tmpData);

        playerHead.transform.localPosition = new Vector2(0, 0);
        playerTail.transform.localPosition = new Vector2(-6, 0);

        // �����ǶԽ�ɫλ�õ�һ����ʼ���趨
        playerControl = GetComponent<PlayerControl>();
        rb = GetComponent<Rigidbody2D>();
        // GameIsOver = false;
    }
    void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 screenRUPoint = new Vector2(Screen.width, Screen.height);
        Vector2 worldRUPoint = mainCamera.ScreenToWorldPoint(screenRUPoint);
        rightX = worldRUPoint.x;
        leftX = mainCamera.ScreenToWorldPoint(Vector2.zero).x;
        upY = worldRUPoint.y;
        downY = mainCamera.ScreenToWorldPoint(Vector2.zero).y;


        RefreshPlayerBody();
    }

    #region ���岿��ͼƬ��Ⱦ
    private Sprite GetProperBodySprite(Vector2 prePos, Vector2 curPos, Vector2 nextPos)
    {
        if (prePos.y == curPos.y && nextPos.y == curPos.y)
        {
            return playerBodySprite[3];
        }
        else if (prePos.x == curPos.x && nextPos.x == curPos.x)
        {
            return playerBodySprite[5];
        }
        else if ((prePos.x == curPos.x && nextPos.y == curPos.y && prePos.y > curPos.y && nextPos.x > curPos.x) ||
            (nextPos.x == curPos.x && prePos.y == curPos.y && nextPos.y > curPos.y && prePos.x > curPos.x)) // UR
        {
            // Debug.Log("UR");
            return playerBodySprite[4];
            
        }
        else if((prePos.x == curPos.x && nextPos.y == curPos.y && prePos.y > curPos.y && nextPos.x < curPos.x) ||
            (nextPos.x == curPos.x && prePos.y == curPos.y && nextPos.y > curPos.y && prePos.x < curPos.x)) // UL 
        {
            // Debug.Log("UL");
            return playerBodySprite[1];
        }
        else if ((prePos.x == curPos.x && nextPos.y == curPos.y && prePos.y < curPos.y && nextPos.x < curPos.x) ||
    (nextPos.x == curPos.x && prePos.y == curPos.y && nextPos.y < curPos.y && prePos.x < curPos.x)) // LD 
        {
            // Debug.Log("LD");
            return playerBodySprite[0];
        }
        else if ((prePos.x == curPos.x && nextPos.y == curPos.y && prePos.y < curPos.y && nextPos.x > curPos.x) ||
    (nextPos.x == curPos.x && prePos.y == curPos.y && nextPos.y < curPos.y && prePos.x > curPos.x)) // DR
        {
            // Debug.Log("DR");
            return playerBodySprite[2];
        }
        else
        {
            Debug.LogError("Sprite Error!");
            return null;
        }
    }
    #endregion
    #region ���ϼ��
    public bool CheckObstacleAtPoint(Vector2 point)
    {
        // ���ָ�����Ƿ�����ϰ���
        Collider2D collider = Physics2D.OverlapPoint(point, obstacleLayerMask);

        // ��� collider ��Ϊ null����ʾ�����ϰ������ true�����򷵻� false
        return collider != null;
    }
    #endregion
    #region ʳ���⣨�����������ɴ�ѱ���Ҳ����ʳ��ɣ�
    public bool CheckFoodAtPoint(Vector2 point)
    {
        // ���ָ�����Ƿ����ʳ��
        Collider2D collider = Physics2D.OverlapPoint(point, foodLayerMask);

        // ��� collider ��Ϊ null����ʾ����ʳ����� true�����򷵻� false
        return collider != null;
    }

    private bool CheckFoodCanBePushed(Vector2 position, Vector2 addPosition)
    {
        if(CheckObstacleAtPoint((Vector2)transform.position + position + addPosition) ||
                CheckBodyAtPoint(position + addPosition))
        {
            
            // ����Ҫ������ĺ�����������Ҫ����һ��collider2D
        }
        return false;
    }

    public void PushFoodToDirection(Vector2 direction)
    {
        Vector2 colliderPoint = (Vector2)transform.position + (Vector2)playerHead.transform.localPosition + direction;
        Collider2D collider = Physics2D.OverlapPoint(colliderPoint, foodLayerMask);
        while (collider != null)
        {
            PushEachFood(collider, direction);
            DectectAfterPushing(collider);
            colliderPoint += direction;
            collider = Physics2D.OverlapPoint(colliderPoint, foodLayerMask);
        }
    }

    private void PushEachFood(Collider2D collider,Vector2 direction)
    {
        Debug.Log("�ƶ�");
        collider.transform.localPosition = (Vector2)collider.transform.localPosition + direction;
    }

    private void DectectAfterPushing(Collider2D collider)
    {
        if (collider != null)
        {
            if(collider.GetComponent<props>().CheckFloorBelowFoot())
            {
                Debug.Log("ʳ������");
            }
            else
            {
                collider.GetComponent<props>().FoodFallingFailure();
                FoodDisappearFailureStart();
            }
        }
    }

    public void EatFood(Vector2 point)
    {
        Collider2D collider = Physics2D.OverlapPoint(point, foodLayerMask);
        if (collider != null)
        {
            switch(collider.GetComponent<props>().foodKind)
            {
                case FoodKind.banana:
                    // �Ե��㽶
                    Destroy(collider.gameObject);

                    PlayerBodyData tmpData = new PlayerBodyData();
                    tmpData.body = GameObject.Instantiate(playerBody);
                    tmpData.body.transform.SetParent(transform);
                    tmpData.body.transform.localPosition = playerHead.transform.localPosition;
                    playerBodyDataList.Insert(0, tmpData);
                    playerLength++;

                    playerHead.transform.position = point;
                    RefreshPlayerBody();
                    break;
                case FoodKind.pepper:
                    break;
                default:
                    Debug.LogError("ʳ��û�б�ǩ");
                    break;
            }
            
        }
    }
    #endregion

    #region �����⣨point��LocalPos��
    public bool CheckBodyAtPoint(Vector2 point)
    {
        foreach (var data in playerBodyDataList)
        {
            if ((Vector2)data.body.transform.localPosition == point)
            {
                Debug.Log("��������");
                return true;
            }
        }
        return false;
    }
    #endregion
    #region ��������Ƿ��ڵذ���
    public bool CheckAtleastOneOnFloor()
    {
        Collider2D collider; 
        foreach (var data in playerBodyDataList)
        {
            collider = Physics2D.OverlapPoint((Vector2)data.body.transform.position, floorLayerMask);
            if (collider != null)
            {
                Debug.Log("���ڵذ�");
                return true;
            }
        }
        collider = Physics2D.OverlapPoint((Vector2)playerHead.transform.position, floorLayerMask);
        if (collider != null)
        {
            Debug.Log("���ڵذ�");
            return true;
        }
        collider = Physics2D.OverlapPoint((Vector2)playerTail.transform.position, floorLayerMask);
        if (collider != null)
        {
            Debug.Log("���ڵذ�");
            return true;
        }
        Debug.Log("ȫ������");
        return false;
    }
    #endregion
    #region �ƶ�����
    public void Move(Vector2 addPosition)
    {
        currentHeadDirection = addPosition;
        Vector2 position = (Vector2)playerHead.transform.localPosition + addPosition;
        #region �Ƿ����ƶ�ȷ��
        if(CheckBodyAtPoint(position))
        {
            Debug.Log("�ƶ�ʧ��");
            return;
        }
        // if(CheckObstacleAtPoint((Vector2)transform.position + position + 0.25f * Vector2.up) && CheckObstacleAtPoint((Vector2)transform.position + position - 0.25f * Vector2.up))
        // �����ϰ����ɶ������������Tile��
        if (CheckObstacleAtPoint((Vector2)transform.position + position))
        {
            Debug.Log("�ƶ�ʧ��");
            return;
        }
        #endregion
        #region ʳ��ȷ��
        if(CheckFoodAtPoint((Vector2)transform.position + position))
        {
            if(CheckObstacleAtPoint((Vector2)transform.position + position + addPosition)  || 
                CheckBodyAtPoint(position + addPosition))
            {
                // Debug.Log("�����ϰ���");
                // ����Position���ʳ���ӳ����Ƿ����ϰ�������Լ�������, ����У��ԣ�û�У��ƶ�
                // ��̫�ԣ���������һ��ͱ��ˣ�д��������
                // ˬ�ԣ�
                EatFood((Vector2)transform.position + position);
                return;
            }
            else
            {
                PushFoodToDirection(addPosition);
            }
        }
        #endregion
        #region ��ɫ����ǰ�Ʋ�λ
        PlayerBodyData playerBodyData;
        Vector2 tmpVec = new Vector2(0, 0), tmpVec2;
        for(int i = 0; i < playerLength - 2;i++)
        {
            playerBodyData = playerBodyDataList[i];
            tmpVec2 = playerBodyData.body.transform.localPosition;
            if (i == 0)
            {
                tmpVec = playerBodyData.body.transform.localPosition;
                playerBodyData.body.transform.localPosition = playerHead.transform.localPosition;
            }
            else
            {
                tmpVec2 = playerBodyData.body.transform.localPosition;
                playerBodyData.body.transform.localPosition = tmpVec;
                tmpVec = tmpVec2;
            }
        }
        playerTail.transform.localPosition = tmpVec;
        #endregion

        playerHead.transform.localPosition = position;
        RefreshPlayerBody();
        if(!CheckAtleastOneOnFloor())
        {
            
            StartCoroutine(Falling()); // ����Э��
        }
    }


    #endregion
    #region ��Ϸʧ��
    IEnumerator Falling()
    {
        overState = GameOverState.Falling;
        playerControl.inputControl.Player.Move.Disable();
        playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[playerHeadSprite.IndexOf(playerHead.GetComponent<SpriteRenderer>().sprite)+4];
        // ˤ�䲢������Ϸ
        StartCoroutine(GameOver());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FallingProgress());
        
    }

    private void FoodDisappearFailureStart()
    {
        // ʳ��׹���ʧ��
        playerControl.inputControl.Player.Move.Disable();
        overState = GameOverState.FoodDisapper;
        playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[playerHeadSprite.IndexOf(playerHead.GetComponent<SpriteRenderer>().sprite) + 4];
        StartCoroutine(GameOver());
    }

    IEnumerator FallingProgress()
    {
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
    IEnumerator GameOver()
    {
        // GameIsOver = true; // ��ʱ���岻�����Ƿ�������ı���
        yield return new WaitForSeconds(0.5f); // ��ʱ0.5s�Ƚ��к�ʵ�����
        gameOverUI.SetActive(true);
    }
    #endregion
    public void RefreshPlayerBody()
    {
        // Vector2 addPosition = playerControl.currentHeadDirection;
        // ͷ��ȷ��
        if(overState == GameOverState.FoodDisapper)
        {
            playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[currentHeadDirection.y == 0 ? (currentHeadDirection.x < 0 ? 0 : 1) : (currentHeadDirection.y < 0 ? 2 : 3) + 8];
        }
        else
        {
            playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[currentHeadDirection.y == 0 ? (currentHeadDirection.x < 0 ? 0 : 1) : (currentHeadDirection.y < 0 ? 2 : 3)];
        }
        // β��ȷ��
        playerTail.GetComponent<SpriteRenderer>().sprite = playerTailSprite[playerBodyDataList[playerLength - 3].body.transform.localPosition.y == playerTail.transform.localPosition.y ? (playerBodyDataList[playerLength - 3].body.transform.localPosition.x > playerTail.transform.localPosition.x ? 0 : 1) :
            (playerBodyDataList[playerLength - 3].body.transform.localPosition.y > playerTail.transform.localPosition.y ? 2 : 3)];
        //if(playerBodyDataList[playerLength - 3].body.transform.localPosition.y > playerTail.transform.localPosition.y)
        //{
        //    Debug.Log("Attention!");
        //}
        // ����ȷ��
        PlayerBodyData playerBodyData;
        for (int i = 0; i < playerLength - 2; i++)
        {
            playerBodyData = playerBodyDataList[i];
            if (i == 0)
            {
                playerBodyData.body.GetComponent<SpriteRenderer>().sprite = GetProperBodySprite(playerHead.transform.localPosition, playerBodyData.body.transform.localPosition,
                    playerBodyDataList[i + 1].body.transform.localPosition);
            }
            else if (i == playerLength - 3) // �������β��
            {
                playerBodyData.body.GetComponent<SpriteRenderer>().sprite = GetProperBodySprite(playerBodyDataList[i - 1].body.transform.localPosition, playerBodyData.body.transform.localPosition,
                    playerTail.transform.localPosition);
            }
            else
            {
                playerBodyData.body.GetComponent<SpriteRenderer>().sprite = GetProperBodySprite(playerBodyDataList[i - 1].body.transform.localPosition, playerBodyData.body.transform.localPosition,
                    playerBodyDataList[i + 1].body.transform.localPosition);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
