using game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class FinishMenu : MonoBehaviour
    {
        public bool isActive = false;

        public GameObject playerHUD;
        public GameObject finishMenuUI;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void Finish()
        {
            playerHUD.SetActive(false);
            finishMenuUI.SetActive(true);
            isActive = true;
            Cursor.visible = true;
        }
    }
}
