using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab; // 포대에서 발사되는 포탄 Prefab
    public float delayTime = 3.0f; // 지연 시간 // 한방 쏘고 그 다음 한방까지 걸리는 시간 // 3초가 되면 쏜다.
    public float fireSpeedX = -4.0f; // 발사 벡터 x
    public float fireSpeedY = 0.0f; // 발사 벡터 Y
    public float length = 8.0f;

    GameObject player; // 플레이어
    GameObject gateObj; // 발사구
    float passedTimes = 0; // 경과 시간


    // Start is called before the first frame update
    void Start()
    {
        // 발사구 오브젝트 얻기
        Transform tr = transform.Find("gate");
        gateObj = tr.gameObject; // gate 오브젝트를 말함
        // 플레이어
        player = GameObject.FindGameObjectWithTag("Player"); //start 안에서 작성해야 됨
    }

    // Update is called once per frame
    void Update()
    {
        // 발사 시간 판정
        passedTimes += Time.deltaTime;
        // 거리 확인
        if(CheckLength(player.transform.position))
        {
            if(passedTimes > delayTime)
            {
                // 발사!!
                passedTimes = 0;
                // 발사 위치
                Vector3 pos = new Vector3(gateObj.transform.position.x, gateObj.transform.position.y, transform.position.z);
                //Prefab으로 GameObject 만들기
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity); // 게임 실행 중에 새로운 오브젝트를 만들어 화면에 등장시킴 = instiate. (반드시 Prefab형태로 등록한 녀석만 쓸 수 있음) //쿼터니언 : 회전각도, 
                // 발사 방향
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                Vector2 v = new Vector2(fireSpeedX, fireSpeedY);
                rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }

    // 거리 확인
    bool CheckLength(Vector2 targetPos) //targetPos = player position
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);
        if(length >= d)
        {
            ret = true;

        }
        return ret;
    }
}
