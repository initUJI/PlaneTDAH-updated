using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sessionButtonsOrganizer : MonoBehaviour
{
    public Button shortSession, mediumSession, longSession;

    // Start is called before the first frame update
    void OnEnable()
    {
        
        shortSession.onClick.AddListener(() => { SessionManager.instance.setSessionMode("Corta"); });
        mediumSession.onClick.AddListener(() => { SessionManager.instance.setSessionMode("Media"); });
        longSession.onClick.AddListener(() => { SessionManager.instance.setSessionMode("Larga"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
