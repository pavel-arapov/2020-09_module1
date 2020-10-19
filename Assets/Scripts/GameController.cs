using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public CanvasGroup buttonPanel;
    public CanvasGroup pausePanel;
    public CanvasGroup gameUIPanel;
    public CanvasGroup gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button button;
    public Button pauseButton;
    public Button returnToGameButton;
    public Button exitToMainMenuButton;
    public Button restartLevelButton;
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;

    Character FirstAliveCharacter(Character[] characters) {
        // LINQ: return enemyCharacter.FirstOrDefault(x => !x.IsDead());
        foreach (var character in characters) {
            if (!character.IsDead())
                return character;
        }

        return null;
    }

    void PlayerWon() {
        // Debug.Log("Player won.");
        gameOverText.text = "Congrats! You won!";
        Utility.SetCanvasGroupEnabled(gameOverPanel, true);
    }

    void PlayerLost() {
        // Debug.Log("Player lost.");
        gameOverText.text = "Wasted!";
        Utility.SetCanvasGroupEnabled(gameOverPanel, true);
    }

    bool CheckEndGame() {
        if (FirstAliveCharacter(playerCharacter) == null) {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacter) == null) {
            PlayerWon();
            return true;
        }

        return false;
    }

    void PlayerAttack() {
        waitingForInput = false;
    }

    public void NextTarget() {
        int index = Array.IndexOf(enemyCharacter, currentTarget);
        for (int i = 1; i < enemyCharacter.Length; i++) {
            int next = (index + i) % enemyCharacter.Length;
            if (!enemyCharacter[next].IsDead()) {
                currentTarget.targetIndicator.gameObject.SetActive(false);
                currentTarget = enemyCharacter[next];
                currentTarget.targetIndicator.gameObject.SetActive(true);
                return;
            }
        }
    }

    IEnumerator GameLoop() {
        yield return null;
        while (!CheckEndGame()) {
            foreach (var player in playerCharacter) {
                if (!player.IsDead()) {
                    currentTarget = FirstAliveCharacter(enemyCharacter);
                    if (currentTarget == null)
                        break;

                    currentTarget.targetIndicator.gameObject.SetActive(true);
                    Utility.SetCanvasGroupEnabled(buttonPanel, true);

                    waitingForInput = true;
                    while (waitingForInput)
                        yield return null;

                    Utility.SetCanvasGroupEnabled(buttonPanel, false);
                    currentTarget.targetIndicator.gameObject.SetActive(false);

                    player.target = currentTarget.transform;
                    player.AttackEnemy();

                    while (!player.IsIdle())
                        yield return null;

                    break;
                }
            }

            foreach (var enemy in enemyCharacter) {
                if (!enemy.IsDead()) {
                    Character target = FirstAliveCharacter(playerCharacter);
                    if (target == null)
                        break;

                    enemy.target = target.transform;
                    enemy.AttackEnemy();

                    while (!enemy.IsIdle())
                        yield return null;

                    break;
                }
            }
        }
    }

    private static void ReturnToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    private static void RestartLevel() {
        // not an efficient approach, I suppose...
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called before the first frame update
    void Start() {
        button.onClick.AddListener(PlayerAttack);
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(pausePanel, false);
        Utility.SetCanvasGroupEnabled(gameUIPanel, true);
        Utility.SetCanvasGroupEnabled(gameOverPanel, false);
        pauseButton.onClick.AddListener(() => {
            Utility.SetCanvasGroupEnabled(pausePanel, true);
            Utility.SetCanvasGroupEnabled(gameUIPanel, false);
        });
        returnToGameButton.onClick.AddListener(() => {
            Utility.SetCanvasGroupEnabled(pausePanel, false);
            Utility.SetCanvasGroupEnabled(gameUIPanel, true);
        });
        exitToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        restartLevelButton.onClick.AddListener(RestartLevel);
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update() {
    }
}