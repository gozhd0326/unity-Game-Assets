using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f; // 이동 속도
    public string direction = "left"; // 방향 right or left
    public float range = 0.0f; // 움직이는 범위
    Vector3 defPos; // 시작 위치

    // Start is called before the first frame update
    void Start()
    {
        if(direction == "right")
        {
            transform.localScale = new Vector2(-1, 1); // 방향 변경
        }
        // 시작 위치
        defPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(range > 0.0f) //range = 0 => 업데이트 코드가 실행되지 않음 // 방향을 바꿀 수 있는 것은 충돌이 발생해야 됨
        {
            if(transform.position.x < defPos.x - (range / 2)) //transform.position은 고슴도치임. //가운데를 중점으로 양 옆을 왔다갔다 함
            {
                direction = "right";
                transform.localScale = new Vector2(-1, 1); // 방향 변경
            }
            if(transform.position.x > defPos.x + (range / 2))
            {
                direction = "left";
                transform.localScale = new Vector2(1, 1); // 방향 변경

            }
        }
    }

    void FixedUpdate()
    {

        // 속도 갱신
        //Rigidbody2D 가져오기
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        if(direction == "right") // 스피드의 방향
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y);
        }
        else 
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y);
        }
    }

    // 접촉
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1,1); // 방향 변경
        }
        else
        {
            direction = "right";
                transform.localScale = new Vector2(-1, 1); // 방향 변경
        }
    }
}
