using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCheck : MonoBehaviour
{
    public BattleGround bg;
    public Player jg;
    public GameObject winText;
    public GameObject ui;

    // Update is called once per frame
    void Update()
    {
        if(bg.NodeQty == jg.PlayerNodes.Count)
        {
            ui.SetActive(false);
            winText.SetActive(true);
        }
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
