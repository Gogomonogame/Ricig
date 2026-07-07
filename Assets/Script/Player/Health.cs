using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Normal")]
    public int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] bool immortal = false;

    [Header("Random")]
    [SerializeField] bool isRandomHealth;
    [SerializeField] int minRandomHealth;
    [SerializeField] int maxRandomHealth;

    private void Awake()
    {
        if (isRandomHealth)
        {
            maxHealth = Random.Range(minRandomHealth, maxRandomHealth);
        }
        currentHealth = maxHealth;
    }

    public void DecreaseHealth(int ammount)
    {
        if(immortal) return;
        currentHealth -= ammount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseHealth(int ammount)
    {
        if (currentHealth <= maxHealth)
        currentHealth += ammount;
    }

    public void DecreaseMaxHealth(int ammount)
    {
        if(maxHealth > 1) maxHealth -= ammount;
        currentHealth = maxHealth;
    } 

    public void IncreaseMaxHealth(int ammount)
    {
        maxHealth += ammount;
        currentHealth = maxHealth;
    } 

    public void ShieldManipulation(bool shieldActivated)
    {
        immortal = shieldActivated;
    }
}
