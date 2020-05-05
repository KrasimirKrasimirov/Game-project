using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static GameObject pauseMenu;

    [SerializeField]
    GameObject _journalCollectionText, _journalMenu, _menuObject, journalEntry1, journalEntry2, journalEntry3, journalEntry1Button, journalEntry2Button, journalEntry3Button;

    public static bool hasJournalEntry1 = false, hasJournalEntry2 = false, hasJournalEntry3 = false;

    bool _isJournalOpen = false;
    bool isJournalActive = false;

    public static bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
        pauseMenu = _menuObject;
        pauseMenu.SetActive(false);

        journalEntry1Button.SetActive(false);
        journalEntry2Button.SetActive(false);
        journalEntry3Button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Open Pause Menu
        if (Input.GetButtonDown("Escape"))
        {
            if (isPaused != true)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
               // Cursor.lockState = CursorLockMode.None;
                isPaused = true;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
               // Cursor.lockState = CursorLockMode.Locked;
                isPaused = false;
            }

            if(hasJournalEntry1 == true)
            {
                journalEntry1Button.SetActive(true);
            }
            if (hasJournalEntry2 == true)
            {
                journalEntry2Button.SetActive(true);
            }
            if (hasJournalEntry3 == true)
            {
                journalEntry3Button.SetActive(true);
            }

        }
        //Open journal
        //if (Input.GetButtonDown("Journal"))
        //{
        //    if (_isJournalOpen == false && isPaused != true)
        //    {
        //        _isJournalOpen = true;
        //        _journalCollectionText.SetActive(false);
        //        pauseMenu.SetActive(true);
        //        _journalMenu.SetActive(true);
        //        isPaused = true;
        //        Time.timeScale = 0;
        //        Cursor.lockState = CursorLockMode.None;
        //    }
        //    else if (_isJournalOpen == true)
        //    {
        //        _isJournalOpen = false;
        //        pauseMenu.SetActive(false);
        //        _journalMenu.SetActive(false);
        //        isPaused = false;
        //        Time.timeScale = 1;
        //        Cursor.lockState = CursorLockMode.Locked;
        //    }
        //}
    }

    public void JournalMenu()
    {
        if(_isJournalOpen == true)
        {
            _journalMenu.SetActive(false);
            _isJournalOpen = false;
        }
        else if (_isJournalOpen == false)
        {
            _journalMenu.SetActive(true);
            _isJournalOpen = true;
        }

    }

    public void JournalEntry1()
    {
        if(hasJournalEntry1 == true)
        {
            //journalEntry1Button.SetActive(true);
            journalEntry1.SetActive(true);
        }
    }
    public void JournalEntry2()
    {
        if (hasJournalEntry2 == true)
        {
            //journalEntry2Button.SetActive(true);
            journalEntry2.SetActive(true);
        }
    }
    public void JournalEntry3()
    {
        if (hasJournalEntry3 == true)
        {
            journalEntry3.SetActive(true);
        }
    }

    public void BackToJournal()
    {
        journalEntry1.SetActive(false);
        journalEntry2.SetActive(false);
        journalEntry3.SetActive(false);
    }

    public void UpdateJournalMenu()
    {
        StartCoroutine(DisplayJournalCollectionText());

        if (hasJournalEntry1 != true)
        {
            hasJournalEntry1 = true;
        }
        else if (hasJournalEntry2 != true)
        {
            hasJournalEntry2 = true;
        }
        else if (hasJournalEntry3 != true)
        {
            hasJournalEntry3 = true;
        }
    }

    IEnumerator DisplayJournalCollectionText()
    {
        _journalCollectionText.SetActive(true);
        yield return new WaitForSeconds(4);
        _journalCollectionText.SetActive(false);
    }

}
