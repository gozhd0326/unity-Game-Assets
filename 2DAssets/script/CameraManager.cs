using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public float leftLimit = 0.0f; // ���� ��ũ�� ����
    public float rightLimit = 0.0f; // ������ ��ũ�� ����
    public float topLimit = 0.0f; // �� ��ũ�� ����
    public float bottomLimit = 0.0f; // �Ʒ� ��ũ�� ����
    public GameObject player; ////�÷��̾�� �̱������� ����°� ����(���� ��ü�� �־ �÷��̾�� ������ �ʱ� ������!!) 

    public GameObject subScreen; // ���� ��ũ��

    public bool isForceScrollX = false; // X�� ���� ��ũ�� �÷���
    public float forceScrollSpeedX = 0.05f; // 1�ʰ� ������ x�� �Ÿ�
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // �÷��̾� ã�� �׷��� ������Ʈ�� �ƴ� ��ŸƮ�� ����
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            // ī�޶��� ��ǥ ����
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z; // ���⼭�� ī�޶��� z���� �������� �ʰڴ�. (�׷��� �������� ����!)

            if(isForceScrollX)
            {
                // ���� ���� ��ũ��
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime); // �ӵ� * ��ŸŸ�� => �����Ӵ� �����̴� �Ÿ�( ������ ���� = ������ �ӵ� ) // Ư���� ���ǵ带 �༭ ī�޶� ������ �����Բ� ������ ����
            }
            // ���� ���� ����ȭ
            // �� ���� �̵� ���� ����
            if (x < leftLimit) 
            { 
                x = leftLimit; 
            }
            else if (x > rightLimit) 
            {
                x = rightLimit;
            }
            // ���� ���� ����ȭ
            // �� �Ʒ��� �̵� ���� ����
            if(y < bottomLimit)
            { 
                y = bottomLimit;
            }
            else if(y > topLimit)
            {  
                y = topLimit;
            }
            // ī�޶� ��ġ�� Vector3 �����
            transform.position = new Vector3(x, y, z);
            
            // ���� ��ũ�� ��ũ��
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
