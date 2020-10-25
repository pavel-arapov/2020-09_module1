using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private TextMeshProUGUI effectsText;

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
        audioMixer.GetFloat("background", out var background);
        volumeSlider.value = background;
        volumeText.text = $"Music Volume {background:F0}";
        volumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        
        audioMixer.GetFloat("effects", out var master);
        effectsSlider.value = master;
        effectsText.text = $"Effects Volume {master:F0}";
        effectsSlider.onValueChanged.AddListener(EffectsVolumeChanged);
        SetCurrentScreen(Screen.Settings);
    }

    private void MusicVolumeChanged(float volume) {
        audioMixer.SetFloat("background", volume);
        volumeText.text = $"Music Volume {volume:F0}";
    }

    private void EffectsVolumeChanged(float volume) {
        audioMixer.SetFloat("effects", volume);
        effectsText.text = $"Effects Volume {volume:F0}";
    }

    public void ResetSettings() {
        volumeSlider.value = -15f; // TODO: magic numbers need to redefine them with constants or take them from snapshot (default) ?
        effectsSlider.value = -5f;
        audioMixer.SetFloat("background", -15f);
        audioMixer.SetFloat("effects", -5f);
    }

    public void ReturnToMainMenu() {
        SetCurrentScreen(Screen.Main);
    }

    public void ExitGame() {
        Application.Quit();
    }
}