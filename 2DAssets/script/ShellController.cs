using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f; // 제거할 시간 지정

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime); // 제거 설정 //(제거할오브젝트,시간) // 인스턴스화(캐논에서 만듦) 되었을 때 start가 불림 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject); // 무언가에 접촉하면 제거 //Edit ->Project Setting -> Physics 2D -> Ground,shell 체크 해제(캐논에서 포탄이 만들어져 콜라이더가 겹쳐도 사라지지 않음)
    }
}
