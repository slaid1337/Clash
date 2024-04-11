using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestPanelItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _questText;
    [SerializeField] private TMP_Text _lvlText;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _buttonPanel;
    public string OriginalText;

    private void Start()
    {
        LeanLocalization.OnLocalizationChanged += delegate
        {
            _questText.text = LeanLocalization.GetTranslationText(OriginalText);
        };
    }

    public void UpdateText(string questText, string lvlText)
    {
        _questText.text = questText;
        _lvlText.text = lvlText;
    }

    public void SetToCollect(UnityAction action)
    {
        _buttonPanel.SetActive(true);
        _button.onClick.AddListener(action);
    }
}
