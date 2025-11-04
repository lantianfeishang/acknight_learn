using UnityEngine;

public class AgentStore : MonoBehaviour
{
    private Cube cube;//脚下的方块
    private bool hasFix;//是否修改地图
    private void OnEnable()
    {
        hasFix = false;
    }
    private void Update()
    {
        if(!hasFix && GetComponent<AgentScope>().cube != null)
        {
            cube = GetComponent<AgentScope>().cube;
            if (cube != null)
            {
                GameManager.instance.map[cube.y, cube.x] = (int)GameManager.mapState.store;
                hasFix = true;
            }
        }
    }
    private void OnDisable()
    {
        if(cube != null)
        {
            GameManager.instance.map[cube.y, cube.x] = cube.cubeType;
        }
        cube = null;
        hasFix = false;
    }
}
