using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("事件监听")]
    public CharacterEventSO healthEvent;

    public ScenesLoadEventSO UnloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FolatEventSO syncVolumeEvent;
    [Header("事件广播")]
    public VoidEventSO pauseEvent;


    [Header("组件")]
    public GameObject gameOverPanel;
    public GameObject restartBtn;
    public Button settingBtn;

    public GameObject pausepanle;
    public Slider volumeSlider;

    private void Awake()
    {
        settingBtn.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        UnloadedSceneEvent.LoadRequestEvent += OnloadedSceneEvent;
        loadDataEvent.OnEventRaised += OnloadedDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnloadedDataEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

   
    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        UnloadedSceneEvent.LoadRequestEvent -= OnloadedSceneEvent;
        loadDataEvent.OnEventRaised -= OnloadedDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnloadedDataEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }
   private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100; 
    }

    private void TogglePausePanel()
    {
        if (pausepanle.activeInHierarchy)
        {
           
            pausepanle.SetActive(false);
            Time.timeScale = 1;

        }
        else 
        {
            pauseEvent.RaiseEvent();
            pausepanle.SetActive(true);
            Time.timeScale = 0;

        }
            

    }

     private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }
    private void OnloadedDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnloadedSceneEvent(GameScenesSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMeau = sceneToLoad.sceneType == SceneType.Menu;
            playerStatBar.gameObject.SetActive(!isMeau);
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.CurrentHp / character.MaxHp;
        playerStatBar.OnHealthChange(persentage);
        playerStatBar.OnPowerChange(character);
    }
}
