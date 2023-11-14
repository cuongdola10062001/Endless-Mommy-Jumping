using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    public GameObject mainMenu;
    public GameObject gameplay;
    public Text scoreText;
    public PauseDialog pauseDialog;
    public GameoverDialog gameoverDialog;

    public override void Awake()
    {
        MakeSingleton(false);
    }
    public void ShowGameplay(bool isShow)
    {
        if (gameplay)
        {
            gameplay.SetActive(isShow);
        }

        if (mainMenu)
        {
            mainMenu.SetActive(!isShow);
        }
    }
    public void UpdateScore(int score)
    {
        if (scoreText)
        {
            scoreText.text ="Score: "+ score.ToString();
        }
    }

}
