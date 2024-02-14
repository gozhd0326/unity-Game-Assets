using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�� ����� �� �ʿ�

public class GameUIManager : MonoBehaviour
{
    public GameObject mainImage; // �̹����� ��Ƶδ� GameObject
    public Sprite gameOverSpr; // GAME OVER �̹���
    public Sprite gameClearSpr; // GAME CLEAR �̹���
    public GameObject panel; //�г�
    public GameObject restartButton; // RESTART ��ư
    public GameObject nextButton; // NEXT ��ư

    //+++ ���� �߰� +++
    public GameObject scoreText; // ���� �ؽ�Ʈ
    public static int totalScore; // ���� ���� 
    public int stageScore = 0; // �������� ����

    Image titleImage; // �̹����� ǥ���ϴ� Image ������Ʈ

    // +++ �ð����� �߰� +++
    public GameObject timeBar; // �ð� ǥ�� �̹���
    public GameObject timeText; // �ð� �ؽ�Ʈ
    TimeController timeCnt; // TimeController

    // Start is called before the first frame update
    void Start()
    {
        //  �̹��� �����
        Invoke("InactiveImage", 1.0f); // �ݹ��Լ� ���, 1�� �ڿ� �̳�Ƽ���̹����� �ҷ��Ͷ�
        // ��ư(�г�)�� �����
        panel.SetActive(false);

        //+++ ���� �߰� +++
        UpdateScore();

        // +++ �ð����� �߰� +++
        // TimeController ������
        timeCnt = GetComponent<TimeController>();
        if(timeCnt != null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false); // �ð������� ������ ����
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear") //static �� ���� �� ����(���纻�� ���� �� ���� ������ �Լ��� ���� �޾ƿ� �� ���� �ٷ� �д� ������� �� �ָ� ������ �ִ� ���� ������Ʈ(������Ʈ)�� ������ ����)
        {
            // ���� Ŭ����
            mainImage.SetActive(true); // �̹��� ǥ��..
            panel.SetActive(true); // ��ư(�г�)�� ǥ��
            // RESTART ��ư ��ȿȭ(restart��ư�� �ȴ���)
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false; //���ͷ��ͺ��� ��ư�� �Ӽ���
            mainImage.GetComponent<Image>().sprite = gameClearSpr; //���� �̹����� �ٲٰڴ�.
            PlayerController.gameState = "gameend";
            // +++ �ð����� �߰� +++
            if(timeCnt != null)
            {
                timeCnt.isTimeOver = true; // �ð� ī��Ʈ ����
            }

            // +++ ���� ǥ�� +++
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore(); // ���� ���� ( ������ UI�� �ݿ� )
        }
        else if (PlayerController.gameState == "gameover")

        {
            // ���� ����
            mainImage.SetActive(true); // �̹��� ǥ��
            panel.SetActive(true); // ��ư(�г�)�� ǥ��
            // NEXT ��ư�� ��Ȱ��
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";
            // +++ �ð����� �߰� +++
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // �ð� ī��Ʈ ����
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            // ���� ��
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //PlayerController ��������
            PlayerController playerCnt = player.GetComponent<PlayerController>(); //���⼭�� ������Ʈ�� Ŭ������
            // +++ ���� �߰� +++ 
            if(playerCnt.score != 0) // �÷��̾ ������ ��Ҵ��� ��� Ȯ��
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
            // +++ �ð����� �߰� +++
            // �ð� ����
            if(timeCnt != null) 
            {
                if(timeCnt.gameTime > 0.0f)
                {
                    // ������ �Ҵ��Ͽ� �Ҽ��� ���ϸ� ����
                    int time = (int)timeCnt.displayTime;
                    // �ð� ����
                    timeText.GetComponent<Text>().text = time.ToString();
                    // Ÿ�� ����
                    if(time == 0)
                    {
                        playerCnt.GameOver(); // ���� ����
                    }
                }
            }
        }
    }
        // �̹��� �����
        void InactiveImage()
        { 
            mainImage.SetActive(false); 
        }

    // +++ ���� �߰� +++
    void UpdateScore() 
    {
        int score = stageScore + totalScore; // ���� ������������ ���� ������ �� �ջ��ؾ� �Ǳ� ����
        scoreText.GetComponent<Text>().text = score.ToString(); //text�� ���� ������ String���� �ٲٰ� text�� ���� 
    }
}
