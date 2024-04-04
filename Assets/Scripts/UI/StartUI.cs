using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public GameObject selectWindow;
    public GameObject startWindow;
    public GameObject returnButtion;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void EnterGame()
    {
        // 进入选关页面
        selectWindow.SetActive(true);
        startWindow.SetActive(false);
        returnButtion.SetActive(true);
    }
    public void ReturnToMain()
    {
        // 离开选关页面
        selectWindow.SetActive(false);
        startWindow.SetActive(true);
        returnButtion.SetActive(false);
    }

    public void EnterScene1()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void EnterScene2()
    {
        SceneManager.LoadScene("Scene2");
    }
    public void EnterScene3()
    {
        SceneManager.LoadScene("Scene3");
    }
    public void EnterScene4()
    {
        SceneManager.LoadScene("Scene4");
    }
    public void EnterScene5()
    {
        SceneManager.LoadScene("SceneTest");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
