 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheak : MonoBehaviour
{

    private CapsuleCollider2D coll;
    [Header("×´Ì¬")]
    public bool isGround;
    public bool touchRightWall;
    public bool touchLeftWall;
    //¼ì²â·¶Î§
    [Header("¼ì²â²ÎÊý")]
    public float cheakRaduis;
    public bool manual;

    public Vector2 bottomOffset;

    public Vector2 rightOffset;
    public Vector2 leftOffset; 

    public LayerMask groundLayer;

    
    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x,rightOffset.y);
        }
    }
    public void Update()
    {
        Cheak();
    }

    public void Cheak()
    {
        //¼ì²âµØÃæ
        isGround=  Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, cheakRaduis, groundLayer);
        //Ç½ÌåÅÐ¶Ï  

        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, cheakRaduis,groundLayer); 
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position +rightOffset, cheakRaduis, groundLayer); 
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, cheakRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, cheakRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, cheakRaduis);
    }



}
