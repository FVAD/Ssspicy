using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControl playerControl;
    public Sprite[] playerHeadSprite;
    public Sprite[] playerBodySprite;
    public Sprite[] playerTailSprite;
    
    public GameObject playerBody;
    public GameObject playerHead;
    public GameObject playerTail;
    [Header("玩家属性")]
    public Vector2 currentHeadDirection;
    public int playerLength = 5; // 长度理论上应该由关卡决定
    public List<PlayerBodyData> playerBodyDataList = new();
    [Header("游戏属性")]
    public LayerMask obstacleLayerMask; // 障碍物所在的图层
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

        playerHead.transform.localPosition = new Vector2(0, 0);
        playerTail.transform.localPosition = new Vector2(-4, 0);

        // 以上是对角色位置的一个初始化设定
        playerControl = GetComponent<PlayerControl>();
    }
    void Start()
    {
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
    #region 移动命令
    public void Move(Vector2 addPosition)
    {
        currentHeadDirection = addPosition;
        Vector2 position = (Vector2)playerHead.transform.localPosition + addPosition;
        #region 是否能移动确定
        foreach(var data in playerBodyDataList)
        {
            if((Vector2)data.body.transform.localPosition == position)
            {
                Debug.Log("移动失败");
                return;
            }
        }
        if(CheckObstacleAtPoint((Vector2)transform.position + position + 0.25f * Vector2.up) && CheckObstacleAtPoint((Vector2)transform.position + position - 0.25f * Vector2.up))
        {
            Debug.Log("移动失败");
            return;
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
    }


    #endregion
    public void RefreshPlayerBody()
    {
        // Vector2 addPosition = playerControl.currentHeadDirection;
        // 头部确定
        playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[currentHeadDirection.y == 0 ? (currentHeadDirection.x < 0 ? 0 : 1) : (currentHeadDirection.y < 0 ? 2 : 3)];
        // 尾部确定
        playerTail.GetComponent<SpriteRenderer>().sprite = playerTailSprite[playerBodyDataList[playerLength - 3].body.transform.localPosition.y == playerTail.transform.localPosition.y ? (playerBodyDataList[playerLength - 3].body.transform.localPosition.x > playerTail.transform.localPosition.x ? 0 : 1) :
            (playerBodyDataList[playerLength - 3].body.transform.localPosition.y > playerTail.transform.localPosition.y ? 2 : 3)];
        //if(playerBodyDataList[playerLength - 3].body.transform.localPosition.y > playerTail.transform.localPosition.y)
        //{
        //    Debug.Log("Attention!");
        //}
        // 身体确定
        PlayerBodyData playerBodyData;
        for(int i = 0;i < playerLength - 2; i++)
        {
            playerBodyData = playerBodyDataList[i];
            if (i == 0)
            {
                playerBodyData.body.GetComponent<SpriteRenderer>().sprite = GetProperBodySprite(playerHead.transform.localPosition, playerBodyData.body.transform.localPosition,
                    playerBodyDataList[i + 1].body.transform.localPosition);
            }
            else if (i == playerLength -3) // 是身体的尾部
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
