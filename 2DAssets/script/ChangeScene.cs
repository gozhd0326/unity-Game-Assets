using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬을 변경할 때 필요 // 씬매니저는 각 씬을 넘나들 수 있음

public class ChangeScene : MonoBehaviour
{
    public string sceneName; // 불러올 씬 // 인스펙터창에 쓰겠다

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 씬 불러오기
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
