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
    [Header("�������")]
    public int playerLength = 3; // ����������Ӧ���ɹؿ�����
    public List<Vector2> playerBodyPosList = new(3); // Body��λ������������Ҳ�ɹؿ�����
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
        // �����ǶԽ�ɫλ�õ�һ����ʼ���趨
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
        // ͷ��ȷ��
        playerHead.GetComponent<SpriteRenderer>().sprite = playerHeadSprite[addPosition.y == 0 ? (addPosition.x < 0 ? 0 : 1) : (addPosition.y < 0 ? 2 : 3)];
        // ����ȷ��
        for(int i = 1;i < playerLength - 1; i++)
        {

        }
        // β��ȷ��

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
