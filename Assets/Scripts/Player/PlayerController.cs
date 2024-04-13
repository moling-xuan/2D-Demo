using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("事件监听")]
    public ScenesLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneloadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;

    public PlayInputControl inputControl;

    private Rigidbody2D rb;

    private CapsuleCollider2D coll;

    private PhysicsCheak physicsCheak;

    private PlayerAnimation playerAnimation;

    private Character character;
    public Vector2 inputDirection;

    private Vector2 originalOffset;

    private Vector2 originalSize;




    [Header("人物属性基本参数")]
    public float speed;

    public float jumpForce;

    public float wallJumpForce;

    public float hurtForce;

    public float slideDistance;

    public int slidePowerCost;

    public float slideSpeed;
    private float walkspeed => speed / 2.5f;

    private float runspeed;


    [Header("状态")]
    public bool isCrouch;

    public bool isHurt;

    public bool isAttack;

    public bool isDead;

    public bool wallJump;

    public bool isSlide;
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
        character = GetComponent<Character>();

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
        inputControl.Gameplay.Slide.started += Slide;
        inputControl.Enable();

    }

    

    private void OnEnable()
    {
        
        sceneLoadEvent.LoadRequestEvent += OnloadEvent;
        afterSceneloadEvent.OnEventRaised += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised  += OnloadDataEvent;
        backToMenuEvent.OnEventRaised += OnloadDataEvent;
    }

   

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnloadEvent;
        afterSceneloadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised  -= OnloadDataEvent;
        backToMenuEvent.OnEventRaised -= OnloadDataEvent;
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
    private void OnAfterSceneLoadEvent()
    {
        inputControl.Gameplay.Enable();
    }

    private void OnloadEvent(GameScenesSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    } 
    private void OnloadDataEvent()
    {
        isDead = false;
    }

    public void Move()
    {
        if(!isCrouch&&!wallJump) 
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
        if (physicsCheak.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            isSlide = false;
            StopAllCoroutines();
        }
        else if (physicsCheak.onWall)
        {
          rb.AddForce(new Vector2(-inputDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
        }
        GetComponent<AudioDefination>()?.PlayAudioClip();
    }
    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
        
    }
    private void Slide(InputAction.CallbackContext context)
    {
        if (!isSlide&&physicsCheak.isGround&&character.currentPower >=slidePowerCost)
        {
            isSlide = true;
            
            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }
        
    }
    private IEnumerator TriggerSlide(Vector3  target)
    {
        do
        {
            yield return null;
            if (!physicsCheak.isGround)
            {
                break;
            }
            if (physicsCheak.touchLeftWall&&transform .localScale .x<0f || physicsCheak.touchRightWall&&transform.localScale.x > 0f)
            {
                isSlide = false;
                break;
            }
            rb.MovePosition(new Vector2(transform.position.x + transform.lossyScale.x * slideSpeed, transform.position.y));
        }
        while (MathF.Abs(target.x - transform.position.x) > 0.1f);
        
            isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

        

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

        if (physicsCheak.onWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f);
            
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        }
        if (wallJump && rb.velocity.y < 0)
        {
            wallJump = false;
        }
    }



}
