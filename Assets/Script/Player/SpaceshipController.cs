using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public static SpaceshipController Instance { get; private set; }

    [Header("Move")]
    [SerializeField] float speed = 10f;
    [SerializeField] float xLimit = 8f;
    [Header("Firing")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject leftFirePoint;
    [SerializeField] GameObject rightFirePoint;
    [SerializeField] float couldown = 1f;
    [SerializeField] int damage = 1;
    float timer = 0f;
    bool canFire = true;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!canFire)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f) 
                canFire = true;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Move(float direction)
    {
        Vector3 newPosition = transform.position + Vector3.right * direction * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -xLimit, xLimit);
        transform.position = newPosition;
    }

    public void Shoot()
    {
        if(!canFire) return;
        GameObject laser;
        laser = Instantiate(laserPrefab,leftFirePoint.transform.position,Quaternion.identity);
        laser.GetComponent<LaserController>().damage = damage;
        laser = Instantiate(laserPrefab,rightFirePoint.transform.position,Quaternion.identity);
        laser.GetComponent<LaserController>().damage = damage;


        timer = couldown;
        canFire = false;
    }
}