using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float MaxHp;

    public float CurrentHp;
    [Header("�޵�")]
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
            //ִ������
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            CurrentHp = 0;//����
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
