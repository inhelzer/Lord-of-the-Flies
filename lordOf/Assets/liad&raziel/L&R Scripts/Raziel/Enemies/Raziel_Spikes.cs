using UnityEngine;
using System.Collections;

public class Raziel_Spikes : MonoBehaviour
{
    [SerializeField] Vector3 HazardPosition;
    [SerializeField] bool delta;
    Raziel_BasePlayer player;


    private void Start()
    {
        player = FindAnyObjectByType<Raziel_BasePlayer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HaltBeforeSendingPlayer());
        }
    }

    private IEnumerator HaltBeforeSendingPlayer()
    {
        player.GiveControls(false);
        player.CheckPlayerConstraints();
        yield return new WaitForSeconds(0.2f);

        if (delta)
            player.SendToPosition(transform.position + HazardPosition);
        else
            player.SendToPosition(HazardPosition);
        player.GiveControls(true);
        player.CheckPlayerConstraints();
    }
}
