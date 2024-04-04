using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite firstSprite;
    public Sprite nextSprite;
    private Player player;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        
        sr.sprite = firstSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.foodLeftCounts == 0 && sr.sprite == firstSprite)
        {
            sr.sprite = nextSprite;
        }
        if(player.playerLength == 0)
        {
            sr.sprite = firstSprite;
        }
    }
}
