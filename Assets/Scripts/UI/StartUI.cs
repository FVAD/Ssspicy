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
        // ����ѡ��ҳ��
        selectWindow.SetActive(true);
        startWindow.SetActive(false);
        returnButtion.SetActive(true);
    }
    public void ReturnToMain()
    {
        // �뿪ѡ��ҳ��
        selectWindow.SetActive(false);
        startWindow.SetActive(true);
        returnButtion.SetActive(false);
    }

    public void EnterScene1()
    {
        SceneManager.LoadScene("Scene1");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
