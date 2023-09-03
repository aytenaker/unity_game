using game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class GameOverMenu : MonoBehaviour
    {
        PlayerStats playerStats;

        public bool isActive = false;

        public GameObject playerHUD;
        public GameObject gameOverMenuUI;

        private void Awake()
        {
            isActive = false;
            playerStats = FindObjectOfType<PlayerStats>();
        }
        void Update()
        {
            if (playerStats.isDead)
            {
                GameOver();
            }
        }

        public void Load()
        {
            DataPersistenceManager.instance.LoadGame();
            SceneManager.LoadScene("level");
        }

        public void GameOver()
        {
            playerHUD.SetActive(false);
            gameOverMenuUI.SetActive(true);
            Cursor.visible = true;
            isActive = true;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
