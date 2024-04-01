using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameOverUI : MonoBehaviour
{
    private float amplitude = 0.5f; // 正弦振动振幅
    private float frequency = 1f;
    private float v = 1f;

    private float appearTime = 1f;
    private float appearTimeTimer = 0f;
    private float curAlpha;
    private SpriteRenderer sr;

    private Vector2 startPos;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sr.color = sr.color - new Color(0f, 0f, 0f, sr.color.a);
        startPos = transform.position; // 之前其他的问题都在这里
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        AppearAnimation();
        FloatingAnimation();
    }

    private void AppearAnimation()
    {
        if(appearTimeTimer < appearTime)
        {
            appearTimeTimer += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, appearTimeTimer / appearTime);
        }
        else if(appearTimeTimer > appearTime)
        {
            appearTimeTimer = appearTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }
    }
    private void FloatingAnimation()
    {
        float Offset = Mathf.Sin(Time.time * frequency) * amplitude;

        Vector2 curPos = startPos + new Vector2(0, Offset + 0.25f);

        transform.localPosition = Vector2.Lerp(transform.localPosition, curPos, Time.deltaTime * v);
    }
}
