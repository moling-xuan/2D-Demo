using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleprotPoint : MonoBehaviour, IInteractable
{
    public ScenesLoadEventSO loadEventSO; 
    public GameScenesSO sceneToGo;

    public Vector3 positionToGo;
    public void TriggerAction()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }
}
