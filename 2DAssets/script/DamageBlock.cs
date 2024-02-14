using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    //스테틱 : 절대 움직이지 않는 고정된 녀석들(게임에 관여하지 않거나 전혀 움직임이 필요없는 애들), 다이나믹과 스테틱을 번갈아가며 사용, 여기서는 스테틱으로 공중에 떠 있다가 다이나믹으로 물리 작용을 받아 떨어짐
    public float length = 0.0f; // 자동 낙하 탐지 거리 //블럭과 플레이어의 거리 // 플레이어와 거리가 가까워지면 떨어지게 되어있음 (4~5정도 추천 , 높이에 따라 다름)
    public bool isDelete = false; // 낙하 후 제거할지 여부
    public GameObject deadObject; //변수이기 때문에 deadObject는 소문자로 작성! 클래스와 오브젝트는  대문자로 작성
    bool isFell = false; // 낙하 플래그 // 떨어져 있느냐
    float fadeTime = 0.5f; // 페이드 아웃 시간 // 0.5초 후에 사라짐

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2D 물리 시물레이션 정지
        Rigidbody2D rbody = GetComponent<Rigidbody2D>(); // 데미지 블록의 리지드바디를 불러옴
        rbody.bodyType = RigidbodyType2D.Static; // 그래서 START로 초기에는 STATIC으로 설정

    }

    // Update is called once per frame
    void Update()
    {
        //UPDATE는 수시로 동작하기 때문에 조건을 달아주는게 좋음
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기
        if (player != null)
        {
            // 플레이어와의 거리 계산
            //float d =Vector2.Distance(transform.position, player.transform.position); // 1번 벡터 포지션에서 2번 벡터 포지션까지 값을 계산 // 이 코드에서는 대각선으로 거리를 측정(1번은 블럭 위치, 2번은 플레이어 위치)
            Vector2 pos = new Vector2(transform.position.x, player.transform.position.y);
            float d = Vector2.Distance(pos, player.transform.position);
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static) // 다이나믹으로 바뀌고 나서도 계속 실행되기 때문에 스테틱이라는 조건을 달아줌 
                {  //Rigidbody2D 물리 시물레이션 시작
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
        if (isFell)
        {
            // 낙하
            // 투명도를 변경해 페이드아웃 효과
            fadeTime -= Time.deltaTime; // 이전 프레임과의 차이만큼 시간 차감 //이전 프레임에서 현재 프레임까지 걸린 시간이 찍힘 // 포탄 같은 것을 사용할 때도 거리를 계산해야 하기 때문에 델타 타임을 이용함
            //틱 : 스케줄러가 도는 시간(cpu마다 다름(미세하게) 그렇기에 보정을 해야 됨
            Color col = GetComponent<SpriteRenderer>().color; // 컬러 값 가져오기
            col.a = fadeTime; // 투명도 변경
            GetComponent<SpriteRenderer>().color = col; // 컬러 값을 재설정
            if (fadeTime <= 0.0f)
            {
                // 0보다 작으면(투명) 제거
                Destroy(gameObject);
            }
        }
    }
        

            


    // 접촉 시작
    void OnCollisionEnter2D(Collision2D collision)
    {
    //데드오브젝트는 오류가 생길 위험성이 있기 때문에 땅에 떨어졌을 때 그 오브젝트를 삭제할 수 있도록 하는 것이 좋음
    //tag 는 string 이지만 layer는 숫자로 관리
        int layer = collision.gameObject.layer;
        if(LayerMask.LayerToName(layer) == "Ground")
        {
            deadObject.SetActive(false);
        }

        if(isDelete) // 인스펙터 창에서 조절할 수 있는 값 // 누구하고던 부딪히면(이즈딜리트가 트루 이면 없애야 됨)
        {
            isFell = true; // 낙하 플래그 true // 일단 플래그를 바꿔놓도록 설정 // 실질적인 설정은 업데이트해서 함
        }
    }
}

