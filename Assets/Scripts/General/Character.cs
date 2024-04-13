using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour ,ISaveavle
{
    [Header("事件监听")]

    public VoidEventSO newGameEvent;
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
    private void NewGame() 
    {
        CurrentHp = MaxHp;
        currentPower = maxPower;
        OnHealthChange?.Invoke(this);
    }
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
        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }
    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveavle saveavle = this;
        saveavle.RegisterSaveData();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveavle saveavle = this;
        saveavle.UnRegisterSaveData();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (CurrentHp > 0)
            { 
               OnDie?.Invoke();
              CurrentHp = 0;
              OnHealthChange?.Invoke(this);
             }
            //死亡
            

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

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = new SerializeVector3 (transform.position);
            data.floatSaveData[GetDataID().ID + "health"]=this.CurrentHp;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower ;

        }
        else 
        {
            data.characterPosDict.Add(GetDataID().ID, new SerializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().ID + "health", this.CurrentHp);
            data.floatSaveData.Add(GetDataID().ID + "power", this.currentPower);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3 ();
            this.CurrentHp = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];
            OnHealthChange?.Invoke(this);
        }
    }
}
