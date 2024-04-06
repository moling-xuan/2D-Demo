using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float MaxHp;

    public float CurrentHp;

    public float maxPower;

    public float currentPower;

    public float powerRecoverSpeed;
    [Header("无敌")]
    public float invulnerableDuration;

    public float invulnerableCounter;

    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange; 

    public UnityEvent<Transform> OnTakeDamage;

    public UnityEvent OnDie;
    private void Start() 
    {
        CurrentHp = MaxHp;
        currentPower = maxPower;
        OnHealthChange?.Invoke(this);
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
        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            //死亡
            OnDie?.Invoke();
            CurrentHp = 0;
            OnHealthChange?.Invoke(this);

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
        OnHealthChange?.Invoke(this);
   
    }
    private void TriggerInvulnerable()
    {
        if (!invulnerable) 
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
             
        }
           
    }
    public void OnSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }
        

}
