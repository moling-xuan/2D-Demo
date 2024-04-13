using System;
using Unity.Mathematics;

using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
      public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = enemy.GetNewPoint();
    }
    
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);

        }
        if (MathF.Abs(target.x - currentEnemy.transform.position.x) < 0.1f  &&  MathF.Abs(target.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.wait = true;
            target = currentEnemy.GetNewPoint();
        }
        moveDir = (target - currentEnemy.transform.position).normalized;
        if (moveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
            
        }
        if (moveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    public override void OnExit()
    {
       
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
        else 
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }

   
}
