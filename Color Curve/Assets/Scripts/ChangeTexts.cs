using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ChangeTexts : MonoBehaviour
{
    public List<TMP_Text> _Texts = new List<TMP_Text>(); 
    public List<Texts> _Languages = new List<Texts>();

    private void Start()
    {
        ChangeTextsToLanguage();
    }

    public void ChangeTextsToLanguage(string key = null)
    {
        if (!PlayerPrefs.HasKey("Language")) return;
        Texts sentencesText = new Texts();


        switch (PlayerPrefs.GetString("Language"))
        {
            //eng
            case "Languageen":
                sentencesText = _Languages[0];
                break;
            //tr
            case "Languagetr":
                sentencesText = _Languages[1];
                break;
            //ru
            case "Languageru":
                sentencesText = _Languages[2];
                break;
            //germany
            case "Languagegermany":
                sentencesText = _Languages[3];
                break;
            //french
            case "Languagefrench":
                sentencesText = _Languages[4];
                break;
            default:
                break;
        }
        for (int i = 0; i < sentencesText.Sentences.Count; i++)
        {
            _Texts[i].text = sentencesText.Sentences[i];
        }
    }

}
[System.Serializable]
public class Texts : IEnumerable<string>
{
    public List<string> Sentences = new List<string>();

    public IEnumerator<string> GetEnumerator()
    {
        return Sentences.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}
