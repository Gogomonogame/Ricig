using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Normal")]
    public int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] bool immortal = false;

    [Header("2D Sprite Healthbar (Для ворогів/метеорів)")]
    [SerializeField] private Transform healthBarFillSprite;

    [Header("Random")]
    [SerializeField] bool isRandomHealth;
    [SerializeField] int minRandomHealth;
    [SerializeField] int maxRandomHealth;

    private float originalXScale;

    private void Awake()
    {
        if (isRandomHealth)
        {
            maxHealth = Random.Range(minRandomHealth, maxRandomHealth);
        }
        currentHealth = maxHealth;

        if (healthBarFillSprite != null)
        {
            originalXScale = healthBarFillSprite.localScale.x;
        }

        UpdateHealthBar();
    }

    public void DecreaseHealth(int ammount)
    {
        if (immortal) return;
        currentHealth -= ammount;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            DistributeScore();
            Destroy(gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFillSprite != null)
        {
            float healthPct = (float)currentHealth / maxHealth;

            Vector3 newScale = healthBarFillSprite.localScale;
            newScale.x = healthPct * originalXScale;
            healthBarFillSprite.localScale = newScale;
        }
    }

    private void DistributeScore()
    {
        if (GameManager.Instance == null) return;
        long scoreFromObject = 0;
        if (CompareTag("Enemy") && GetComponent<Meteor>())
        {
            Meteor meteorScript = GetComponent<Meteor>();
            switch (meteorScript.type)
            {
                case MeteorType.Big: scoreFromObject = 10; break;
                case MeteorType.Medium: scoreFromObject = 5; break;
                case MeteorType.Small: scoreFromObject = 3; break;
                case MeteorType.Tiny: scoreFromObject = 1; break;
            }
        }
        else if (CompareTag("Enemy") && GetComponent<EnemyController>())
        {
            EnemyController enemyScript = GetComponent<EnemyController>();
            switch (enemyScript.type)
            {
                case EnemyType.Scout: scoreFromObject = 15; break;
                case EnemyType.Sniper: scoreFromObject = 30; break;
                case EnemyType.Kamikaze: scoreFromObject = 25; break;
            }
        }
        if (scoreFromObject > 0) GameManager.Instance.ChangeScore(scoreFromObject);
    }

    public void IncreaseHealth(int ammount)
    {
        if (currentHealth <= maxHealth) currentHealth += ammount;
        UpdateHealthBar();
    }

    public void DecreaseMaxHealth(int ammount)
    {
        if (maxHealth > 1) maxHealth -= ammount;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void IncreaseMaxHealth(int ammount)
    {
        maxHealth += ammount;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void ShieldManipulation(bool shieldActivated)
    {
        immortal = shieldActivated;
    }
}