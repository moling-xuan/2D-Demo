using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName ="Event/ScenesLoadEventSO")]
public class ScenesLoadEventSO : ScriptableObject
{
    public UnityAction<GameScenesSO, Vector3, bool> LoadRequestEvent;
    //场景加载请求
    //locationToload 加载的场景
    //posTogo 加载人物的坐标
    //fadeScreen 是否渐入渐出
    public void RaiseLoadRequestEvent(GameScenesSO locationToLoad,Vector3 posToGo,bool fadeScreen)

    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}
    
