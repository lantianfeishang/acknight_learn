using UnityEngine;
using System.Collections.Generic;

public class AgentPrefabs : MonoBehaviour
{
    //实例化
    public static AgentPrefabs instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] Transform agentInitPosition;//干员初始位置
    [SerializeField] GameObject[] agentPerfabs;//干员模型
    public List<GameObject> hasDownAgentPerfabs = new();//已经放下的干员
    private GameObject thisAgentPerfabs = null;//鼠标上的干员
    private Cube thisCube = null;//鼠标指向的方块
    private bool hasAgentClick = false;//是否点击干员
    private AgentScope scope;//干员攻击范围，并判断是否被拖动过

    public GameObject highLight;//上下左右标
    private void Start()
    {
        HighLightOff();
    }

    //跟随鼠标
    private bool hasAgentDown = false;//干员是否占格子
    //记录并唤醒干员
    public void AgentClick(int agentNumber)
    {
        if (!hasAgentClick && hasDownAgentPerfabs.Contains(agentPerfabs[agentNumber]))
        {
            return;
        }
        thisAgentPerfabs = agentPerfabs[agentNumber];
        thisAgentPerfabs.transform.position = agentInitPosition.position;
    }
    
    public void AgentDrag()
    {
        //没选择关于 或 已经放下
        if (thisAgentPerfabs == null)
        {
            return;
        }

        if(!hasAgentClick && hasAgentDown && hasDownAgentPerfabs.Contains(thisAgentPerfabs))
        {
            return;
        }

        if(scope == null)
        {
            thisAgentPerfabs.SetActive(true);
            scope = thisAgentPerfabs.GetComponent<AgentScope>();
            scope.OpenScope();
            scope.CloseFunction();
            Time.timeScale = 0.1f;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线
        if (Physics.Raycast(ray, out RaycastHit hit, 100, 1 << 0 | 1 << 7))
        {
            Cube cube = hit.collider.gameObject.GetComponent<Cube>();
            if (cube != null && cube.agent == null && cube.cubeType == thisAgentPerfabs.GetComponent<AgentScope>().agentType)
            {
                Vector3 position = cube.transform.position;
                position.y = hit.point.y;
                thisAgentPerfabs.transform.position = position;
                return;
            }
            thisAgentPerfabs.transform.position = hit.point;
        }
    }
    //松开鼠标左键
    public void AgentDrayEnd()
    {
        if (thisAgentPerfabs == null)
        {
            return;
        }

        if(!hasAgentClick && hasAgentDown && hasDownAgentPerfabs.Contains(thisAgentPerfabs))
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线
        if (Physics.Raycast(ray, out RaycastHit hit, 100, 1 << 0 | 1 << 7))
        {
            Cube cube = hit.collider.gameObject.GetComponentInParent<Cube>();
            if (cube != null && cube.agent == null && cube.cubeType == thisAgentPerfabs.GetComponent<AgentScope>().agentType)
            {
                HighLightOn(thisAgentPerfabs.transform.position);
                hasAgentDown = true;
                thisCube = cube;
                scope = null;
                return;
            }
        }
        scope.CloseScope();
        scope.OpenFunction();
        scope = null;
        thisAgentPerfabs.SetActive(false);
        thisAgentPerfabs = null;
        Time.timeScale = 1;
    }
    //设置上下左右标
    private void HighLightOn(Vector3 vector3)
    {
        vector3.y += 0.2f;
        highLight.transform.position = vector3;
        highLight.SetActive(true);
    }
    private void HighLightOff()
    {
        highLight.SetActive(false);
    }
    //干员朝向
    public void TowardsDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线
        RaycastHit hit;//射线着点

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPostion = hit.point;
            hitPostion.y = 0;
            Vector3 agentPositon = thisAgentPerfabs.transform.position;
            agentPositon.y = 0;
            if (Vector3.Distance(hitPostion, agentPositon) > 1)
            {
                Vector3 forword = hitPostion - agentPositon;
                if (Mathf.Abs(forword.x) > Mathf.Abs(forword.z))
                {
                    forword.z = 0;
                }
                else
                {
                    forword.x = 0;
                }

                thisAgentPerfabs.transform.forward = forword;
                return;
            }
        }
    }
    public void TowardsDragEnd()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Finish"))
            {
                HighLightOff();

                thisAgentPerfabs.SetActive(false);
                thisAgentPerfabs = null;

                Time.timeScale = 1;
                hasAgentDown = false;
                return;
            }

            Vector3 hitPostion = hit.point;
            hitPostion.y = 0;
            Vector3 agentPositon = thisAgentPerfabs.transform.position;
            agentPositon.y = 0;
            //离开1个单位，认为选择了
            if (Vector3.Distance(hitPostion, agentPositon) > 1)
            {
                hasDownAgentPerfabs.Add(thisAgentPerfabs);
                AgentScope scope = thisAgentPerfabs.GetComponent<AgentScope>();
                scope.CloseScope();
                scope.OpenFunction();
                thisCube.agent = thisAgentPerfabs;
                thisAgentPerfabs.GetComponent<AgentScope>().cube = thisCube;
                thisAgentPerfabs = null;
                thisCube = null;

                HighLightOff();
                hasAgentDown = false;
                Time.timeScale = 1;
            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hasAgentDown)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线

            if (Physics.Raycast(ray, out RaycastHit hit, 100, 1 << 0 | 1 << 5 | 1 << 9))
            {
                if (hasAgentClick)
                {
                    if (hit.collider.CompareTag("Finish"))
                    {
                        thisAgentPerfabs.GetComponent<AgentScope>().cube.agent = null;
                        thisAgentPerfabs.GetComponent<AgentScope>().cube = null;
                        thisAgentPerfabs.SetActive(false);
                        hasDownAgentPerfabs.Remove(thisAgentPerfabs);
                    }
                    thisAgentPerfabs = null;
                    HighLightOff();
                    hasAgentClick = false;
                    Time.timeScale = 1;
                }
                else if (hit.collider.CompareTag("Agent"))
                {
                    Time.timeScale = 0.1f;

                    thisAgentPerfabs = hit.collider.gameObject;
                    hasAgentClick = true;
                    HighLightOn(hit.collider.transform.position);
                }
            }
        }
    }
}
