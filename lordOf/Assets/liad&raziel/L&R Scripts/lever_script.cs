using UnityEngine;

public class lever_script : MonoBehaviour
{
    [SerializeField] bool Pull;
    [SerializeField] GameObject[] On_Objects;
    [SerializeField] GameObject[] Off_Objects;
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
        update_switch();
        
    }
    private void update_switch()
    {
        if (Pull)
        {
            for (int i = 0; i < On_Objects.Length; i++)
            {
                On_Objects[i].GetComponent<GameObject>().SetActive(false);
            }
            for (int i = 0; i < On_Objects.Length; i++)
            {
                Off_Objects[i].GetComponent<GameObject>().SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < On_Objects.Length; i++)
            {
                Off_Objects[i].GetComponent<GameObject>().SetActive(false);
            }
            for (int i = 0; i < On_Objects.Length; i++)
            {
                On_Objects[i].GetComponent<GameObject>().SetActive(true);
            }
        }
    }
}
