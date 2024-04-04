 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCheak : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private PlayerController playerController;
    [Header("×´Ì¬")]
    public bool onWall;
    public bool isGround;
    public bool touchRightWall;
    public bool touchLeftWall;
    //¼ì²â·¶Î§
    [Header("¼ì²â²ÎÊý")]
    public float cheakRaduis;
    public bool manual;
    public bool isPlayer;
    public Vector2 bottomOffset;

    public Vector2 rightOffset;
    public Vector2 leftOffset;

    public LayerMask groundLayer;


    private void Awake()

    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {

            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
         }
        if (isPlayer)
            playerController = GetComponent<PlayerController>();
    }
    public void Update()
    {
        Cheak();
    }

    public void Cheak()
    {
        //¼ì²âµØÃæ
        if (onWall)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), cheakRaduis, groundLayer);
        else 
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), cheakRaduis, groundLayer);
        }
        //Ç½ÌåÅÐ¶Ï  
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x,leftOffset.y), cheakRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position +new Vector2(rightOffset.x,rightOffset.y),cheakRaduis, groundLayer);

        //ÔÚÇ½±ÚÉÏ
        if (isPlayer)
        onWall = (touchLeftWall && playerController.inputDirection.x < 0f || touchRightWall && playerController.inputDirection.x > 0f) && rb.velocity.y<0f;
    
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), cheakRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), cheakRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), cheakRaduis);



    }
}
