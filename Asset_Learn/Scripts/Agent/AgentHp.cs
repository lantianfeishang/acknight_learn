using UnityEngine;

public class AgentHp : MonoBehaviour
{
    [SerializeField] float Hp;
    private float HpNow;
    private void Awake()
    {
        HpNow = Hp;
    }
    public void GetHurt(float hert)
    {
        HpNow -= hert;
        IfDead();
    }
    private void IfDead()
    {
        if(HpNow > 0)
        {
            return;
        }
        if(gameObject.GetComponent<AgentScope>() == null)
        {
            return;
        }
        AgentScope scope = gameObject.GetComponent<AgentScope>();
        scope.cube.agent = null;
        scope.cube = null;
        gameObject.SetActive(false);
        AgentPrefabs.instance.hasDownAgentPerfabs.Remove(gameObject);
    }
}
