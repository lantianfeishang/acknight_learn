using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_button : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("learnGame");
    }
}
