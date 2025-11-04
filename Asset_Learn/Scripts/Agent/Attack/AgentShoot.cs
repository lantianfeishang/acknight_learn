using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AgentShoot : MonoBehaviour
{
    private List<GameObject> enemies = new();
    [SerializeField] float timer;
    private float delTimer;
    [SerializeField] Slider timeSlider;//显示输出间隔

    [SerializeField] float hurt;//伤害
    private void Awake()
    {
        delTimer = 0;
        timeSlider.value = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemies.Contains(other.gameObject))
        {
            enemies.Add(other.gameObject);//进入攻击范围记录
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (enemies.Contains(other.gameObject))
        {
            enemies.Remove(other.gameObject);//退出攻击范围记录
        }
    }
    private void FixedUpdate()
    {
        //攻击间隔
        if (timer > delTimer)
        {
            delTimer += Time.fixedDeltaTime;
            timeSlider.value = delTimer / timer;
            return;
        }
        else
        {
            timeSlider.value = 1;
        }        
        if (enemies.Count > 0)
        {
            GameObject gameObject = enemies[0];
            if (gameObject == null)
            {
                enemies.Remove(gameObject);//如果对象不存在则剔除
                return;
            }
            gameObject.GetComponent<Enemy>().GetHurt(hurt);
            delTimer = 0;
            timeSlider.value = 0;
        }
    }
}
