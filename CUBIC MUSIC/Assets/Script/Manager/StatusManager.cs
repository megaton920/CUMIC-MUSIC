using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 0.1f;
    [SerializeField] int blinkCount = 10;
    int currentBlinkCount = 0;

    bool isBlink = false;

    int maxHP = 3;
    int currentHP = 3;

    int maxShield = 3;
    int currentShield = 0;

    [SerializeField] Image[] hpImage = null;
    [SerializeField] Image[] shieldImage = null;
    [SerializeField] int shieldIncreaseCombo = 5;
    int currentShieldCombo = 0;
    [SerializeField] Image shieldGauge = null;

    bool isDead = false;

    Result theResult;
    NoteManager theNoteManager;
    [SerializeField] MeshRenderer playerMesh = null;

    private void Start()
    {
        theResult = FindObjectOfType<Result>(); 
        theNoteManager = FindObjectOfType<NoteManager>();
    }
    public void DecreaseHP(int p_num)
    {
        if(!isBlink)
        {
            if (currentShield > 0)
                DecreaseShield(p_num);
            else
            {
                currentHP -= p_num;

                if (currentHP <= 0)
                {
                    isDead = true;
                    theResult.ShowResult();
                    theNoteManager.RemoveNote();
                }
                else
                {
                    StartCoroutine(BlinkCo());

                }

                SettingHPImage();
            }
        }
    }

    void SettingHPImage()
    {
        for(int i=0; i<hpImage.Length; i++)
        {
            if (i < currentHP)
                hpImage[i].gameObject.SetActive(true);
            else
                hpImage[i].gameObject.SetActive(false);
        }
    }

    void SettingShieldImage()
    {
        for (int i = 0; i < shieldImage.Length; i++)
        {
            if (i < currentShield)
                shieldImage[i].gameObject.SetActive(true);
            else
                shieldImage[i].gameObject.SetActive(false);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator BlinkCo()
    {
        isBlink = true;
        while(currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            currentBlinkCount++;
        }

        playerMesh.enabled = true;
        isBlink = false;
        currentBlinkCount = 0;
    }

    public void IncreaseShield()
    {
        currentShield++;

        if (currentShield >= maxShield)
            currentShield = maxShield;
        
        SettingShieldImage();
    }

    public void DecreaseShield(int p_num)
    {
        currentShield -= p_num;

        if (currentShield <= 0)
            currentShield = 0;

        SettingShieldImage();
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if(currentShieldCombo >= shieldIncreaseCombo)
        {
            currentShieldCombo = 0;
            IncreaseShield();
        }

        shieldGauge.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }

    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        shieldGauge.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }

    public void IncreaseHP(int p_num)
    {
        currentHP += p_num;
        if (currentHP <= maxHP)
            currentHP = maxHP;

        SettingHPImage();
    }

    public void Initialized()
    {
        currentHP = maxHP;
        currentShield = 0;
        currentShieldCombo = 0;
        shieldGauge.fillAmount = 0;
        isDead = false;
        SettingHPImage();
        SettingShieldImage();
    }
}
