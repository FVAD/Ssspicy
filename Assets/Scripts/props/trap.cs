using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] sprites;
    public bool isIdle;
    private int idleAnimCounter;
    private float timer;
    private SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        isIdle = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0.3f)
        {
            timer = 0f;
            if (isIdle)
            {
                idleAnim();
            }
        }
        timer += Time.deltaTime;
    }

    // 干脆就没有用Animator
    private void idleAnim()
    {
        if(idleAnimCounter > 3)
        {
            idleAnimCounter = 0;
        }
        sr.sprite = sprites[idleAnimCounter];
        idleAnimCounter++;

    }
}
