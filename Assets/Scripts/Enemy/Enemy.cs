using System.Collections;
using System.Collections.Generic;
using UnityEditor.UnityLinker;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    protected Animator anim;

    PhysicsCheak physicsCheak;


    [Header("基本属性")]

    public float normalSpeed;
     
    public float chaseSpeed;

    public float currentSpeed;

    public Vector3 faceDir;
    [Header("计时器")]

    public float waitTime;
    public float waitTimeCounter;
    public bool wait;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheak = GetComponent<PhysicsCheak>();
        currentSpeed = normalSpeed;
        

    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if ((physicsCheak.touchLeftWall&&faceDir.x<0)||(physicsCheak.touchRightWall&&faceDir.x>0))
        {
            wait = true;
            anim.SetBool("Walk", false);
        }
        TimeCounter();

    } 
    private void FixedUpdate()
    {
        Move();
        
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }


    public void TimeCounter()
    {
        if (wait)
        {
            wait = false;
            waitTimeCounter = waitTime;
            transform.localScale = new Vector3(faceDir.x, 1, 1);
        }
    }


}
