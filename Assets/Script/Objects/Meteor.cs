using UnityEngine;

public enum MeteorType
{
    Big,
    Medium,
    Small,
    Tiny,
}

public class Meteor : MonoBehaviour
{
    public MeteorType type;
    [SerializeField] int damage;
    [SerializeField] GameObject spriteObj;
    [HideInInspector] public GameObject bonusPrefab;
    [SerializeField] private GameObject[] bonusPrefabs;

    [Header("Settings")]
    [SerializeField] float minFallSpeed = 1f;
    [SerializeField] float maxFallSpeed = 2f;
    [SerializeField] float minRotationSpeed = 10f;
    [SerializeField] float maxRotationSpeed = 100f;
    [SerializeField, Range(0, 100)] int bonusChance;

    float currentFallSpeed;
    float currentRotationSpeed;

    private void Start()
    {
        InitializeRandomProperties();
    }

    private void Update()
    {
        transform.position += Vector3.down * currentFallSpeed * Time.deltaTime;

        spriteObj.transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            Destroy(gameObject);
        }
    }

    public void InitializeRandomProperties()
    {
        currentFallSpeed = Random.Range(minFallSpeed, maxFallSpeed);

        float randomSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        float rotationDirection = Random.value > 0.5f ? 1f : -1f;

        currentRotationSpeed = randomSpeed * rotationDirection;

        bonusPrefab = bonusPrefabs[Random.Range(0, bonusPrefabs.Length)];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().DecreaseHealth(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        if (Tools.ChanceCalculation(bonusChance))
        {
            if (type == MeteorType.Big || type == MeteorType.Medium)
                Instantiate(bonusPrefab, transform.position, Quaternion.identity);
        }
    }
}