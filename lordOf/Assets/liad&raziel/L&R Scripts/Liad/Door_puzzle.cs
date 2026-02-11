using Unity.VisualScripting;
using UnityEngine;
public class Door_puzzle : MonoBehaviour
{
    [SerializeField] GameObject[] Objects;
    bool solve = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            gameObject.SetActive(!solve);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        solve = true;
        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] != null)
            {
                if (!Objects[i].activeSelf)
                {
                    solve = false;
                }
            }
        }
    }
}
