using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName ="Event/ScenesLoadEventSO")]
public class ScenesLoadEventSO : ScriptableObject
{
    public UnityAction<GameScenesSO, Vector3, bool> LoadRequestEvent;
    //������������
    //locationToload ���صĳ���
    //posTogo �������������
    //fadeScreen �Ƿ��뽥��
    public void RaiseLoadRequestEvent(GameScenesSO locationToLoad,Vector3 posToGo,bool fadeScreen)

    {
        LoadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);
    }
}
    
