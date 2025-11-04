using UnityEngine;

public class Towards : MonoBehaviour
{
    public void OnMouseDrag()
    {
        AgentPrefabs.instance.TowardsDrag();
    }
    public void OnMouseUp()
    {
        AgentPrefabs.instance.TowardsDragEnd();
    }
}
