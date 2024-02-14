using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MonoBehaviour
{
    //����ƽ : ���� �������� �ʴ� ������ �༮��(���ӿ� �������� �ʰų� ���� �������� �ʿ���� �ֵ�), ���̳��Ͱ� ����ƽ�� �����ư��� ���, ���⼭�� ����ƽ���� ���߿� �� �ִٰ� ���̳������� ���� �ۿ��� �޾� ������
    public float length = 0.0f; // �ڵ� ���� Ž�� �Ÿ� //���� �÷��̾��� �Ÿ� // �÷��̾�� �Ÿ��� ��������� �������� �Ǿ����� (4~5���� ��õ , ���̿� ���� �ٸ�)
    public bool isDelete = false; // ���� �� �������� ����
    public GameObject deadObject; //�����̱� ������ deadObject�� �ҹ��ڷ� �ۼ�! Ŭ������ ������Ʈ��  �빮�ڷ� �ۼ�
    bool isFell = false; // ���� �÷��� // ������ �ִ���
    float fadeTime = 0.5f; // ���̵� �ƿ� �ð� // 0.5�� �Ŀ� �����

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2D ���� �ù����̼� ����
        Rigidbody2D rbody = GetComponent<Rigidbody2D>(); // ������ ����� ������ٵ� �ҷ���
        rbody.bodyType = RigidbodyType2D.Static; // �׷��� START�� �ʱ⿡�� STATIC���� ����

    }

    // Update is called once per frame
    void Update()
    {
        //UPDATE�� ���÷� �����ϱ� ������ ������ �޾��ִ°� ����
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // �÷��̾� ã��
        if (player != null)
        {
            // �÷��̾���� �Ÿ� ���
            //float d =Vector2.Distance(transform.position, player.transform.position); // 1�� ���� �����ǿ��� 2�� ���� �����Ǳ��� ���� ��� // �� �ڵ忡���� �밢������ �Ÿ��� ����(1���� �� ��ġ, 2���� �÷��̾� ��ġ)
            Vector2 pos = new Vector2(transform.position.x, player.transform.position.y);
            float d = Vector2.Distance(pos, player.transform.position);
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static) // ���̳������� �ٲ�� ������ ��� ����Ǳ� ������ ����ƽ�̶�� ������ �޾��� 
                {  //Rigidbody2D ���� �ù����̼� ����
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
        if (isFell)
        {
            // ����
            // ������ ������ ���̵�ƿ� ȿ��
            fadeTime -= Time.deltaTime; // ���� �����Ӱ��� ���̸�ŭ �ð� ���� //���� �����ӿ��� ���� �����ӱ��� �ɸ� �ð��� ���� // ��ź ���� ���� ����� ���� �Ÿ��� ����ؾ� �ϱ� ������ ��Ÿ Ÿ���� �̿���
            //ƽ : �����ٷ��� ���� �ð�(cpu���� �ٸ�(�̼��ϰ�) �׷��⿡ ������ �ؾ� ��
            Color col = GetComponent<SpriteRenderer>().color; // �÷� �� ��������
            col.a = fadeTime; // ���� ����
            GetComponent<SpriteRenderer>().color = col; // �÷� ���� �缳��
            if (fadeTime <= 0.0f)
            {
                // 0���� ������(����) ����
                Destroy(gameObject);
            }
        }
    }
        

            


    // ���� ����
    void OnCollisionEnter2D(Collision2D collision)
    {
    //���������Ʈ�� ������ ���� ���輺�� �ֱ� ������ ���� �������� �� �� ������Ʈ�� ������ �� �ֵ��� �ϴ� ���� ����
    //tag �� string ������ layer�� ���ڷ� ����
        int layer = collision.gameObject.layer;
        if(LayerMask.LayerToName(layer) == "Ground")
        {
            deadObject.SetActive(false);
        }

        if(isDelete) // �ν����� â���� ������ �� �ִ� �� // �����ϰ�� �ε�����(�������Ʈ�� Ʈ�� �̸� ���־� ��)
        {
            isFell = true; // ���� �÷��� true // �ϴ� �÷��׸� �ٲ������ ���� // �������� ������ ������Ʈ�ؼ� ��
        }
    }
}

