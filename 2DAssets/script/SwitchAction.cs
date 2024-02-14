using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAction : MonoBehaviour
{

    public GameObject targetMoveBlock;
    public Sprite imageOn;
    public Sprite imageOff;
    public bool on = false; // 스위치 상태( true : 눌린 상태 false : 눌리지 않은 상태)
    // Start is called before the first frame update
    void Start()
    {
        if(on)
        {
            GetComponent<SpriteRenderer>().sprite = imageOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imageOff;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 접촉 시작
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(on)
            {
                on = false;
                GetComponent<SpriteRenderer>().sprite = imageOff;
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock>();
                movBlock.Stop();
            }
            else
            {
                on = true;
                GetComponent<SpriteRenderer>().sprite = imageOn;
                MovingBlock movBlock = targetMoveBlock.GetComponent<MovingBlock> ();
                movBlock.Move();
            }
        }
    }
}
//new를 통해 만들어지는 객체 = 인스턴스 (원래는 메모리에 없던 녀석을 메모리에 추가함)