using UnityEngine;
using UnityEngine.UIElements;

public class lever_script : MonoBehaviour
{
    [SerializeField] GameObject[] Objects;
    float delay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (delay < Time.time)
        {
            pull_switch();
            gameObject.GetComponent<Transform>().rotation = new Quaternion(0, 0, gameObject.GetComponent<Transform>().rotation.z * -1, 0.9762961f);
            delay = Time.time + 0.5f;
        }
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
