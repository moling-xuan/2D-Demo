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

    private PlayerAnimation playerAnimation;

    public Vector2 inputDirection;

    private Vector2 originalOffset;

    private Vector2 originalSize;




    [Header("人物属性基本参数")]
    public float speed;

    public float jumpForce;

    public float hurtForce;
    private float walkspeed => speed / 2.5f;

    private float runspeed;


    [Header("状态")]
    public bool isCrouch;

    public bool isHurt;

    public bool isAttack;

    public bool isDead;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;

    public PhysicsMaterial2D wall;
    


   



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayInputControl();
        physicsCheak = GetComponent<PhysicsCheak>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

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
        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;



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
        CheakState();
    }
    private void FixedUpdate()
    {
        if(!isHurt&&!isAttack)
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
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
        
    }
    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x-attacker.position.x),0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse); 

    }
    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();

    }

    #endregion
    private void CheakState()
    {
        coll.sharedMaterial = physicsCheak.isGround ? normal : wall;
    }



}
