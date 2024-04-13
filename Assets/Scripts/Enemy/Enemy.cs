using System.Collections;
using System.Collections.Generic;

using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), (typeof(Animator)), (typeof(PhysicsCheak)))] 
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public Animator anim;

    [HideInInspector] public PhysicsCheak physicsCheak;


    [Header("基本属性")]

    public float normalSpeed;

    public float chaseSpeed;

    public float currentSpeed;

    public float hurtForce;

    public Vector3 faceDir;

    public Transform attacker;

    public Vector3 spwanPoint;
    [Header("检测参数")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("计时器")]

    public float waitTime;

    public float waitTimeCounter;

    public bool wait;

    public float lostTime;

    public float lostTimeCounter;
    [Header("状态")]

    public bool isHurt;

    public bool isDead;

    protected BaseState patrolState;

    protected BaseState currentState;

    protected BaseState chaseState;

    protected BaseState skillState;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheak = GetComponent<PhysicsCheak>();
        currentSpeed = normalSpeed; 

        //waitTimeCounter = waitTime;
        spwanPoint = transform.position;

    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        currentState.LogicUpdate();
        TimeCounter();
        

    } 
    private void FixedUpdate() 
    { 
        currentState.PhysicsUpdate();
        if (!isHurt && !isDead && !wait)
            Move();
       
    }
    private void OnDisable()
    {
        currentState.OnExit(); 
    }
    public virtual void Move()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("SnailPreMove")&& !anim.GetCurrentAnimatorStateInfo(0).IsName("SnailRecover"))
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }

        }
        if (!FoundPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter -= Time.deltaTime;

        } 
       

    }

    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }
    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null

        } ;

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }



    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        if (attackTrans.position.x-transform.position.x>0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //被击退
        isHurt = true;
        
        anim.SetTrigger("Hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
        

    }
    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("Dead",true);
        isDead = true;
    }
    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion 方法

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+new Vector3(checkDistance* -transform.localScale.x,0), 0.2f);
    }
}
