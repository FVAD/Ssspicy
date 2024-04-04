using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private bool isPaused = false;
    // public GameObject playerHead;
    private Player player;
    // public Sprite[] playerHeadSprite; 
    public Vector2 currentHeadDirection = new Vector2(1, 0); // 之后要改这里
    public Canvas canvas;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GetComponent<Player>();
        canvas.enabled = false;
        inputControl = new PlayerInputControl();
        inputControl.Player.Move.started += Move;
        inputControl.Player.Reset.started += Reset;
        inputControl.Player.Pause.started += Pause;
    }



    private void Reset(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    #region UI相关
    private void Pause(InputAction.CallbackContext obj)
    {
        if(isPaused)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }
        isPaused = !isPaused;

    }

    public void PauseByButton()
    {
        if (isPaused)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }
        isPaused = !isPaused;
    }

    public void ReturnToMainPage()
    {
        SceneManager.LoadScene("start");
    }
    #endregion

}
