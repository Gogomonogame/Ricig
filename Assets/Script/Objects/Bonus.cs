using UnityEngine;

public enum BonusType
{
    Shield,
}

public class Bonus : MonoBehaviour
{
    public BonusType type;
    [SerializeField] float fallSpeed = 1f;


    [Header("Settings")]
    [SerializeField] float shieldTime = 20f;
    

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= -6f)
        {
            Destroy(gameObject);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case BonusType.Shield:
                    collision.gameObject.GetComponent<Shield>().ShieldActivation(shieldTime);
                    break;
            }

            Destroy(gameObject);
        }
    }
}