using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    public int damage = 0;
    [SerializeField] int damageScalerX = 2;

    private void Update()
    {
        Move();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Health>().DecreaseHealth(damage);
        Destroy(gameObject);
    }

    public void Move()
    {

        Vector3 newPosition = transform.position + Vector3.up * speed * Time.deltaTime;
        transform.position = newPosition;

        if (transform.position.y >= 8) Destroy(gameObject);
    }

    public void UpgradeDamageAnimTrigger()
    {
        damage *= damageScalerX;
    }


}
