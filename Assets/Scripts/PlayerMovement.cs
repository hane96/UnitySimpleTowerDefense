using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); //左右鍵盤輸入 (-1 , 1)
        transform.position += new Vector3(moveX, 0, 0) * 5f * Time.deltaTime; //移動角色
    }
}
