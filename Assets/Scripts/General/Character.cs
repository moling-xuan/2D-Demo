using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float MaxHp;

    public float CurrentHp;
    [Header("无敌")]
    public float invulnerableDuration;

    public float invulnerableCounter;

    public bool invulnerable;

    public UnityEvent<Transform> OnTakeDamage;

    public UnityEvent OnDie;
    private void Start() 
    {
        CurrentHp = MaxHp;
    }
    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        if (invulnerable)
            return;
        if (CurrentHp - attacker.damage > 0)
        {
            CurrentHp -= attacker.damage;
            TriggerInvulnerable();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            CurrentHp = 0;//死亡
            OnDie?.Invoke();

        }

   
    }
    private void TriggerInvulnerable()
    {
        if (!invulnerable) 
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
             
        }
           
    }



}
