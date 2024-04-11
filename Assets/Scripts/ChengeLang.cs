using UnityEngine;
using Lean.Localization;
using Eiko.YaSDK;

public class ChengeLang : MonoBehaviour
{
    private void Awake()
    {
        GameLoader.Instance.OnLoad.AddListener(OnLoad);
    }

    public void OnLoad()
    {
        string currentLang = SaveSystem.GetLang();

        if (currentLang == "")
        {
            YandexSDK.instance.Lang.ToUpper();
        }

        LeanLocalization.SetCurrentLanguageAll(currentLang);
        Debug.Log(currentLang);
    }

    public void SetLangauge(string lang)
    {
        lang = lang.ToUpper();
        LeanLocalization.SetCurrentLanguageAll(lang);
        SaveSystem.SetLang(lang);
    }
}