using UnityEngine;

public class AgentScope : MonoBehaviour
{
    [SerializeField] GameObject agentScopeHighLight;//干员攻击范围
    [SerializeField] GameObject agentScope;//干员攻击碰撞箱
    public int agentType;//能放置的地块 1地面2高台
    public Cube cube = null;//干员占据的方块
    public void OpenScope()
    {
        agentScopeHighLight.SetActive(true);
    }
    public void CloseScope()
    {
        agentScopeHighLight.SetActive(false);
    }
    public void CloseFunction()
    {
        agentScope.SetActive(false);
    }
    public void OpenFunction()
    {
        agentScope.SetActive(true);
    }
}
