using UnityEngine;

public class towerType2 : MonoBehaviour
{
    [SerializeField] private int range=5;
    [SerializeField] private int damage=5;
    [SerializeField] private float attackSpeed=5f;
    [SerializeField] private float bulletSpeed=5f;
    [SerializeField] private GameObject bullet;
    private float timer;
    private float interval=1f;
    private float interval_timer;

    void Start()
    {
        timer = 0f;
        interval_timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        interval_timer += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        
        if (interval_timer>=interval) //每一段時間才檢查一次
        {
            Debug.Log("trigger");
            interval_timer=0f;
            if (timer >= attackSpeed)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
                {
                    
                    Vector3 direction = (collision.transform.position - transform.position).normalized;

                    GameObject bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);

                    bullet1.GetComponent<bullet>().SetUp(direction, range, bulletSpeed, damage);
                
                    timer = 0f;
                }
            }
        }
    }


}
