using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour //Start�� Update�� ����ϱ� ���ؼ� �������̺� ��ӹ��� �ڽ��̾�� ��
{
    Rigidbody2D rbody; //robody��� ���� ���� // �ƹ��͵� ���������� ������ private��
    float axisH = 0.0f; //float�� Ȧ����� ��, ������ �ڿ� f �� �ٿ��� ��. //axis : �Է�Ű //H ȣ�������� : ������� // V ��Ƽ�� : �������� 
                        //���� ���� ������, ���� ���� ���� // ���̽�ƽ �����ɷ� ������ �̼��ϰ� ������ ��� �̵��� ��ݻ�� ������ ���� �����ϱ� ���� �Ҽ��� ���

    public float speed = 3.0f; //public�� ����ϸ� �ν�����â(������Ʈâ)���� ���� �ٲ� �� �ְ� ��
    public float jump = 9.0f; // ������
    public LayerMask groundLayer; // ������ �� �ִ� ���̾�
    bool goJump = false; // ���� ���� �÷���
    bool onGround = false; // ���鿡 �� �ִ� �÷���
    public static string gameState = "playing"; //���� ���� //����ƽ�� �̱����� ����� ����� ! ������..! �� �̻� ���纻�� ���� ���� ���� ���Ӱ� ���� ���� ���� ���� ����ƽ���� ������ �� �ϳ��ۿ� ������ �� ����
    //��ŸƮ ��ư�� ������ ���� ����, ������ ���� ������ ����(�̱���) //���°����� �ִ� ���� ���� �ϳ������� �����ؾ� �ϱ⶧���� ����ƽ���� ����! �׷��� ������ ����(ȥ��)�� �� �� ����

    //�ִϸ��̼� ó��
    Animator animator; //�ִϸ�����
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";
    public int score = 0; // ����

    // Start is called before the first frame update
    void Start() //���� �ʱ�ȭ ��ų �ڵ� (�� �ڵ尡 ���� ������ �� �Ŀ� ������ �÷��� ��) //��������� ���� ������ ���� �ʱ� ������ �ǵ����̸� �������� �ο����� �ʴ°� ����
    {
        //������ �÷��� �Ǳ� ���� rbody�� �ʱ�ȭ ��
        //Attach(����ġ) :��ũ��Ʈ�� ��ü�� ����.
        rbody = this.GetComponent<Rigidbody2D>(); //this �� ���� �� ���� ������Ʈ , Rigidbody2D ������Ʈ�� �޾� �´�. 

        // Animator ��������
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing"; //���� ��
    
    }

    // Update is called once per frame
    // ��ư �̺�Ʈ�� ���� ������Ʈ�� ����
    void Update() // ��� ȣ���(�ֱ�������, �뷫 0.02��..?) //ȭ�� �������� ���ҽ��� ���� �����ϴµ� �����췯�� �δ��� �Ǳ� ������ Ư�� ���ҽ��� ���� �ʿ���ϴ�(���� �׸���)���� �������� ���� �ʿ�� �Ҽ��� ȣ�� �󵵼��� ��������
    {
        if(gameState != "playing")
        {
            return; //Ư���� ��Ȳ�� ���� �� �̻� ��Ʈ�ѷ��� ������Ʈ�� ���� �ʰڴ�. 
        }

    
        //���� ������ �Է� Ȯ��
        axisH = Input.GetAxisRaw("Horizontal"); //������ ��ǥ���� �޾ƿͶ� 

        //���� ����
        if (axisH > 0.0f) //�̷��̹����� ����ϱ� ���� �ڵ�
        {
            //������ �̵�
            transform.localScale = new Vector2(1, 1); //��ǥ�� ��� Vector�� ǥ�� ( x, y, z) Vector2D(x,y) ,Vector3D(x,y,z) //ĳ���Ͱ� �������� �ٶ�
        }
        else if (axisH < 0.0f)
        {
            //���� �̵�
            transform.localScale = new Vector2(-1, 1); //ĳ���Ͱ� ������ �ٶ�
        }
        // Debug.Log(axisH);
        //Debug.Log("Update");//�׳� ȭ�鿡 �������� �͵� 

        //ĳ���� �����ϱ�
        if (Input.GetButtonDown("Jump"))
        {
            Jump();//����
        }

    }


    //��ǲ �̺�Ʈ�� ���� ����
    //LineCast �� ���ҽ� �Ҹ� ���� ������ 
    void FixedUpdate() //FixedUpdate : �ݵ�� 0.02�ʸ��� ������Ʈ �� //�������� ����� �ʿ��� ������ (ĳ����, �Ѿ�, ���ư��� ����� ... ��� �����̴� ��ü�� ����)
    {
        if(gameState != "playing")
        {
            return; // �Ʒ� �ڵ带 ���� ���ϰڴ�. ȣ��Ǵ� �Լ��� ���� ���κп��� ����ؾ� ��. �� �ڵ尡 �Ʒ��� �ִٸ� ��ǲ���� �޴� �Լ����� ����Ǿ ������ ����� �Կ��� �ұ��ϰ� ��� ��Ʈ���� ��
        }

        //���� ����
        //front z���� 1
        //up�� y���� 1
        //laycast ���� �߿� �ϳ��� ����ĳ��Ʈ�� ����(����:���� ������ ��)
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        if (onGround || axisH != 0)// ������ �ϰ� ���� �� ����Ű�� ���� �� �ֵ��� �ϴ� �ڵ�       axisH != 0 �����̴� ����
        {
            //���� �� or �ӵ��� 0 �ƴ�
            //�ӵ� �����ϱ�
            //������ �Ӽ��� ���� Rigidbody2D�� �������� rbody�� ���
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y); //�߱ݾ߱� ���� ��
            //speed* axisH : x�� �ӵ� 
            //velocity = vector 
            //Debug.Log(rbody.velocity.y);
            //Debug.Log(FixedUpdate);

        }
        if (onGround && goJump) // ����Ű�� ������ �±׶��� ���� //�Ѿ��� �����ų� ���� �����ų� ���...
        {
            //���� ������ ���� Ű ����
            //�����ϱ�
            Debug.Log("����!");
            Vector2 jumpPw = new Vector2(0, jump); //������ ���� ���� ����
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); // �������� �� ���ϱ� //AddForce: �ѹ濡 �ִ� �Ŀ� // jumpPw : �Ŀ��� //���� ������ �ִ� �������� ���� �����Ͽ� �����
            goJump = false; // ���� �÷��� ����
        }
        if (onGround)
        {
            //���� ��
            if (axisH == 0)
            {
                nowAnime = stopAnime; //����

            }
            else
            {
                nowAnime = moveAnime; //�̵�
            }
        }
        else
        {
            nowAnime = jumpAnime; //����
        }

        if(nowAnime != oldAnime) //0.02 ���� �Ҹ��� ������ ���� ���� �ִϸ��̼ǰ� ���� ���ϸ��̼��� ���ٸ� �ٽ� ���� ��Ű�� ���� �ٸ��ٸ� ���� �����ض�
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); //�ִϸ��̼� ���
        }

    }

    //����
    public void Jump()
    {
        goJump = true; //���� �÷��� �ѱ�
        Debug.Log("���� ��ư ����!");
    }
    
    //���� ����
    private void OnTriggerEnter2D(Collider2D collision) //�ݶ��̴����� �浹�Ǹ� ��Ʈ���Ű� �߻�
    { 
        if (collision.gameObject.tag == "Goal") //�ڽ��� �θ��� ���ӿ�����Ʈ�� ������ �ִ� �±׸� ���� �� �ִ� �ڵ� 
        {
            Goal(); //��

        }
        else if(collision.gameObject.tag == "Dead")
        {
            GameOver(); //���ӿ���
        }
        else if (collision.gameObject.tag == "ScoreItem")
        { // ���� ������
          // ItemData ��������
          ItemData item = collision.gameObject.GetComponent<ItemData>();
          // ���� ���
          score = item.value;
          // ������ ����
          Destroy(collision.gameObject); // �ƿ� ���ӿ��� ����(�޸𸮿��� ����)
        }
        
    
    }
    //��
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear"; //�� �̻� �÷��� ���°� �ƴ�!
        GameStop(); //���� ����
    }
    //���� ����
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop(); // ���� ����
       // =======================
       // ���� ���� ����
       // =======================
       //�÷��̾��� �浹 ���� ��Ȱ��
       GetComponent<CapsuleCollider2D>().enabled = false; //�� ���� Ƣ�� �ι�°���ʹ� �浹�� ��Ȱ��ȭ �Ǿ �ٽ� Ƣ������� ����
       //�÷��̾ ���� Ƣ�� ������ �ϴ� ����
       rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
//���� ����
void GameStop()
{
    //Rigidbody2D ��������
    Rigidbody2D rbody = GetComponent<Rigidbody2D>();
    //�ӵ��� 0���� �Ͽ� ���� ����
    rbody.velocity = new Vector2(0, 0);

}
}

//������
//����ĳ��Ʈ
//����Ʈ���� => �浹�� �Ͼ�� �� ������ ������ �浹�� ���� �̺�Ʈ�� �˷���
//���͸��� : ����
// ������ : ���� ���� ( 0�̸� ������ ����, 1�̸� ������ ���ϰ� ����)
//�ٿ�Ͻ� : ź��