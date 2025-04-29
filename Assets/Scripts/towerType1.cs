using UnityEngine;

public class towerType1 : MonoBehaviour
{
    [SerializeField] private int damage = 10; //傷害
    private float timer; //計時器
    private float attack_time=5f;
    [SerializeField] private float range;

    void Start()
    {
        timer=0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= attack_time)
        {
            Attack();
            timer = 0f;
        }
    }

    void Attack()
    {
        //把range內所有碰撞體放進hit 
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D e in hit)
        {
            if (e.gameObject == gameObject) continue; //略過自己
            //Debug.Log("Hit object: " + e.name + ", Tag: " + e.tag);
            if (e.CompareTag("enemy"))
            {
                //Debug.Log("find enemy");
                e.GetComponent<EnemyMovement>().Damage(damage);
            }
        }

    }
 
}
