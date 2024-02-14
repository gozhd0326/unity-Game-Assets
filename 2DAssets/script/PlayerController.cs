using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour //Start와 Update를 사용하기 위해선 모노비헤이비어를 상속받은 자식이어야 함
{
    Rigidbody2D rbody; //robody라는 변수 선언 // 아무것도 쓰지않으면 무조건 private임
    float axisH = 0.0f; //float은 홀수라는 뜻, 무조건 뒤에 f 를 붙여야 함. //axis : 입력키 //H 호라이즌털 : 수평방향 // V 버티컬 : 수직방향 
                        //양의 값은 오른쪽, 음의 값은 왼쪽 // 조이스틱 같은걸로 방향을 미세하게 조정할 경우 이동을 살금살금 빠르게 조절 가능하기 위해 소숫점 사용

    public float speed = 3.0f; //public을 사용하면 인스펙터창(컴포넌트창)에서 값을 바꿀 수 있게 됨
    public float jump = 9.0f; // 점프력
    public LayerMask groundLayer; // 착지할 수 있는 레이어
    bool goJump = false; // 점프 개시 플래그
    bool onGround = false; // 지면에 서 있는 플래그
    public static string gameState = "playing"; //게임 상태 //스테틱은 싱글턴을 만드는 방법임 ! 빨라짐..! 더 이상 복사본을 만들 수도 없고 새롭게 만들 수도 없음 오직 스테틱으로 선언한 애 하나밖에 존재할 수 없음
    //스타트 버튼을 누르는 순간 생성, 게임이 끝날 때까지 존재(싱글턴) //상태관리에 있는 값은 오직 하나만으로 존재해야 하기때문에 스테틱으로 관리! 그러지 않으면 오류(혼란)이 올 수 있음

    //애니메이션 처리
    Animator animator; //애니메이터
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";
    public int score = 0; // 점수

    // Start is called before the first frame update
    void Start() //내가 초기화 시킬 코드 (이 코드가 먼저 실행이 된 후에 게임이 플레이 됨) //실행순서가 딱히 정해져 있지 않기 때문에 되도록이면 의존성을 부여하지 않는게 좋다
    {
        //게임이 플레이 되기 전에 rbody가 초기화 됨
        //Attach(어테치) :스크립트를 객체에 적용.
        rbody = this.GetComponent<Rigidbody2D>(); //this 는 지금 이 게임 오브젝트 , Rigidbody2D 컴포넌트를 받아 온다. 

        // Animator 가져오기
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing"; //게임 중
    
    }

    // Update is called once per frame
    // 버튼 이벤트는 전부 업데이트에 넣음
    void Update() // 계속 호출됨(주기적으로, 대략 0.02초..?) //화면 렌더링이 리소스를 많이 차지하는데 스케쥴러에 부담이 되기 때문에 특히 리소스를 많이 필요로하는(빛과 그림자)같은 렌더링을 많이 필요로 할수록 호출 빈도수는 적어진다
    {
        if(gameState != "playing")
        {
            return; //특정한 상황이 오면 더 이상 컨트롤러는 업데이트는 하지 않겠다. 
        }

    
        //수평 방향의 입력 확인
        axisH = Input.GetAxisRaw("Horizontal"); //찍히는 좌표값을 받아와라 

        //방향 조절
        if (axisH > 0.0f) //미러이미지를 사용하기 위한 코드
        {
            //오른쪽 이동
            transform.localScale = new Vector2(1, 1); //좌표는 모드 Vector로 표기 ( x, y, z) Vector2D(x,y) ,Vector3D(x,y,z) //캐릭터가 오른쪽을 바라봄
        }
        else if (axisH < 0.0f)
        {
            //왼쪽 이동
            transform.localScale = new Vector2(-1, 1); //캐릭터가 왼쪽을 바라봄
        }
        // Debug.Log(axisH);
        //Debug.Log("Update");//그냥 화면에 보여지는 것들 

        //캐릭터 점프하기
        if (Input.GetButtonDown("Jump"))
        {
            Jump();//점프
        }

    }


    //인풋 이벤트는 넣지 않음
    //LineCast 도 리소스 소모가 많기 때문에 
    void FixedUpdate() //FixedUpdate : 반드시 0.02초마다 업데이트 됨 //물리적인 계산이 필요할 때마다 (캐릭터, 총알, 날아가는 비행기 ... 등등 움직이는 물체에 적용)
    {
        if(gameState != "playing")
        {
            return; // 아래 코드를 실행 안하겠다. 호출되는 함수의 제일 윗부분에서 사용해야 됨. 이 코드가 아래에 있다면 인풋값을 받는 함수들이 실행되어서 게임이 멈춰야 함에도 불구하고 계속 컨트롤이 됨
        }

        //착지 판정
        //front z값이 1
        //up은 y값이 1
        //laycast 종류 중에 하나로 라인캐스트가 있음(래이:빛이 나가는 것)
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        if (onGround || axisH != 0)// 점프를 하고 있을 때 방향키가 먹을 수 있도록 하는 코드       axisH != 0 움직이는 상태
        {
            //지면 위 or 속도가 0 아님
            //속도 갱신하기
            //물리적 속성을 가진 Rigidbody2D를 재정의한 rbody를 사용
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y); //야금야금 힘을 줌
            //speed* axisH : x축 속도 
            //velocity = vector 
            //Debug.Log(rbody.velocity.y);
            //Debug.Log(FixedUpdate);

        }
        if (onGround && goJump) // 점프키를 눌렀고 온그라운드 상태 //총알을 누르거나 공을 던지거나 등등...
        {
            //지면 위에서 점프 키 눌림
            //점프하기
            Debug.Log("점프!");
            Vector2 jumpPw = new Vector2(0, jump); //점프를 위한 벡터 생성
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); // 순간적인 힘 가하기 //AddForce: 한방에 주는 파워 // jumpPw : 파워력 //원래 기존에 있던 물리적인 힘과 결합하여 적용됨
            goJump = false; // 점프 플래그 끄기
        }
        if (onGround)
        {
            //지면 위
            if (axisH == 0)
            {
                nowAnime = stopAnime; //정지

            }
            else
            {
                nowAnime = moveAnime; //이동
            }
        }
        else
        {
            nowAnime = jumpAnime; //공중
        }

        if(nowAnime != oldAnime) //0.02 마다 불리기 때문에 지금 현재 애니메이션과 현재 에니메이션이 같다면 다시 실행 시키지 말고 다르다면 끊고 실행해라
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); //애니메이션 재생
        }

    }

    //점프
    public void Jump()
    {
        goJump = true; //점프 플래그 켜기
        Debug.Log("점프 버튼 눌림!");
    }
    
    //접촉 시작
    private void OnTriggerEnter2D(Collider2D collision) //콜라이더끼리 충돌되면 온트리거가 발생
    { 
        if (collision.gameObject.tag == "Goal") //자신의 부모임 게임오브젝트가 가지고 있는 태그를 읽을 수 있는 코드 
        {
            Goal(); //골

        }
        else if(collision.gameObject.tag == "Dead")
        {
            GameOver(); //게임오버
        }
        else if (collision.gameObject.tag == "ScoreItem")
        { // 점수 아이템
          // ItemData 가져오기
          ItemData item = collision.gameObject.GetComponent<ItemData>();
          // 점수 얻기
          score = item.value;
          // 아이템 제거
          Destroy(collision.gameObject); // 아예 게임에서 삭제(메모리에서 삭제)
        }
        
    
    }
    //골
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear"; //더 이상 플레잉 상태가 아님!
        GameStop(); //게임 중지
    }
    //게임 오버
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop(); // 게임 중지
       // =======================
       // 게임 오버 연출
       // =======================
       //플레이어의 충돌 판정 비활성
       GetComponent<CapsuleCollider2D>().enabled = false; //한 번만 튀고 두번째부터는 충돌이 비활성화 되어서 다시 튀어오르지 않음
       //플레이어를 위로 튀어 오르게 하는 연출
       rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
//게임 중지
void GameStop()
{
    //Rigidbody2D 가져오기
    Rigidbody2D rbody = GetComponent<Rigidbody2D>();
    //속도를 0으로 하여 강제 정지
    rbody.velocity = new Vector2(0, 0);

}
}

//디텍팅
//레이캐스트
//이즈트리거 => 충돌이 일어났을 때 막지는 않지만 충돌이 나는 이벤트를 알려줌
//머터리얼 : 재질
// 프릭션 : 마찰 개수 ( 0이면 마찰이 없음, 1이면 마찰이 어마어마하게 있음)
//바운스니스 : 탄성