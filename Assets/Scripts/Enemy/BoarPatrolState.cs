
public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
       
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
       
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }


        if (!currentEnemy.physicsCheak.isGround || (currentEnemy.physicsCheak.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheak.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("Walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("Walk", true);
        }
    }

    
    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Walk", false);
    }

  
}
