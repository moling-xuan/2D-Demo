using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayInputControl inputControl;

    private Rigidbody2D rb;

    private CapsuleCollider2D coll;

    private PhysicsCheak physicsCheak;

    public Vector2 inputDirection;
    
    [Header("人物属性基本参数")]
    public float speed;

    public float jumpForce;
    private float walkspeed=>speed/2.5f;

    private float runspeed;

    public bool isCrouch;

    private Vector2 originalOffset;
    private Vector2 originalSize;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayInputControl();
        physicsCheak = GetComponent<PhysicsCheak>();
        coll = GetComponent<CapsuleCollider2D>();

        originalOffset = coll.offset;
        originalSize = coll.size;

        inputControl.Gameplay.Jump.started += Jump;
        #region 强制走路
        runspeed = speed;

        inputControl.Gameplay.WalkBotton.performed += ctx =>
        {
            if (physicsCheak.isGround)
            {
                speed = walkspeed;
            }
        };
        inputControl.Gameplay.WalkBotton.canceled += ctx =>
        {
            if (physicsCheak.isGround)
            {
                speed = runspeed;
            }
        };
        #endregion   

    }



    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        
    }
    private void FixedUpdate()
    {
        Move();
    }
    
    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime,rb.velocity.y);


        int faceDir = (int)transform.localScale.x;

        //翻转
        if (inputDirection.x> 0)
        {
            faceDir = 1;
        }
        if (inputDirection.x < 0)
        {
            faceDir = -1;
        }
        transform.localScale = new Vector3(faceDir, 1, 1);


        //下蹲
        isCrouch = inputDirection.y < -0.5f && physicsCheak.isGround;
        //修改下蹲碰撞体
        if (isCrouch)
        {
            coll.size = new Vector2(0.7f, 1.7f);
            coll.offset = new Vector2(-0.05f, 0.85f);

        }
        else//还原
        {
            coll.size = originalSize;
            coll.offset = originalOffset;
        }
    
    
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if(physicsCheak.isGround)
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }








}
