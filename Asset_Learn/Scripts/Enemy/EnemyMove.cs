using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private List<Cube> blueDoor = new();//将入的蓝门
    private Cube redDoor;//出的红门。实际上，记录的是门下方的方块

    private int x = -1, y = -1;//当前的坐标
    private Cube thisCube = null;//脚下方块
    private Cube nextCube = null;//下一个方块
    private List<Cube> cubes = new();//脚下方块集。因不一定只踩着一个
    private int[,] map;//全局地图
    private int[,] movePath;//路径地图
    private bool hasGone;//已经到记录点了

    [SerializeField] float moveSpeed;//移动速度
    private float moveSpeedNow;//当前速度，为被阻挡准备

    private Enemy thisEnemy;
    public void Init(List<Cube> blue, Cube red)
    {
        blueDoor.AddRange(blue);
        redDoor = red;
        moveSpeedNow = moveSpeed;

        //初始位置
        Vector3 vector3 = redDoor.transform.position;
        vector3.y = transform.position.y;
        transform.position = vector3;

        map = (int[,])GameManager.instance.map.Clone();
        FixMovePath(blueDoor[0],redDoor);

        thisEnemy = gameObject.GetComponent<Enemy>();
    }
    //获取脚下的方块，以明确当前坐标
    private void OnTriggerEnter(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube!=null && !cubes.Contains(cube))
        {
            cubes.Add(cube);
            if(cube == blueDoor[0])
            {
                blueDoor.Remove(blueDoor[0]);
                hasGone = true;
            }
        }
    }
    //消去方块记录
    private void OnTriggerExit(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube!= null && cubes.Contains(cube))
        {
            cubes.Remove(cube);
            //已经到记录点了
            if(hasGone)
            {
                hasGone = false;
                if (blueDoor.Count == 0)
                {
                    GameManager.instance.HomeGetHart();
                    Destroy(gameObject);
                    return;
                }
                FixMovePath(blueDoor[0], thisCube);
            }
            return;
        }
    }
    private void FixedUpdate()
    {
        //如果地图改变，修改路径
        if(!IsArraySame(map,GameManager.instance.map))
        {
            map = (int[,])GameManager.instance.map.Clone();
            FixMovePath(blueDoor[0], thisCube);
        }
        //离开方块
        if (cubes.Count > 0 && thisCube != cubes[0])
        {
            thisCube = cubes[0];
            x = cubes[0].x;
            y = cubes[0].y;
        }
        //存在路径地图，即可移动
        if (x!= -1 && y!=-1)
        {
            //抵达了，换目的地
            if(nextCube == null || nextCube == thisCube)
            {
                int xMax = movePath.GetLength(0) - 1;//可取最大值
                int yMax = movePath.GetLength(1) - 1;

                int nowPath = movePath[y, x];

                if (x > 0 && movePath[y, x - 1] == nowPath + 1)
                {
                    nextCube = thisCube.GetComponent<Cube>().westCube;
                }
                else if(x < xMax && movePath[y, x + 1] == nowPath + 1)
                {
                    nextCube = thisCube.GetComponent<Cube>().eastCube;
                }
                else if (y > 0 && movePath[y - 1, x] == nowPath + 1)
                {
                    nextCube = thisCube.GetComponent<Cube>().northCube;
                }
                else if(y < yMax && movePath[y + 1, x] == nowPath + 1)
                {
                    nextCube = thisCube.GetComponent<Cube>().southCube;
                }
            }
            //移动。若需要于某点等待，改这里
            if (nextCube != null)
            {
                Vector3 nextV3 = nextCube.transform.position;
                nextV3.y = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, nextV3, moveSpeedNow * Time.fixedDeltaTime);
            }
        }
    }
    private bool IsArraySame(int[,] array1, int[,] array2)
    {
        for (int j = 0; j < array1.GetLength(1); j++)
        {
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                if (array1[j, i] != array2[j, i])
                {
                    return false;
                }
            }
        }
        return true;
    }
    //修正路径
    struct Point
    {
        int x,y,way;

        public Point(int x,int y,int way)
        {
            this.x = x;
            this.y = y;
            this.way = way;
        }
        public int get_x()
        {
            return x;
        }
        public int get_y()
        {
            return y;
        }
        public int get_way()
        {
            return way;
        }
    }
    private void FixMovePath(Cube blue,Cube red)
    {
        int path_x, path_y, path_way,xMax,yMax;
        int[,] tempMap = new int[map.GetLength(0),map.GetLength(1)];
        movePath = new int[map.GetLength(0), map.GetLength(1)];

        Queue<Point> r = new();
        if (thisCube != null)
        {
            r.Enqueue(new Point(thisCube.x, thisCube.y,0));
            tempMap[thisCube.y, thisCube.x] = 1;
        }
        else
        {
            r.Enqueue(new Point(red.x, red.y,0));
            tempMap[red.y, red.x] = 1;
        }
        xMax = tempMap.GetLength(0) - 1;//可取最大值
        yMax = tempMap.GetLength(1) - 1;
        int ground = (int)GameManager.mapState.ground;
        int path;

        //广度遍历建表
        while (r.Count > 0)
        {
            Point q = r.Dequeue();
            path_x = q.get_x();
            path_y = q.get_y();
            path_way = q.get_way();
            path = tempMap[path_y, path_x] + 1;

            //可否取西 可否取；第二个条件：被遍历过，但路程远
            if (path_way!=2 && path_x > 0 && map[path_y, path_x - 1] == ground)
            {
                //未遍历过
                if (tempMap[path_y, path_x - 1] == 0)
                {
                    tempMap[path_y, path_x - 1] = path;
                    r.Enqueue(new Point(path_x - 1, path_y,1));
                }
                //遍历过，但路程近
                else if (tempMap[path_y, path_x - 1] > path)
                {
                    tempMap[path_y, path_x - 1] = path;
                    r.Enqueue(new Point(path_x - 1, path_y,1));
                }
            }
            //可否取东
            if (path_way!=1 && path_x < xMax && map[path_y, path_x + 1] == ground)
            {
                if(tempMap[path_y, path_x + 1] == 0)
                {
                    tempMap[path_y, path_x + 1] = path;
                    r.Enqueue(new Point(path_x + 1, path_y,2));
                }
                else if(tempMap[path_y, path_x + 1] > path)
                {
                    tempMap[path_y, path_x + 1] = path;
                    r.Enqueue(new Point(path_x + 1, path_y,2));
                }
            }
            //可否取北
            if (path_way!=4 && path_y > 0 && map[path_y - 1, path_x] == ground)
            {
                if (tempMap[path_y - 1, path_x] == 0)
                {
                    tempMap[path_y - 1, path_x] = path;
                    r.Enqueue(new Point(path_x, path_y - 1,3));
                }
                else if (tempMap[path_y - 1, path_x] > path)
                {
                    tempMap[path_y - 1, path_x] = path;
                    r.Enqueue(new Point(path_x, path_y - 1,3));
                }
            }
            //可否取南
            if (path_way !=3 && path_y < yMax && map[path_y + 1, path_x] == ground)
            {
                if (tempMap[path_y + 1, path_x] == 0)
                {
                    tempMap[path_y + 1, path_x] = path;
                    r.Enqueue(new Point(path_x, path_y + 1,4));
                }
                else if (tempMap[path_y + 1, path_x] > path)
                {
                    tempMap[path_y + 1, path_x] = path;
                    r.Enqueue(new Point(path_x, path_y + 1,4));
                }
            }
        }

        //如果石头挡住所有道路
        if (tempMap[blue.y,blue.x] == 0)
        {
            //重置变量
            r = new();
            if (thisCube != null)
            {
                r.Enqueue(new Point(thisCube.x, thisCube.y,0));
                tempMap[thisCube.y, thisCube.x] = 1;
            }
            else
            {
                r.Enqueue(new Point(red.x, red.y,0));
                tempMap[red.y, red.x] = 1;
            }
            tempMap = new int[map.GetLength(0), map.GetLength(1)];

            int store = (int)GameManager.mapState.store;
            while (r.Count > 0)
            {
                Point q = r.Dequeue();
                path_x = q.get_x();
                path_y = q.get_y();
                path_way = q.get_way();
                path = tempMap[path_y, path_x] + 1;

                //可否取西
                if (path_way!= 2 && path_x > 0 && (map[path_y, path_x - 1] == ground | map[path_y, path_x - 1]==store))
                {
                    if (tempMap[path_y, path_x - 1] == 0)
                    {
                        tempMap[path_y, path_x - 1] = path;
                        r.Enqueue(new Point(path_x - 1, path_y,1));
                    }
                    else if (tempMap[path_y, path_x - 1] > path)
                    {
                        tempMap[path_y, path_x - 1] = path;
                        r.Enqueue(new Point(path_x - 1, path_y,1));
                    }
                }
                //可否取东
                if (path_way!= 1 && path_x < xMax && (map[path_y, path_x + 1] == ground | map[path_y, path_x + 1] == store))
                {
                    if (tempMap[path_y, path_x + 1] == 0)
                    {
                        tempMap[path_y, path_x + 1] = path;
                        r.Enqueue(new Point(path_x + 1, path_y,2));
                    }
                    else if (tempMap[path_y, path_x + 1] > path)
                    {
                        tempMap[path_y, path_x + 1] = path;
                        r.Enqueue(new Point(path_x + 1, path_y,2));
                    }
                }
                //可否取北
                if (path_way!= 4 && path_y > 0 && (map[path_y - 1, path_x] == ground | map[path_y - 1, path_x] == store))
                {
                    if (tempMap[path_y - 1, path_x] == 0)
                    {
                        tempMap[path_y - 1, path_x] = path;
                        r.Enqueue(new Point(path_x, path_y - 1,3));
                    }
                    else if (tempMap[path_y - 1, path_x] > path)
                    {
                        tempMap[path_y - 1, path_x] = path;
                        r.Enqueue(new Point(path_x, path_y - 1,3));
                    }
                }
                //可否取南
                if (path_way != 3 && path_y < yMax && (map[path_y + 1, path_x] == ground | map[path_y + 1, path_x] == store))
                {
                    if (tempMap[path_y + 1, path_x] == 0)
                    {
                        tempMap[path_y + 1, path_x] = path;
                        r.Enqueue(new Point(path_x, path_y + 1,4));
                    }
                    else if (tempMap[path_y + 1, path_x] > path)
                    {
                        tempMap[path_y + 1, path_x] = path;
                        r.Enqueue(new Point(path_x, path_y + 1,4));
                    }
                }
            }
        }

        //遍历回去
        path_x = blue.x;
        path_y = blue.y;
        path = tempMap[path_y, path_x];
        movePath[path_y, path_x] = path;
        while (path > 1)
        {
            path--;
            if (path_x > 0 && tempMap[path_y, path_x - 1] == path)
            {
                movePath[path_y, path_x - 1] = path;
                path_x -= 1;
            }
            else if(path_x < xMax && tempMap[path_y, path_x + 1] == path)
            {
                movePath[path_y, path_x + 1] = path;
                path_x += 1;
            }
            else if(path_y > 0 && tempMap[path_y - 1, path_x] == path)
            {
                movePath[path_y - 1, path_x] = path;
                path_y -= 1;
            }
            else
            {
                movePath[path_y + 1, path_x] = path;
                path_y += 1;
            }
        }

        nextCube = null;
    }
    //碰到石头停滞，否则前进
    private GameObject stopGameObject = null;//阻挡的干员
    private void OnTriggerStay(Collider other)
    {
        //路径产生偏差
        if (other.GetComponent<Cube>() != null)
        {
            Cube cube = other.GetComponent<Cube>();
            int nowPath = movePath[cube.y, cube.x];
            int nextPath = 0;
            if (nextCube != null)
            {
                nextPath = movePath[nextCube.y, nextCube.x];
            }
            //当前路径本不可达；当前路径不是下一个路径、下一个路径前一、前二格
            if (nowPath == 0 || (nextCube != null && (nowPath != nextPath & nowPath != nextPath - 1 & nowPath != nextPath - 2)))
            {
                FixMovePath(blueDoor[0], cube);
            }
            return;
        }
        //被阻挡
        AgentScope scope = other.GetComponent<AgentScope>();
        if(scope == null || scope.cube == null)
        {
            return;
        }
        if (stopGameObject == null && scope.agentType == 1)
        {
            stopGameObject = other.gameObject;
            moveSpeedNow = 0;
            thisEnemy.agentHp = stopGameObject.GetComponent<AgentHp>();
            thisEnemy.isStop = true;
        }
    }
    private void Update()
    {
        if (stopGameObject != null && !stopGameObject.activeSelf)
        {
            stopGameObject = null;
            moveSpeedNow = moveSpeed;
            thisEnemy.isStop = false;
        }
    }
}
