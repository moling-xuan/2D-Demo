using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheak : MonoBehaviour
{
    [Header("״̬")]
    public bool isGround; 
    //��ⷶΧ
    [Header("������")]
    public float cheakRaduis;

    public Vector2 bottomOffset;

    public LayerMask groundLayer;
    public void Update()
    {
        Cheak();
    }

    public void Cheak()
    {
        //������
        isGround=  Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset, cheakRaduis, groundLayer);
          

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, cheakRaduis);
    }



}
