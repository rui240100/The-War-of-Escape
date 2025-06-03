using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 180f;  // �������ԁi�b�j
    private float currentTime;     // ���݂̎c�莞��
    public TextMeshProUGUI timeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= 0)
        {
            GameOver();
        }
        else
        {
            currentTime -= Time.deltaTime;
        }

        // �c�莞�Ԃ�UI�ɕ\��
        timeText.text = "Time Left: " + Mathf.Ceil(currentTime).ToString() + "s";
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    // �c�莞�Ԃ��擾����֐���ǉ�
    public float GetRemainingTime()
    {
        return Mathf.Max(currentTime, 0f); // 0�ȉ��ɂȂ�Ȃ��悤�ɂ���
    }








}
