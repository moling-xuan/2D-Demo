
public class SnailSkillState : BaseState
{
    

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        
        currentEnemy.anim.SetBool("Walk", false);
       
        currentEnemy.anim.SetBool("HIde", true);
        currentEnemy.anim.SetTrigger("Skill");
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
        currentEnemy.GetComponent<Character>().invulnerable = true;
    }
    public override void LogicUpdate()
    {
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter; 
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("HIde", false);
        currentEnemy.GetComponent<Character>().invulnerable = false;
    }

    public override void PhysicsUpdate()
    {
        
    }
}
