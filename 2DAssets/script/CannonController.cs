using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab; // ���뿡�� �߻�Ǵ� ��ź Prefab
    public float delayTime = 3.0f; // ���� �ð� // �ѹ� ��� �� ���� �ѹ���� �ɸ��� �ð� // 3�ʰ� �Ǹ� ���.
    public float fireSpeedX = -4.0f; // �߻� ���� x
    public float fireSpeedY = 0.0f; // �߻� ���� Y
    public float length = 8.0f;

    GameObject player; // �÷��̾�
    GameObject gateObj; // �߻籸
    float passedTimes = 0; // ��� �ð�


    // Start is called before the first frame update
    void Start()
    {
        // �߻籸 ������Ʈ ���
        Transform tr = transform.Find("gate");
        gateObj = tr.gameObject; // gate ������Ʈ�� ����
        // �÷��̾�
        player = GameObject.FindGameObjectWithTag("Player"); //start �ȿ��� �ۼ��ؾ� ��
    }

    // Update is called once per frame
    void Update()
    {
        // �߻� �ð� ����
        passedTimes += Time.deltaTime;
        // �Ÿ� Ȯ��
        if(CheckLength(player.transform.position))
        {
            if(passedTimes > delayTime)
            {
                // �߻�!!
                passedTimes = 0;
                // �߻� ��ġ
                Vector3 pos = new Vector3(gateObj.transform.position.x, gateObj.transform.position.y, transform.position.z);
                //Prefab���� GameObject �����
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity); // ���� ���� �߿� ���ο� ������Ʈ�� ����� ȭ�鿡 �����Ŵ = instiate. (�ݵ�� Prefab���·� ����� �༮�� �� �� ����) //���ʹϾ� : ȸ������, 
                // �߻� ����
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                Vector2 v = new Vector2(fireSpeedX, fireSpeedY);
                rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }

    // �Ÿ� Ȯ��
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
