using UnityEngine;
using UnityEngine.UIElements;

public class lever_script : MonoBehaviour
{
    [SerializeField] GameObject[] Objects;
    [SerializeField] float a;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        pull_switch();
        gameObject.GetComponent<Transform>().rotation = new Quaternion(0, 0, gameObject.GetComponent<Transform>().rotation.z * -1, 0.9762961f);
    }
    private void pull_switch()
    {
        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] != null)
            {
                Objects[i].SetActive(!Objects[i].activeSelf);
            }
        }
    }
}
