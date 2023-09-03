using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("Save Data Config")]
    [SerializeField] private string fileName;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    private SaveFileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one data persistence manager, destroying the newest one...");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new SaveFileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if (this.gameData == null)
        {
            return;
        }

        foreach(IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            return;
        }

        foreach(IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

}
