using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable 
{
    [Header("¹ã²¥")]
    public VoidEventSO saveDataEvent;
     

    public SpriteRenderer spriteRenderer;
    public GameObject lightobj;

    public Sprite darkSprit;
    public Sprite lightSprit;
    public bool isDone;

     
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprit : darkSprit;
        lightobj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprit;
            lightobj.SetActive(isDone);
            saveDataEvent.RaiseEvent(); 
            this.gameObject.tag = "Untagged";
        }
    }
}
