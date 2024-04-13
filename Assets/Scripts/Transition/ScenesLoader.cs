using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour,ISaveavle
{
    public Transform playerTransform;

    public Vector3  firstPosition;

    public Vector3 menuPosition;


    [Header("事件监听")]

    public ScenesLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;
    [Header("场景")]
    
    public GameScenesSO menuScene;
    public GameScenesSO fristLoadScene;
    private  GameScenesSO currentLoadedScene;
    private  GameScenesSO scenesToGo;
    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;

    public ScenesLoadEventSO unloadedSceneEvent;

    private  Vector3 positionToGo;
    
    private bool fadeScreen;
    private bool isLoading;
    public float fadeTime;
    
   
    public void Awake()
    {

        //Addressables.LoadSceneAsync(fristLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadedScene = fristLoadScene;
        //currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
       
    }
    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        ISaveavle saveavle = this;
        saveavle.RegisterSaveData();
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        ISaveavle saveavle = this;
        saveavle.UnRegisterSaveData();
    }

    private void OnBackToMenuEvent()
    {
        scenesToGo = menuScene;
        loadEventSO.RaiseLoadRequestEvent(scenesToGo, menuPosition, true);

    }

    private void NewGame()
    {
        scenesToGo = fristLoadScene;
        //OnLoadRequestEvent(scenesToGo, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(scenesToGo, firstPosition, true);
    }

    private void OnLoadRequestEvent(GameScenesSO locationToGo, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        scenesToGo = locationToGo;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;
        if (currentLoadedScene != null)
        {
            StartCoroutine(UnloadPreviousScence());
        }
        else 
        {
            LoadNewScene();
        }

    }


    private  IEnumerator UnloadPreviousScence()
    {
        if (fadeScreen)
        {
            fadeEvent.FadeIn(fadeTime);
        }
        yield return new WaitForSeconds(fadeTime);
        unloadedSceneEvent.RaiseLoadRequestEvent(scenesToGo, positionToGo, true);
        yield return currentLoadedScene.sceneReference.UnLoadScene();
        //关闭人物
        playerTransform.gameObject.SetActive(false);
       
        LoadNewScene();
        
    }

    public void LoadNewScene()
    {
       var loadingOption= scenesToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnloadCompleted;
    }
    //场景加载结束之后
    private void OnloadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = scenesToGo;
        playerTransform.position = positionToGo;
        playerTransform.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeTime);
        }
        isLoading = false;
        //场景加载完成后事件
        if(currentLoadedScene .sceneType!=SceneType.Menu)

        afterSceneLoadedEvent.RaiseEvent();
    }

    DataDefinition ISaveavle.GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadedScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTransform.GetComponent<DataDefinition>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID].ToVector3 ();

            scenesToGo = data.GetSavedScene();

            OnLoadRequestEvent(scenesToGo, positionToGo, true);
        }
    }
}
