using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Sign : MonoBehaviour
{
    private PlayInputControl playerInput;

    private Animator anim;

    public GameObject signSprite;

    public Transform playerTrans;

    private IInteractable targetItem; 

    private bool canPress;

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        
        playerInput = new PlayInputControl();
        playerInput.Enable();
    }
    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interatable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }

    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confrim.started += OnConfrim;
    }

    private void OnConfrim(InputAction.CallbackContext obj)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted) 
        {
            //Debug.Log(((InputAction)obj).activeControl.device) ;
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device )
            {
                case Keyboard:
                    anim.Play("Sign");
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
