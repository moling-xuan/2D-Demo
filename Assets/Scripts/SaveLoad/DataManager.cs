using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

[DefaultExecutionOrder(order: -100)]
public class DataManager : MonoBehaviour
{
    private List<ISaveavle> saveableList = new List<ISaveavle>();
    public static DataManager instance;
    private Data savaData;
    [Header("ÊÂ¼þ¼àÌý")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    public string jsonFolder;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        savaData = new Data();
        jsonFolder = Application.persistentDataPath+ "/SAVE DATA/";

        ReadSavedData();
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }
    private void Update()
    {
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    public void RegisterSaveData(ISaveavle saveavle)
    {
        if (!saveableList.Contains(saveavle))
        {
            saveableList.Add(saveavle);
        }
    }
    public void UnRegisterSaveData(ISaveavle saveavle)
    {
        saveableList.Remove(saveavle);

    }

    public void Save()
    {
        foreach(var saveavle in saveableList)
        {
            saveavle.GetSaveData(savaData);
        }
        var resultPath = jsonFolder + "data.sav";

        var jsonData = JsonConvert.SerializeObject(savaData);
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        File.WriteAllText(resultPath, jsonData);
        //foreach (var item in savaData.characterPosDict )
        //{
        //    Debug.Log(item.Key + "" + item.Value);
        //}
    }
    public void Load()
    {
        foreach (var saveavle in saveableList)
        {
            saveavle.LoadData(savaData);
        }

    }

    private void ReadSavedData()
    {
        var resultPath = jsonFolder + "data.sav";
        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data> (stringData) ;
            savaData = jsonData;
        }
    }

}
