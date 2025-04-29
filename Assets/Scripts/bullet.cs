using UnityEngine;

public class bullet : MonoBehaviour
{
    private float speed;
    private float maxRange; //最遠距離
    private Vector3 direction;
    private Vector3 spawnPoint;
    [SerializeField] private float range; //本身的碰撞範圍
    private int damage;
    void Start()
    {
        
    }

    public void SetUp(Vector3 dir, float range, float getSpeed, int getDamage)
    {
        direction = dir.normalized; //normalized轉成單位向量
        maxRange = range;
        speed = getSpeed;
        spawnPoint = transform.position;
        damage = getDamage;
    }

    void Update()
    {
        
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(spawnPoint, transform.position) > maxRange) //超出範圍自爆
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            Debug.Log("bullet hit enemy");
            collision.GetComponent<EnemyMovement>().Damage(damage);
            Destroy(gameObject);
        }
    }


}
