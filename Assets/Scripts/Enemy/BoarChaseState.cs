using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState: BaseState
{  
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        Debug.Log("chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("Run",true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        if (!currentEnemy.physicsCheak.isGround || (currentEnemy.physicsCheak.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheak.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x , 1, 1);
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }
     
    public override void PhysicsUpdate()
    {
       
    }
}
