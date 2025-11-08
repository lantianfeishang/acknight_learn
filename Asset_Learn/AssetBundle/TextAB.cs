using UnityEngine;
using UnityEngine.SceneManagement;

public class TextAB : MonoBehaviour
{
    public void TextButton()
    {
        string path = "Assets/AssetBundle/text.ab";


        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        GameObject obj = null;
        obj = assetBundle.LoadAsset<GameObject>("Cube_Trap");
        if(obj == null)
        {
            return;
        }
        Instantiate(obj);
        Destroy(gameObject);
    }
}
