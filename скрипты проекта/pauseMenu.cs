using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pauseMenu : MonoBehaviour
{
    public GameObject Menu1;
    public GameObject Hint;
    public GameObject OHint;
    private bool EscStat = false;
    private bool HintStat = false;
    void Update() {
        // Активация паузы
        if(Input.GetKeyDown("escape"))
        {
            EscStat = !EscStat;
            Menu1.SetActive(EscStat);
            if(EscStat==true) Time.timeScale = 0;
            else Time.timeScale = 1;
        }
        // Активация подсказки
        if(Input.GetButtonDown("Hint"))
        {
            HintStat = !HintStat;
            Hint.SetActive(!HintStat);
            OHint.SetActive(HintStat);
        }
    }
    public void PlayPressed()
    {
        SceneManager.LoadScene("main menu");
    }
    public void ExitPressed()
    {
        Application.Quit();
    }
}
