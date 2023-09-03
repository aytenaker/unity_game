using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace game
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void Start()
        {
            Cursor.visible = false;
            slider = GetComponent<Slider>();
        }

        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}
