using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{

    public GameObject prompt;

    public void OnStartButtonClick()
    {
        gameObject.SetActive(false);
        GameConroller.Instance.StartNewGame(false);
    }
    public void OnStartTutorialButtonClick()
    {
        GameConroller.Instance.StartNewGame(true);

    }
    public void OnQuitButtonClick()
    {
        prompt.SetActive(true);
    }
    public void OnPromptQuitButtonClick()
    {
        Application.Quit();
    }
    public void ShowMenu()
    {
        gameObject.SetActive(true);

    }
}
