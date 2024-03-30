using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public GameObject playerHead;
    private Player player;
    // public Sprite[] playerHeadSprite; 
    public Vector2 currentHeadDirection = new Vector2(1, 0); // 之后要改这里
    // Start is called before the first frame update
    private void Awake()
    {
        player = GetComponent<Player>();
        inputControl = new PlayerInputControl();
        inputControl.Player.Move.started += Move;
    }

    private void Move(InputAction.CallbackContext obj)
    {
        Vector2 addPosition = inputControl.Player.Move.ReadValue<Vector2>();
        player.Move(addPosition);
        // currentHeadDirection = addPosition;
        //Vector2 position = (Vector2)playerHead.transform.position + addPosition;
        //player.RefreshPlayerBody();

        //playerHead.transform.position = position;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
