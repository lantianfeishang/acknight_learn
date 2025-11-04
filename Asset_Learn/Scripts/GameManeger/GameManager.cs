using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //实例化
    public static GameManager instance;
    [SerializeField] int homeHp;
    public int homeHpNow;

    public int wholeEnemy;
    public int nowEnemyDead;

    public TextMeshProUGUI homeHpText;
    public TextMeshProUGUI overText;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI enemyText;
    private void Awake()
    {
        instance = this;
        homeHpNow = homeHp;
        homeHpText.text = homeHpNow.ToString() + "/" + homeHp.ToString();
    }
    public enum mapState
    {
        nothing = 0,
        ground = 1,
        platform = 2,
        //redDoor = 3,
        //blueDoor = 4
        store = 5,
    }
    public int[,] map = new int[6, 6]
    {
        { 2,2,2,2,2,2 },
        { 1,1,1,1,1,2 },
        { 2,1,2,2,1,2 },
        { 2,1,1,1,1,2 },
        { 2,1,2,2,2,0 },
        { 0,1,0,0,0,0 },
     };
    public void HomeGetHart()
    {
        homeHpNow--;
        homeHpText.text = homeHpNow.ToString() + "/" + homeHp.ToString();
        //基地血量为0
        if (homeHpNow == 0)
        {
            overText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if((nowEnemyDead + homeHp - homeHpNow) == wholeEnemy){
            successText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void GetWholeEnemy(int number)
    {
        wholeEnemy = number;
        nowEnemyDead = 0;
        enemyText.text = 0 + "/" + wholeEnemy.ToString();
    }
    public void EnemyDead()
    {
        nowEnemyDead++;
        enemyText.text = nowEnemyDead.ToString() + "/" + wholeEnemy.ToString();
        //敌人全灭 或 敌人死的 加 进基地的数量等于总量
        if (nowEnemyDead == wholeEnemy && (nowEnemyDead + homeHp - homeHpNow) == wholeEnemy)
        {
            successText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
