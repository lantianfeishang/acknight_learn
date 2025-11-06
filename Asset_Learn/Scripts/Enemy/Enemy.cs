using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float hpMax;//最大血量
    public float hp;//当前血量
    [SerializeField] CanvasGroup hpSliderCanva;//血条UI父级，用以隐藏UI
    [SerializeField] Slider hpSlider;//血条UI

    [SerializeField] float timer;
    private float delTimer;
    [SerializeField] float hurt;//伤害
    public AgentHp agentHp;
    public bool isStop;

    private void Awake()
    {
        hp = hpMax;
        hpSliderCanva.alpha = 0;
        hpSlider.value = hp / hpMax;

        isStop = false;
    }
    private void FixedUpdate()
    {
        if (!isStop)
        {
            delTimer = 0;
            return;
        }
        if (delTimer < timer)
        {
            delTimer += Time.deltaTime;
            return;
        }
        delTimer = 0;
        agentHp.GetHurt(hurt);
    }

    public void GetHurt(float hurt)
    {
        hpSliderCanva.alpha = 1;
        hp -= hurt;
        hpSlider.value = hp / hpMax;
        IfDead();
    }
    private void IfDead()
    {
        if (hp <= 0)
        {
            GameManager.instance.EnemyDead();
            Destroy(gameObject);
        }
    }
}
