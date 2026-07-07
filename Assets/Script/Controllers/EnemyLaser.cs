using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void Update()
    {
        if (transform.position.y <= -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.DecreaseHealth(damage);
            }

            Destroy(gameObject);
        }
    }
}