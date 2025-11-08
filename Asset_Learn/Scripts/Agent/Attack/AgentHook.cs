using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentHook : MonoBehaviour
{
    private List<GameObject> enemies = new();
    private GameObject enemy = null;
    private Rigidbody enemyRb = null;
    [SerializeField] float attackTime;
    private float delAttack;
    [SerializeField] Slider attackTimeUI;
    [SerializeField] float usefulTime;
    private float delUseful;
    [SerializeField] float hurt;
    [SerializeField] float usefulSpeed;
    private void Awake()
    {
        delAttack = 0;
        delUseful = usefulTime;
        attackTimeUI.value = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemies.Contains(other.gameObject))
        {
            enemies.Add(other.gameObject);//½øÈë¹¥»÷·¶Î§¼ÇÂ¼
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (enemies.Contains(other.gameObject))
        {
            enemies.Remove(other.gameObject);//ÍË³ö¹¥»÷·¶Î§¼ÇÂ¼
        }
    }
    private void FixedUpdate()
    {
        //¹¥»÷×÷ÓÃ
        if(delUseful < usefulTime)
        {
            delUseful += Time.deltaTime;
            if (enemy != null && enemy.activeSelf)
            {
                enemyRb.velocity = -transform.forward * usefulSpeed;
            }
            else
            {
                delUseful = usefulTime;
                enemyRb = null;
                enemy = null;
            }
        }
        else if (enemy != null)
        {
            enemyRb.velocity = Vector3.zero;
            enemyRb = null;
            enemy = null;
        }
        //¹¥»÷¼ä¸ô
        if(delAttack < attackTime)
        {
            delAttack += Time.deltaTime;
            attackTimeUI.value = delAttack / attackTime;
            return;
        }
        else
        {
            attackTimeUI.value = 1;
        }
        if (enemies.Count > 0)
        {
            enemy = enemies[0];
            if (enemy == null)
            {
                enemies.Remove(enemy);
                return;
            }
            enemy.GetComponent<Enemy>().GetHurt(hurt);
            delAttack = delUseful = 0;
            attackTimeUI.value = 0;
            enemyRb = enemy.GetComponent<Rigidbody>();
        }
    }
}
