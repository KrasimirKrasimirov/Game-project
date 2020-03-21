using System.Collections;
using UnityEngine;

public class JournalPickups : MonoBehaviour
{
    [SerializeField]
    PauseMenu pauseMenu;

    [SerializeField]
    GameObject _journalCollectionText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pauseMenu.UpdateJournalMenu();
            Destroy(gameObject);
        }
    }
}
