using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title_agentClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] int AgentNumber;//该干员序列
    private bool hasClicked;
    private Image image;
    private void Awake()
    {
        hasClicked = false;
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (hasClicked)
        {
            Title_button.instance.removeAgent(AgentNumber);
            image.color = Color.black;
            hasClicked = !hasClicked;
            return;
        }
        Title_button.instance.addAgent(AgentNumber);
        image.color = Color.blue;
        hasClicked = !hasClicked;
    }
}
