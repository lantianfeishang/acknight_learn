using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour,IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] int AgentNumber;//该干员序列

    public void OnDrag(PointerEventData eventData)
    {
        AgentPrefabs.instance.AgentDrag();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        AgentPrefabs.instance.AgentDrayEnd();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AgentPrefabs.instance.AgentClick(AgentNumber);
    }
}
