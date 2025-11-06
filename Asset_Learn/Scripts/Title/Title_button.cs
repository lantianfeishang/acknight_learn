using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_button : MonoBehaviour
{
    //สตภýปฏ
    public static Title_button instance;
    private void Awake()
    {
        instance = this;
    }
    private List<int> agentNumber = new();
    [SerializeField] GameObject UI_1;
    [SerializeField] GameObject UI_2;
    [SerializeField] GameObject startButton;
    public void ClickUI_1()
    {
        UI_1.SetActive(false);
        UI_2.SetActive(true);
    }
    public void addAgent(int number)
    {
        if (agentNumber.Contains(number))
        {
            return;
        }
        agentNumber.Add(number);
        startButton.SetActive(true);
    }
    public void removeAgent(int number)
    {
        if (!agentNumber.Contains(number))
        {
            return;
        }
        agentNumber.Remove(number);
        if (agentNumber.Count < 1)
        {
            startButton.SetActive(false);
        }
    }
    private string saveAgent()
    {
        string agents = "";
        foreach(int i in agentNumber)
        {
            agents += i + " ";
        }
        return agents;
    }
    public void NextScene()
    {
        PlayerPrefs.SetString("agents", saveAgent());
        SceneManager.LoadScene("learnGame");
    }
}
