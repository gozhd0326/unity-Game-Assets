using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    //�̵��Ÿ� x �ð� = �ӵ�
    public float moveX = 0.0f; // x �̵� �Ÿ�
    public float moveY = 0.0f; // y �̵� �Ÿ�
    public float times = 0.0f; // �ð�
    public float weight = 0.0f; // ���� �ð�
    public bool isMoveWhenOn = false; // �ö� ���� �� �����̱� (�갡 Ʈ���̸� isCanMove�� false�� �ٲ���� ��)

    public bool isCanMove = true; // ������ (��� ������)
    float perDX; // 1������ �� x �̵� ��
    float perDY; // 1 ������ �� Y �̵� ��
    Vector3 defPos; // �ʱ� ��ġ
    bool isReverse = false; // ���� ����
    // Start is called before the first frame update
    void Start()
    {
        // �ʱ� ��ġ
        defPos = transform.position;
        // 1 �����ӿ� �̵��ϴ� �ð�
        float timestep = Time.fixedDeltaTime; //fiexed�� �Լ��� �Ҹ��� �ð��� �����Ǿ����� (fixed�� ������ ������ 0.02�ʸ��� �Ҹ�)
        // 1 �������� x �̵� ��
        perDX = moveX / (1.0f / timestep * times);
        // 1 �������� Y �̵� ��
        perDY = moveY / (1.0f / timestep * times);

        // �̵��Ÿ� / �ɸ��ð� = �ʴ� �̵��Ÿ�
        // ex) 5 / 5�� = 1�ʿ� 1��ŭ �̵�
        // 1�ʿ� �� �������� �����ϴ°�? 1 / 0.02 = 50
        // 1�ʿ� ���� �̵��Ÿ� / ������ �� = 1 �����Ӵ� �̵��Ÿ�

        if(isMoveWhenOn ) 
        {
            // ó������ �������� �ʰ� �ö󰡸� �����̱� ����
            isCanMove = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() // �� �ȿ��� �ڵ带 �ۼ��� �� ������ ���� �ޱ�!! (��� ������ �Ҹ��� ����!)
    {
        if( isCanMove ) 
        {
            // �̵� ��
            float x = transform.position.x;
            float y = transform.position.y;
            bool endX = false; // �ʱⰪ�� false�� ����!!
            bool endY = false;
            
            if( isReverse )
            {
                // �ݴ� ���� �̵�
                // �̵����� ����� �̵� ��ġ�� �ʱ� ��ġ���� �۰ų�
                // �̵����� ������ �̵� ��ġ�� �ʱ� ��ġ���� ū ���
                if((perDX >= 0.0f && x <= defPos.x) || (perDX < 0.0f && x >= defPos.x))
                {
                    // �̵����� +
                    endX = true; // X ���� �̵� ���� // �������� ���� true!

                }   
                if((perDY >= 0.0f && y <= defPos.y) || (perDY < 0.0f && y >= defPos.y)) 
                    // 1 �����Ӵ� �̵��Ÿ��� �ְ�(���������� ���� ���) �����ؾ� �Ǵ� x ������ Ŀ���� ������ �̵��� �����Ѵ�. || ���� �� �����Ӵ� �̵��ؾ� �ϴ� ������ �����̶�� �������� x ������ �۾����� ������ �̵��� �����Ѵ�.
                {
                    endY = true; // Y ���� �̵� ����
                }
                // �� �̵�
                transform.Translate(new Vector3(-perDX, -perDY, defPos.z)); // �������� �������� �ƴ϶� ������ �����̰� �ϴ� ���� // �ݴ�� ���ߵǱ� ������(�����⿡��) -�� ����
            }
            else 
            {
                // �̵��Ÿ��� Ȧ���� �ʱ� ��ġ�� ���� ���� �� �������� �޶��� �� ����
                // ������ �̵�
                // �̵����� ����� �̵� ��ġ�� �ʱ� ��ġ���� ũ�ų�
                // �̵����� ������ �̵� ��ġ�� �ʱ� + �̵��Ÿ� ���� ���� ���
                if((perDX >= 0.0f && x >= defPos.x + moveX) || (perDX < 0.0f && x <= defPos.x + moveX)) 
                {
                    endX = true; // x ���� �̵� ����
                }
                if((perDY >= 0.0f && y >= defPos.y + moveY) || (perDY < 0.0f && y <= defPos.y + moveY))
                {
                    endY = true; // Y ���� �̵� ����
                }
                // ��� �̵�
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }

            if( endX && endY )
            {
                // �̵� ����
                if( isReverse )
                {
                    // ��ġ�� ��߳��� ���� �����ϰ��� ���� ���� �̵����� ���ư��� ���� �ʱ� ��ġ�� ������ // �ӵ��� ������ ���� ���� ��ġ�� ��߳��� ������ �� ����...
                    transform.position = defPos;

                }
                isReverse = !isReverse; // ���� ������Ű�� //isReverse�� true������ 
                isCanMove = false; // �̵� ���� ���� false�� ������
                if(isMoveWhenOn == false)
                {
                    // �ö��� �� �����̴� ���� ���� ���
                    Invoke("Move", weight); // weight��ŭ ���� �� �ٽ� �̵� // Invoke�� �� �ð���ŭ ��ٷȴٰ� �Լ��� ȣ����
                }
            }

        }
    }
    // �̵��ϰ� �����
    public void Move()
    {
        isCanMove=true;
    }

    // �̵����� ���ϰ� �����
    public void Stop()
    {
        isCanMove = false;
    }

    // ���� ����
    //private void OnCollisionEnter2D(Collision2D collision) // �÷��̾ ���� �ڽ��� ���� �� //oncollision�� triger�ʹ� ������� �浹�� �Ͼ�� �߻� //ontrigger�� trigger üũ�� �� �ֵ鸸 �ش�
      private void OnTriggerEnter2D(Collider2D collision) // trigger ����� ����Ϸ��� TriggerEnter�� ����ؾ� �ȴ�.
    {
        if(collision.gameObject.tag == "Player") // collision = player �÷��̾��̸� �÷��̾ ����ڽ��� �ڽ����� ����� // �׷��� ������ ���� ������ ���� 1�� �ƴ� �ٸ� ��ġ�̸� ĳ������ ������ ���� ����ȴ�.
        { // �ڽ����� �־���� �ڽ��� �����ӿ����� ���� �����δ�. �׷��� ������ ĳ������ ��ġ�� ������ �ʴ´�.
            // ������ ���� �÷��̾��� �̵� ����� �ڽ����� �����
            collision.transform.SetParent(transform); ;
            if(isMoveWhenOn)
            {
                // �ö��� �� �����̴� �����
                isCanMove = true; // �̵��ϰ� �����

            }
        }
    }
    // ���� ����
    private void OnCollisionExit2D(Collision2D collision) // �÷��̾ �����ڽ����� ������ ��
    {
        if(collision.gameObject.tag == "Player")
        {
            // ������ ���� �÷��̾��� �̵� ����� �ڽĿ��� ���ܽ�Ű��
            collision.transform.SetParent(null); // null : �θ� ���� �ʰڴ� (��Ʈ����)
        }
    }
}
