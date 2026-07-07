using UnityEngine;

public enum EnemyType
{
    Scout,    
    Sniper,   
    Kamikaze  
}

public class EnemyController : MonoBehaviour
{
    [Header("Тип ворога")]
    public EnemyType type;

    [Header("Загальні налаштування")]
    [SerializeField] float speed = 3f;
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] float fireRate = 2f;
    [SerializeField] int ramDamage = 2;

    [Header("Для Снайпера та Камікадзе")]
    [SerializeField] float kamikazeHoverTime = 3f;

    private float fireTimer;
    private float stateTimer;
    private int moveDirection = 1;
    private Vector3 targetDirection;
    private bool isDiving = false;

    void Start()
    {
        fireTimer = fireRate;
        stateTimer = kamikazeHoverTime;
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();

        if (transform.position.y <= -6f) Destroy(gameObject);
    }

    private void HandleMovement()
    {
        switch (type)
        {
            case EnemyType.Scout:
                transform.position += Vector3.down * (speed * 0.5f) * Time.deltaTime;
                transform.position += Vector3.right * moveDirection * speed * Time.deltaTime;

                if (transform.position.x >= 1.8f) moveDirection = -1;
                if (transform.position.x <= -1.8f) moveDirection = 1;
                break;

            case EnemyType.Sniper:
                if (transform.position.y > 3.5f)
                {
                    transform.position += Vector3.down * speed * Time.deltaTime;
                }
                break;

            case EnemyType.Kamikaze:
                if (!isDiving)
                {
                    transform.position += Vector3.down * (speed * 0.3f) * Time.deltaTime;
                    stateTimer -= Time.deltaTime;

                    if (stateTimer <= 0 && SpaceshipController.Instance != null)
                    {
                        targetDirection = (SpaceshipController.Instance.transform.position - transform.position).normalized;
                        isDiving = true;
                        speed *= 2.5f;
                    }
                }
                else
                {
                    transform.position += targetDirection * speed * Time.deltaTime;
                }
                break;
        }
    }

    private void HandleShooting()
    {
        if (type == EnemyType.Kamikaze) return;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    private void Shoot()
    {
        if (enemyLaserPrefab == null) return;

        GameObject laser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();

        if (rb == null) return;

        if (type == EnemyType.Sniper && SpaceshipController.Instance != null)
        {
            Vector2 shootDir = (SpaceshipController.Instance.transform.position - transform.position).normalized;
            rb.linearVelocity = shootDir * 7f;

            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            laser.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        else
        {
            rb.linearVelocity = Vector2.down * 5f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == EnemyType.Kamikaze && collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.DecreaseHealth(ramDamage);
            }

            Destroy(gameObject);
        }
    }
}