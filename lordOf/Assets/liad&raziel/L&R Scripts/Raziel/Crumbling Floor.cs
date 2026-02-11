using UnityEngine;
using System.Collections;

public class CrumblingFloor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(WaitBeforeBreaking());
        }
    }

    private IEnumerator WaitBeforeBreaking()
    {
        yield return new WaitForSeconds(0.1f);
        Break();

    }

    private void Break()
    {
        Destroy(gameObject);
    }
}
