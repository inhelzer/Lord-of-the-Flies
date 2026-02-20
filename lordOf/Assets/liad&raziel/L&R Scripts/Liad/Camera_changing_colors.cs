using UnityEngine;

public class Camera_changing_colors : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Camera>().backgroundColor = Color.HSVToRGB(a, 1f, 1f); //  שינוי צבע לצבע צפציפי בין 0 ל-1
        GetComponent<Camera>().backgroundColor = Color.LerpUnclamped(Color.orange, Color.HSVToRGB(0, 1f, 1f), (gameObject.GetComponent<Transform>().position.x / 10) - 1f); // שינוי צבע אחד לאחר מימין לשמאל
    }
}
