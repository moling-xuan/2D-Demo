using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    public Transform playerTransform;

    public Vector3  firstPosition;


    [Header("事件监听")]

    public ScenesLoadEventSO loadEventSO;


    public GameScenesSO fristLoadScene;

    [Header("广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public  GameScenesSO currentLoadedScene;
    public  GameScenesSO scenesToGo;

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
        NewGame();
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }


    private void NewGame()
    {
        scenesToGo = fristLoadScene;
        OnLoadRequestEvent(scenesToGo, firstPosition, true);
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
        afterSceneLoadedEvent.RaiseEvent();
    }
}
