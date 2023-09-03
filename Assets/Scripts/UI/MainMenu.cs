using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void NewGame()
    {
        DisableButtons();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("level");
    }

    public void ContinueGame()
    {
        DisableButtons();
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene("level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisableButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}
