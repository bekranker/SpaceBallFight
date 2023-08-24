using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSupport : MonoBehaviour
{
    [SerializeField] private ButtonClickManager _LanguageSupportButtonRight, _LanguageSupportButtonLeft;
    [SerializeField] private List<string> _Languages;
    [SerializeField] private List<Sprite> _LanguagesSprites;
    [SerializeField] private Image _FlagImage;
    private int _languageIndex;
    [SerializeField] private ChangeTexts _ChangeTexts;

    void Start()
    {
        Cursor.visible = true;
        _FlagImage.sprite = _LanguagesSprites[IndexOfLanguage()];
        _languageIndex = IndexOfLanguage();
        _LanguageSupportButtonRight.DoSomething += ()=> SetLanguageRight();
        _LanguageSupportButtonLeft.DoSomething += ()=> SetLanguageLeft();
    }
    private void SetLanguageRight()
    {
        _languageIndex = (_languageIndex + 1 > 4) ? 0 : _languageIndex + 1;
        PlayerPrefs.SetString($"Language", $"Language{_Languages[_languageIndex]}");
        SetFlag();
        _ChangeTexts.ChangeTextsToLanguage();
    }
    private void SetLanguageLeft()
    {
        _languageIndex = (_languageIndex - 1 < 0) ? 4 : _languageIndex - 1;
        PlayerPrefs.SetString($"Language", $"Language{_Languages[_languageIndex]}");
        SetFlag();
        _ChangeTexts.ChangeTextsToLanguage();
    }
    private void SetFlag()
    {
        _FlagImage.sprite = _LanguagesSprites[_languageIndex];
    }
    private int IndexOfLanguage()
    {
        switch (PlayerPrefs.GetString("Language", "Languageen"))
        {
            //eng
            case "Languageen":
                return 0;
            //tr
            case "Languagetr":
                return 1;
            //ru
            case "Languageru":
                return 2;
            //germany
            case "Languagegermany":
                return 3;
            //french
            case "Languagefrench":
                return 4;
            default:
                break;
        }
        return 0;
    }
}
