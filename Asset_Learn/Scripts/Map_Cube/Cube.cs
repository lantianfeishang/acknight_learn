using UnityEngine;

public class Cube : MonoBehaviour
{
    public int x, y;
    public Cube northCube, eastCube, southCube, westCube;
    public int cubeType;
    public GameObject agent = null;
    enum Type
    {
        nothing = 0,
        ground = 1,
        platform = 2,
        stone = 5,
    }
}
