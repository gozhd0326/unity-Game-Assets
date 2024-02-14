using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public float leftLimit = 0.0f; // 왼쪽 스크롤 제한
    public float rightLimit = 0.0f; // 오른쪽 스크롤 제한
    public float topLimit = 0.0f; // 위 스크롤 제한
    public float bottomLimit = 0.0f; // 아래 스크롤 제한
    public GameObject player; ////플레이어는 싱글턴으로 만드는게 맞음(게임 전체에 있어서 플레이어는 변하지 않기 때문에!!) 

    public GameObject subScreen; // 서브 스크린

    public bool isForceScrollX = false; // X축 강제 스크롤 플래그
    public float forceScrollSpeedX = 0.05f; // 1초간 움직일 x의 거리
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기 그래서 업데이트가 아닌 스타트에 넣음
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            // 카메라의 좌표 갱신
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z; // 여기서는 카메라의 z값을 수정하지 않겠다. (그래서 원래값을 넣음!)

            if(isForceScrollX)
            {
                // 가로 강제 스크롤
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime); // 속도 * 델타타임 => 프레임당 움직이는 거리( 벡터의 길이 = 벡터의 속도 ) // 특정한 스피드를 줘서 카메라가 앞으로 나가게끔 강제로 설정
            }
            // 가로 방향 동기화
            // 양 끝에 이동 제한 적용
            if (x < leftLimit) 
            { 
                x = leftLimit; 
            }
            else if (x > rightLimit) 
            {
                x = rightLimit;
            }
            // 세로 방향 동기화
            // 위 아래에 이동 제한 적용
            if(y < bottomLimit)
            { 
                y = bottomLimit;
            }
            else if(y > topLimit)
            {  
                y = topLimit;
            }
            // 카메라 위치의 Vector3 만들기
            transform.position = new Vector3(x, y, z);
            
            // 서브 스크린 스크롤
            if(subScreen != null) 
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3(x / 2.0f, y, z);
                subScreen.transform.localPosition = v;
            }
        }
        
    }
}
