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
    [Header("屏幕内边缘坐标")]
    public float leftX;
    public float rightX;
    public float upY;
    public float downY;


    [Header("玩家属性")]
    public Vector2 currentHeadDirection;
    public int playerLength = 5; // 长度理论上应该由关卡决定
    public List<PlayerBodyData> playerBodyDataList = new();
    [Header("游戏属性")]
    public LayerMask obstacleLayerMask; // 障碍物所在的图层
    public LayerMask foodLayerMask; // 食物所在的图层
    public LayerMask floorLayerMask; // 地板所在的图层
    public GameObject gameOverUI; // 失败结束页面
    // 以下被整合进结构体,关于这个结构体，position属性是不需要的，但是不排除之后会增加其他属性的可能性，于是就留在这里
    //public List<Vector2> playerBodyPosList = new(3); // Body的位置数组理论上也由关卡决定
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

        // 以上是对角色位置的一个初始化设定
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

    #region 身体部分图片渲染
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
    #region 排障检测
    public bool CheckObstacleAtPoint(Vector2 point)
    {
        // 检测指定点是否存在障碍物
        Collider2D collider = Physics2D.OverlapPoint(point, obstacleLayerMask);

        // 如果 collider 不为 null，表示存在障碍物，返回 true；否则返回 false
        return collider != null;
    }
    #endregion
    #region 食物检测（啊啊啊啊啊干脆把冰块也看成食物吧）
    public bool CheckFoodAtPoint(Vector2 point)
    {
        // 检测指定点是否存在食物
        Collider2D collider = Physics2D.OverlapPoint(point, foodLayerMask);

        // 如果 collider 不为 null，表示存在食物，返回 true；否则返回 false
        return collider != null;
    }

    private bool CheckFoodCanBePushed(Vector2 position, Vector2 addPosition)
    {
        if(CheckObstacleAtPoint((Vector2)transform.position + position + addPosition) ||
                CheckBodyAtPoint(position + addPosition))
        {
            
            // 这里要改上面的函数方法，需要返回一个collider2D
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
        Debug.Log("推动");
        collider.transform.localPosition = (Vector2)collider.transform.localPosition + direction;
    }

    private void DectectAfterPushing(Collider2D collider)
    {
        if (collider != null)
        {
            if(collider.GetComponent<props>().CheckFloorBelowFoot())
            {
                Debug.Log("食物正常");
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
                    // 吃掉香蕉
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
                    Debug.LogError("食物没有标签");
                    break;
            }
            
        }
    }
    #endregion

    #region 身体检测（point是LocalPos）
    public bool CheckBodyAtPoint(Vector2 point)
    {
        foreach (var data in playerBodyDataList)
        {
            if ((Vector2)data.body.transform.localPosition == point)
            {
                Debug.Log("存在身体");
                return true;
            }
        }
        return false;
    }
    #endregion
    #region 检测整体是否在地板上
    public bool CheckAtleastOneOnFloor()
    {
        Collider2D collider; 
        foreach (var data in playerBodyDataList)
        {
            collider = Physics2D.OverlapPoint((Vector2)data.body.transform.position, floorLayerMask);
            if (collider != null)
            {
                Debug.Log("存在地板");
                return true;
            }
        }
        collider = Physics2D.OverlapPoint((Vector2)playerHead.transform.position, floorLayerMask);
        if (collider != null)
        {
            Debug.Log("存在地板");
            return true;
        }
        collider = Physics2D.OverlapPoint((Vector2)playerTail.transform.position, floorLayerMask);
        if (collider != null)
        {
            Debug.Log("存在地板");
            return true;
        }
        Debug.Log("全部悬空");
        return false;
    }
    #endregion
    #region 移动命令
    public void Move(Vector2 addPosition)
    {
        currentHeadDirection = addPosition;
        Vector2 position = (Vector2)playerHead.transform.localPosition + addPosition;
        #region 是否能移动确定
        if(CheckBodyAtPoint(position))
        {
            Debug.Log("移动失败");
            return;
        }
        // if(CheckObstacleAtPoint((Vector2)transform.position + position + 0.25f * Vector2.up) && CheckObstacleAtPoint((Vector2)transform.position + position - 0.25f * Vector2.up))
        // 现在障碍物变成独立个体而不是Tile了
        if (CheckObstacleAtPoint((Vector2)transform.position + position))
        {
            Debug.Log("移动失败");
            return;
        }
        #endregion
        #region 食物确定
        if(CheckFoodAtPoint((Vector2)transform.position + position))
        {
            if(CheckObstacleAtPoint((Vector2)transform.position + position + addPosition)  || 
                CheckBodyAtPoint(position + addPosition))
            {
                // Debug.Log("存在障碍物");
                // 两个Position检测食物延长线是否有障碍物或者自己的身体, 如果有，吃，没有，推动
                // 不太对，多个物体叠一起就爆了，写个函数罢
                // 爽吃！
                EatFood((Vector2)transform.position + position);
                return;
            }
            else
            {
                PushFoodToDirection(addPosition);
            }
        }
        #endregion
        #region 角色身体前移补位
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
            
            StartCoroutine(Falling()); // 发起协程
        }
    }


    #endregion
    #region 游戏失败
    IEnumerator Falling()
    {
        overState = GameOverState.Falling;
        playerControl.inputControl.Player.Move.Disable();
        playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[playerHeadSprite.IndexOf(playerHead.GetComponent<SpriteRenderer>().sprite)+4];
        // 摔落并结束游戏
        StartCoroutine(GameOver());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FallingProgress());
        
    }

    private void FoodDisappearFailureStart()
    {
        // 食物坠落的失败
        playerControl.inputControl.Player.Move.Disable();
        overState = GameOverState.FoodDisapper;
        playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[playerHeadSprite.IndexOf(playerHead.GetComponent<SpriteRenderer>().sprite) + 4];
        StartCoroutine(GameOver());
    }

    IEnumerator FallingProgress()
    {
        Debug.Log("下落过程");
        while (transform.position.y >= -50f)
        {
            // 计算当前帧的移动距离
            float step = 40f * Time.deltaTime;

            // 移动物体向目标位置
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position - new Vector2(0, 50f + transform.position.y), step);

            // 等待下一帧
            yield return null;
        }
    }
    IEnumerator GameOver()
    {
        // GameIsOver = true; // 暂时意义不明还是放在这里的变量
        yield return new WaitForSeconds(0.5f); // 延时0.5s比较切合实际情况
        gameOverUI.SetActive(true);
    }
    #endregion
    public void RefreshPlayerBody()
    {
        // Vector2 addPosition = playerControl.currentHeadDirection;
        // 头部确定
        if(overState == GameOverState.FoodDisapper)
        {
            playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[currentHeadDirection.y == 0 ? (currentHeadDirection.x < 0 ? 0 : 1) : (currentHeadDirection.y < 0 ? 2 : 3) + 8];
        }
        else
        {
            playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[currentHeadDirection.y == 0 ? (currentHeadDirection.x < 0 ? 0 : 1) : (currentHeadDirection.y < 0 ? 2 : 3)];
        }
        // 尾部确定
        playerTail.GetComponent<SpriteRenderer>().sprite = playerTailSprite[playerBodyDataList[playerLength - 3].body.transform.localPosition.y == playerTail.transform.localPosition.y ? (playerBodyDataList[playerLength - 3].body.transform.localPosition.x > playerTail.transform.localPosition.x ? 0 : 1) :
            (playerBodyDataList[playerLength - 3].body.transform.localPosition.y > playerTail.transform.localPosition.y ? 2 : 3)];
        //if(playerBodyDataList[playerLength - 3].body.transform.localPosition.y > playerTail.transform.localPosition.y)
        //{
        //    Debug.Log("Attention!");
        //}
        // 身体确定
        PlayerBodyData playerBodyData;
        for (int i = 0; i < playerLength - 2; i++)
        {
            playerBodyData = playerBodyDataList[i];
            if (i == 0)
            {
                playerBodyData.body.GetComponent<SpriteRenderer>().sprite = GetProperBodySprite(playerHead.transform.localPosition, playerBodyData.body.transform.localPosition,
                    playerBodyDataList[i + 1].body.transform.localPosition);
            }
            else if (i == playerLength - 3) // 是身体的尾部
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
