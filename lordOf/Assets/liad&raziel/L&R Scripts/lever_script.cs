using UnityEngine;

public class lever_script : MonoBehaviour
{
    [SerializeField] GameObject[] Objects;
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
        pull_switch();
    }
    private void pull_switch()
    {
        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] != null)
            {
                Objects[i].GetComponent<GameObject>().SetActive(!Objects[i].GetComponent<GameObject>().activeSelf);
            }
        }
    }
}
