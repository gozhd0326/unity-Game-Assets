using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    //이동거리 x 시간 = 속도
    public float moveX = 0.0f; // x 이동 거리
    public float moveY = 0.0f; // y 이동 거리
    public float times = 0.0f; // 시간
    public float weight = 0.0f; // 정지 시간
    public bool isMoveWhenOn = false; // 올라 탔을 때 움직이기 (얘가 트루이면 isCanMove를 false로 바꿔놔야 됨)

    public bool isCanMove = true; // 움직임 (계속 움직임)
    float perDX; // 1프레임 당 x 이동 값
    float perDY; // 1 프레임 당 Y 이동 값
    Vector3 defPos; // 초기 위치
    bool isReverse = false; // 반전 여부
    // Start is called before the first frame update
    void Start()
    {
        // 초기 위치
        defPos = transform.position;
        // 1 프레임에 이동하는 시간
        float timestep = Time.fixedDeltaTime; //fiexed는 함수가 불리는 시간이 고정되어있음 (fixed가 붙으면 무조건 0.02초마다 불림)
        // 1 프레임의 x 이동 값
        perDX = moveX / (1.0f / timestep * times);
        // 1 프레임의 Y 이동 값
        perDY = moveY / (1.0f / timestep * times);

        // 이동거리 / 걸린시간 = 초당 이동거리
        // ex) 5 / 5초 = 1초에 1만큼 이동
        // 1초에 몇 프레임이 동작하는가? 1 / 0.02 = 50
        // 1초에 가는 이동거리 / 프레임 수 = 1 프레임당 이동거리

        if(isMoveWhenOn ) 
        {
            // 처음에는 움직이지 않고 올라가면 움직이기 시작
            isCanMove = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() // 이 안에서 코드를 작성할 땐 무조건 조건 달기!! (계속 꾸준히 불리기 때문!)
    {
        if( isCanMove ) 
        {
            // 이동 중
            float x = transform.position.x;
            float y = transform.position.y;
            bool endX = false; // 초기값은 false로 설정!!
            bool endY = false;
            
            if( isReverse )
            {
                // 반대 방향 이동
                // 이동량이 양수고 이동 위치가 초기 위치보다 작거나
                // 이동량이 음수고 이동 위치가 초기 위치보다 큰 경우
                if((perDX >= 0.0f && x <= defPos.x) || (perDX < 0.0f && x >= defPos.x))
                {
                    // 이동량이 +
                    endX = true; // X 방향 이동 종료 // 도착했을 때만 true!

                }   
                if((perDY >= 0.0f && y <= defPos.y) || (perDY < 0.0f && y >= defPos.y)) 
                    // 1 프레임당 이동거리가 있고(오른쪽으로 가는 경우) 도착해야 되는 x 값보다 커지면 정방향 이동을 종료한다. || 내가 한 프레임당 이동해야 하는 방향이 왼쪽이라면 도착지의 x 값보다 작아지면 정방향 이동을 종료한다.
                {
                    endY = true; // Y 방향 이동 종료
                }
                // 블럭 이동
                transform.Translate(new Vector3(-perDX, -perDY, defPos.z)); // 물리적인 움직임이 아니라 강제로 움직이게 하는 것임 // 반대로 가야되기 때문에(역방향에는) -를 붙임
            }
            else 
            {
                // 이동거리가 홀수면 초기 위치로 돌아 왔을 때 도착점이 달라질 수 있음
                // 정방향 이동
                // 이동량이 양수고 이동 위치가 초기 위치보다 크거나
                // 이동량이 음수고 이동 위치가 초기 + 이동거리 보다 작은 경우
                if((perDX >= 0.0f && x >= defPos.x + moveX) || (perDX < 0.0f && x <= defPos.x + moveX)) 
                {
                    endX = true; // x 방향 이동 종료
                }
                if((perDY >= 0.0f && y >= defPos.y + moveY) || (perDY < 0.0f && y <= defPos.y + moveY))
                {
                    endY = true; // Y 방향 이동 종료
                }
                // 블록 이동
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }

            if( endX && endY )
            {
                // 이동 종료
                if( isReverse )
                {
                    // 위치가 어긋나는 것을 방지하고자 정면 방향 이동으로 돌아가기 전에 초기 위치로 돌리기 // 속도가 빠르면 빠를 수록 위치가 어긋나는 현상이 더 해짐...
                    transform.position = defPos;

                }
                isReverse = !isReverse; // 값을 반전시키기 //isReverse가 true값으로 
                isCanMove = false; // 이동 가능 값을 false로 설정함
                if(isMoveWhenOn == false)
                {
                    // 올라갔을 때 움직이는 값이 꺼진 경우
                    Invoke("Move", weight); // weight만큼 지연 후 다시 이동 // Invoke는 이 시간만큼 기다렸다가 함수를 호출함
                }
            }

        }
    }
    // 이동하게 만들기
    public void Move()
    {
        isCanMove=true;
    }

    // 이동하지 못하게 만들기
    public void Stop()
    {
        isCanMove = false;
    }

    // 접촉 시작
    //private void OnCollisionEnter2D(Collision2D collision) // 플레이어가 무빙 박스에 들어갔을 때 //oncollision은 triger와는 상관없이 충돌이 일어나면 발생 //ontrigger는 trigger 체크가 된 애들만 해당
      private void OnTriggerEnter2D(Collider2D collision) // trigger 기능을 사용하려면 TriggerEnter를 사용해야 된다.
    {
        if(collision.gameObject.tag == "Player") // collision = player 플레이어이면 플레이어를 무브박스의 자식으로 만든다 // 그렇기 때문에 블럭의 스케일 값이 1이 아닌 다른 수치이면 캐릭터의 스케일 값도 변경된다.
        { // 자식으로 넣어야지 박스의 움직임에따라 같이 움직인다. 그렇지 않으면 캐릭터의 위치는 변하지 않는다.
            // 접촉한 것이 플레이어라면 이동 블록의 자식으로 만들기
            collision.transform.SetParent(transform); ;
            if(isMoveWhenOn)
            {
                // 올라갔을 때 움직이는 경우라면
                isCanMove = true; // 이동하게 만들기

            }
        }
    }
    // 접촉 종료
    private void OnCollisionExit2D(Collision2D collision) // 플레이어가 무빙박스에서 떠났을 때
    {
        if(collision.gameObject.tag == "Player")
        {
            // 접촉한 것이 플레이어라면 이동 블록의 자식에서 제외시키기
            collision.transform.SetParent(null); // null : 부모를 두지 않겠다 (루트레벨)
        }
    }
}
