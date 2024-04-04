using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName ="Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;

    public void RaiseEvent(Character character)
    {
        OnEventRaised?.Invoke(character);
    }
      
}
