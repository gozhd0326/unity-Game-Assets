using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f; // ������ �ð� ����

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime); // ���� ���� //(�����ҿ�����Ʈ,�ð�) // �ν��Ͻ�ȭ(ĳ���� ����) �Ǿ��� �� start�� �Ҹ� 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject); // ���𰡿� �����ϸ� ���� //Edit ->Project Setting -> Physics 2D -> Ground,shell üũ ����(ĳ���� ��ź�� ������� �ݶ��̴��� ���ĵ� ������� ����)
    }
}
