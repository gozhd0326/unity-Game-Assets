using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ���� ������ �� �ʿ� // ���Ŵ����� �� ���� �ѳ��� �� ����

public class ChangeScene : MonoBehaviour
{
    public string sceneName; // �ҷ��� �� // �ν�����â�� ���ڴ�

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �� �ҷ�����
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
