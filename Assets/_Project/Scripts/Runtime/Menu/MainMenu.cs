using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject creditsMenu;

    private List<GameObject> menuList;

    private GameObject currentMenu;

    private void Start()
    {
        menuList = new List<GameObject>() { mainMenu, settingsMenu, tutorialMenu, creditsMenu };
    }

    public void StartGame()
    {
        // Load the game scene (replace "GameScene" with your game scene name)
        SceneManager.LoadScene("SCN_Game");
    }

    public void QuitGame()
    {
        // Quit the game (this will only work in a built version of the game)
        Application.Quit();
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        currentMenu = creditsMenu;
    }

    public void Tutorial()
    {
        mainMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        currentMenu = tutorialMenu;
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        currentMenu = settingsMenu;
    }

    public void BackToMainMenu()
    {
        currentMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = mainMenu;
    }

}
