using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControl playerControl;
    public Sprite[] playerHeadSprite;
    public Sprite[] playerBodySprite;
    public Sprite[] playerTailSprite;
    public List<PlayerBodyData> playerBodyDataList;
    public GameObject playerBody;
    [Header("玩家属性")]
    public int playerLength = 3; // 长度理论上应该由关卡决定
    public List<Vector2> playerBodyPosList = new(3); // Body的位置数组理论上也由关卡决定
    public List<GameObject> playerBodyList = new(3-1);

    // Start is called before the first frame update
    private void Awake()
    {
        playerBodyPosList[0] = new Vector2(0, 0);
        playerBodyPosList[1] = new Vector2(-1, 0);
        playerBodyPosList[2] = new Vector2(-2, 0);
        GameObject body;
        for(int i = 1; i < playerLength-1;i++)
        {
            body = GameObject.Instantiate(playerBody);
            body.transform.position = playerBodyPosList[i];
            playerBodyList.Add(body);
        }
        // 以上是对角色位置的一个初始化设定
        playerControl = GetComponent<PlayerControl>();
    }
    void Start()
    {
        RefreshPlayerBody();
    }

    public void RefreshPlayerBody()
    {
        Vector2 addPosition = playerControl.currentHeadDirection;
        GameObject playerHead = playerControl.playerHead;
        // 头部确定
        playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[addPosition.y == 0 ? (addPosition.x < 0 ? 0 : 1) : (addPosition.y < 0 ? 2 : 3)];
        // 身体确定
        for(int i = 1;i < playerLength - 1; i++)
        {

        }
        // 尾部确定

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
