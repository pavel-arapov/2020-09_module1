using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        None,
        Main,
        Levels,
        Settings,
    }

    public CanvasGroup mainScreen;
    public CanvasGroup settingsScreen;
    public CanvasGroup levelsScreen;

    void SetCurrentScreen(Screen screen) {
        Utility.SetCanvasGroupEnabled(mainScreen, screen == Screen.Main);
        Utility.SetCanvasGroupEnabled(settingsScreen, screen == Screen.Settings);
        Utility.SetCanvasGroupEnabled(levelsScreen, screen == Screen.Levels);
    }

    // Start is called before the first frame update
    void Start() {
        SetCurrentScreen(Screen.Main);
    }

    public void StartNewGame() {
        SetCurrentScreen(Screen.Levels);
    }

    /**
     * TODO: level parameter has to be verified 
     */
    public void SelectLevel(string level) {
        SetCurrentScreen(Screen.None);
        LoadingScreen.instance.LoadScene(level);
    }

    public void OpenSettings() {
        SetCurrentScreen(Screen.Settings);
    }

    public void ReturnToMainMenu() {
        SetCurrentScreen(Screen.Main);
    }

    public void ExitGame() {
        Application.Quit();
    }
}