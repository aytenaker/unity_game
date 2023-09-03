using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentHealth;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> enemiesKilled;

    public GameData() {
        currentHealth = 100;
        playerPosition = new Vector3(435.53f, 0f, 382.5f);
        enemiesKilled = new SerializableDictionary<string, bool>();
    }
}
