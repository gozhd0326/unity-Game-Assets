using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f; // �̵� �ӵ�
    public string direction = "left"; // ���� right or left
    public float range = 0.0f; // �����̴� ����
    Vector3 defPos; // ���� ��ġ

    // Start is called before the first frame update
    void Start()
    {
        if(direction == "right")
        {
            transform.localScale = new Vector2(-1, 1); // ���� ����
        }
        // ���� ��ġ
        defPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(range > 0.0f) //range = 0 => ������Ʈ �ڵ尡 ������� ���� // ������ �ٲ� �� �ִ� ���� �浹�� �߻��ؾ� ��
        {
            if(transform.position.x < defPos.x - (range / 2)) //transform.position�� ������ġ��. //����� �������� �� ���� �Դٰ��� ��
            {
                direction = "right";
                transform.localScale = new Vector2(-1, 1); // ���� ����
            }
            if(transform.position.x > defPos.x + (range / 2))
            {
                direction = "left";
                transform.localScale = new Vector2(1, 1); // ���� ����

            }
        }
    }

    void FixedUpdate()
    {

        // �ӵ� ����
        //Rigidbody2D ��������
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        if(direction == "right") // ���ǵ��� ����
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y);
        }
        else 
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y);
        }
    }

    // ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1,1); // ���� ����
        }
        else
        {
            direction = "right";
                transform.localScale = new Vector2(-1, 1); // ���� ����
        }
    }
}