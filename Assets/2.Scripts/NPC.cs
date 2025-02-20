using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private TextMeshProUGUI shadowText;
    private float timerDisplay;
    private int countRobotLeft;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        countRobotLeft = GameObject.FindGameObjectsWithTag("ENEMY").Length;
        questText.text = shadowText.text = $"Help!\nFix the Robots\nLeft : {countRobotLeft}";
        SetDisplayText(); // 여기 안에 값을 안주고 에러 없앨려면 기본 값을 주면 됨
    }
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
    public bool NoticeRobotFixed()
    {
        countRobotLeft--;
        bool isCompleted = (countRobotLeft <= 0);
        if(countRobotLeft <= 0)
        {
            isCompleted = true;
        }
        else
        {
            isCompleted = false;
        }
        SetDisplayText(isCompleted); // 0이하면 true 아니면 false
        return isCompleted;
    }
    public void SetDisplayText(bool isCompleted = false)
    {
        if(isCompleted)
        {
            questText.text = shadowText.text = "Good job!\nYou Success!";
        }
        else
        {
            questText.text = shadowText.text = $"Help!\nFix the Robots\nLeft : {countRobotLeft}";
        }
    }
}
