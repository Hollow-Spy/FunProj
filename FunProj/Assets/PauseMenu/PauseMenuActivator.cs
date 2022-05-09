using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuActivator : MonoBehaviour
{

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject OnSound, OffSound;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
           
            if(PauseMenu.activeSelf)
            {
                Instantiate(OnSound, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(OffSound, transform.position, Quaternion.identity);

            }
        }
    }
}
