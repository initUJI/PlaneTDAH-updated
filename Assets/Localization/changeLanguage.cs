using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class changeLanguage : MonoBehaviour
{
    // Start is called before the first frame update
    int _idLanguage = 0;
    void Start()
    {
        f_ChangeLanguage(0);
    }

    public void f_ChangeLanguage(int id)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
    }

    public void f_ChangeLanguageClick()
    {
        _idLanguage = _idLanguage == 0 ? 1 : 0;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_idLanguage];
    }
}
