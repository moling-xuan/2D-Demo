using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();


    }
    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
    }
    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    //TODO:场景切换后更改
    private void Start()
    {
        GetNewCameraBounds();
    }
    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj ==null)
            return;
        
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        //清缓存
        confiner2D.InvalidateCache();


    }


}
