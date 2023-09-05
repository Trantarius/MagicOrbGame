using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private const float maxHealth = 3f;
    public float health = maxHealth;

    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    void Update()
    {
        health = PlayerPrefs.GetFloat("PlayerHealth", maxHealth); 
        healthBar.fillAmount = health / maxHealth; 
    }
    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            //saves the updated health value to PlayerPrefs
            PlayerPrefs.SetFloat("PlayerHealth", health);
            PlayerPrefs.Save();
        }
    }
    
    public void RestoreHealth()
    {
        PlayerPrefs.SetFloat("PlayerHealth", maxHealth);
        PlayerPrefs.Save();
    }

    void OnEnable()
    {
        EventBus.onLevelCompleted += RestoreHealth;
        EventBus.onGameFailed += RestoreHealth;
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= RestoreHealth;
        EventBus.onGameFailed -= RestoreHealth;
    }
}
