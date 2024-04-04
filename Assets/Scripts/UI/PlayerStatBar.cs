using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStatBar : MonoBehaviour
{
    public Character currentCharacter;
    public Image healthImage;

    public Image healthDealyImage;

    public Image powerImage;

    private bool isRecovery;
    private void Update()
    {
        if (healthDealyImage.fillAmount > healthImage.fillAmount)
        {
            healthDealyImage.fillAmount -= Time.deltaTime; 
        
        }
        if (isRecovery)
        {
            float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = persentage;
            if (persentage >= 1)
            {
                isRecovery = false;
                return;
            }
        }
    }

    public void OnHealthChange (float persentage)

    {
        healthImage.fillAmount = persentage;

    }
    public void OnPowerChange(Character character)
    {
        isRecovery = true;
        currentCharacter = character;
    }


}
