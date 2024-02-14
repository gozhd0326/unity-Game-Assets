using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI를 사용할 때 필요

public class GameUIManager : MonoBehaviour
{
    public GameObject mainImage; // 이미지를 담아두는 GameObject
    public Sprite gameOverSpr; // GAME OVER 이미지
    public Sprite gameClearSpr; // GAME CLEAR 이미지
    public GameObject panel; //패널
    public GameObject restartButton; // RESTART 버튼
    public GameObject nextButton; // NEXT 버튼

    //+++ 점수 추가 +++
    public GameObject scoreText; // 점수 텍스트
    public static int totalScore; // 점수 총합 
    public int stageScore = 0; // 스테이지 점수

    Image titleImage; // 이미지를 표시하는 Image 컴포넌트

    // +++ 시간제한 추가 +++
    public GameObject timeBar; // 시간 표시 이미지
    public GameObject timeText; // 시간 텍스트
    TimeController timeCnt; // TimeController

    // Start is called before the first frame update
    void Start()
    {
        //  이미지 숨기기
        Invoke("InactiveImage", 1.0f); // 콜백함수 등록, 1초 뒤에 이넥티브이미지를 불러와라
        // 버튼(패널)을 숨기기
        panel.SetActive(false);

        //+++ 점수 추가 +++
        UpdateScore();

        // +++ 시간제한 추가 +++
        // TimeController 가져옴
        timeCnt = GetComponent<TimeController>();
        if(timeCnt != null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false); // 시간제한이 없으면 숨김
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear") //static 은 읽을 수 있음(복사본을 만들 수 없기 때문에 함수를 통해 받아올 수 없어 바로 읽는 방법으로 이 애를 가지고 있는 게임 오브젝트(컴포넌트)를 가져와 읽음)
        {
            // 게임 클리어
            mainImage.SetActive(true); // 이미지 표시..
            panel.SetActive(true); // 버튼(패널)을 표시
            // RESTART 버튼 무효화(restart버튼이 안눌림)
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false; //인터렉터블은 버튼의 속성임
            mainImage.GetComponent<Image>().sprite = gameClearSpr; //게임 이미지를 바꾸겠다.
            PlayerController.gameState = "gameend";
            // +++ 시간제한 추가 +++
            if(timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 시간 카운트 중지
            }

            // +++ 점수 표시 +++
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore(); // 점수 갱신 ( 점수를 UI에 반영 )
        }
        else if (PlayerController.gameState == "gameover")

        {
            // 게임 오버
            mainImage.SetActive(true); // 이미지 표시
            panel.SetActive(true); // 버튼(패널)을 표시
            // NEXT 버튼을 비활성
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";
            // +++ 시간제한 추가 +++
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 시간 카운트 중지
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            // 게임 중
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerController 가져오기
            PlayerController playerCnt = player.GetComponent<PlayerController>(); //여기서의 컴포넌트는 클래스임
            // +++ 점수 추가 +++ 
            if(playerCnt.score != 0) // 플레이어가 점수를 모았는지 계속 확인
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
            // +++ 시간제한 추가 +++
            // 시간 갱신
            if(timeCnt != null) 
            {
                if(timeCnt.gameTime > 0.0f)
                {
                    // 정수에 할당하여 소수점 이하를 버림
                    int time = (int)timeCnt.displayTime;
                    // 시간 갱신
                    timeText.GetComponent<Text>().text = time.ToString();
                    // 타임 오버
                    if(time == 0)
                    {
                        playerCnt.GameOver(); // 게임 오버
                    }
                }
            }
        }
    }
        // 이미지 숨기기
        void InactiveImage()
        { 
            mainImage.SetActive(false); 
        }

    // +++ 점수 추가 +++
    void UpdateScore() 
    {
        int score = stageScore + totalScore; // 이전 스테이지에서 얻은 점수를 다 합산해야 되기 때문
        scoreText.GetComponent<Text>().text = score.ToString(); //text를 쓰기 때문에 String으로 바꾸고 text에 넣음 
    }
}
