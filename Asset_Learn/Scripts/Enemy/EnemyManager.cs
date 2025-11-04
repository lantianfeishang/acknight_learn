using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<Cube> redCube = new();//要出场的格子
    [SerializeField] List<Cube> cubes = new();//要经过的格子
    [SerializeField] List<GameObject> enemies = new();//要出场的敌人
    [SerializeField] List<float> enemiesTime = new();//对应敌人的出场时间
    public List<GameObject> has_enemies = new();//已经上场的敌人
    private float nowTimer;//计时器
    private int path;//判定第几个敌人
    private void Start()
    {
        nowTimer = 0;
        path = 0;
        GameManager.instance.GetWholeEnemy(enemies.Count);
    }
    private void FixedUpdate()
    { 
        nowTimer += Time.fixedDeltaTime;
        if (enemiesTime.Count >= path+1 && enemiesTime[path] <= nowTimer)
        {
            GameObject obj = Instantiate(enemies[path]);
            EnemyMove move = obj.GetComponent<EnemyMove>();
            move.Init(cubes, redCube[0]);
            obj.transform.SetParent(transform);

            has_enemies.Add(obj);
            path++;
        }
    }
}
