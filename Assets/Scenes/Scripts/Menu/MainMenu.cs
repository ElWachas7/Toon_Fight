using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject optionsmenu;

    public void OpenOptionMenu() 
    {
        mainmenu.SetActive(false);
        optionsmenu.SetActive(true);
    }

    public void OpenmainMenu()
    {
        optionsmenu.SetActive(false);
        mainmenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame() 
    {
        SceneManager.LoadScene("Level1");
    }
}
