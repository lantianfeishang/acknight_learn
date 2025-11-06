using System.Collections.Generic;
using UnityEngine;

public class GetData : MonoBehaviour
{
    [SerializeField] List<ButtonClick> agentButtons = new();
    private List<int> agentsNumber = new();
    private void Awake()
    {
        string agents = PlayerPrefs.GetString("agents");
        foreach (string i in agents.Split(" "))
        {
            if(i ==" " | i == "")
            {
                continue;
            }
            int agent = int.Parse(i);
            if (agentsNumber.Contains(agent))
            {
                continue;
            }
            agentsNumber.Add(agent);
        }
        foreach(ButtonClick i in agentButtons)
        {
            if (agentsNumber.Contains(i.AgentNumber))
            {
                i.gameObject.SetActive(true);
                continue;
            }
            i.gameObject.SetActive(false);
        }
    }
}
