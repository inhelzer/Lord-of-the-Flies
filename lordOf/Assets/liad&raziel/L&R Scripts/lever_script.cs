using UnityEngine;

public class lever_script : MonoBehaviour
{
    [SerializeField] bool Pull;
    [SerializeField] object[] On_Objects;
    [SerializeField] object[] Off_Scripts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Pull = !Pull;
    }
    private void update_switch()
    {
        if (Pull)
        {

        }
        else
        {

        }
    }
}
