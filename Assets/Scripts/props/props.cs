using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class props : MonoBehaviour
{
    private float amplitude = 0.15f; // 正弦振动振幅
    private float frequency = 1f;
    private float v = 1f;
    private Vector2 startPos;
    private Vector2 startScale;

    [Header("主要属性")]
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
