using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheak : MonoBehaviour
{
    [Header("×´Ì¬")]
    public bool isGround; 
    //¼ì²â·¶Î§
    [Header("¼ì²â²ÎÊý")]
    public float cheakRaduis;

    public Vector2 bottomOffset;

    public LayerMask groundLayer;
    public void Update()
    {
        Cheak();
    }

    public void Cheak()
    {
        //¼ì²âµØÃæ
        isGround=  Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, cheakRaduis, groundLayer);
          

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, cheakRaduis);
    }



}
