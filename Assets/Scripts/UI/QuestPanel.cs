using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class QuestPanel : Singletone<QuestPanel>
{
    [SerializeField] private RectTransform _questIcon;
    [SerializeField] private RectTransform _closeIcon;
    [SerializeField] private Button _panelButton;
    [SerializeField] private bool _isOpen;
    [SerializeField] RectTransform _questContainer;
    [SerializeField] private QuestObjects _questObjects;
    [SerializeField] private GameObject _prefab;
    private float _startPosX;
    [SerializeField] private int _questStage;
    [SerializeField] private GameObject _noQuestText;
    private List<QuestPanelItem> _questPanelItems;
    [SerializeField] private GameObject _notification;

    public QuestObjects QuestObjects
    {
        get
        {
            return _questObjects;
        }
    }

    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

    private void Awake()
    {
        base.Awake();

        GameLoader.Instance.OnLoad.AddListener(OnLoad);
    }

    public void OnLoad()
    {
        _questPanelItems = new List<QuestPanelItem>();

        _panelButton.onClick.AddListener(TogglePanel);

        _startPosX = RectTransform.anchoredPosition.x;

        _questStage =  SaveSystem.GetQuestStage();

        if (_questStage < _questObjects.quests.Length)
        {
            QuestPanelItem panelItem = Instantiate(_prefab, _questContainer).GetComponent<QuestPanelItem>();
            _questPanelItems.Add(panelItem);

            string text = Lean.Localization.LeanLocalization.GetTranslationText("Build the " + _questObjects.quests[_questStage].BuildingName);

            panelItem.UpdateText(text, "+ " + _questObjects.quests[_questStage].Exp);
            panelItem.OriginalText = "Build the " + _questObjects.quests[_questStage].BuildingName;

            if ( SaveSystem.IsQuestComplited(_questObjects.quests[_questStage]))
            {
                _questPanelItems[0].SetToCollect(CollectReward);
            }
            else
            {
                if (_questObjects.quests[_questStage].Type == QuestType.Building)
                {
                    BuildingSystem.Instance.OnBuildNewBuilding.AddListener(OnBuildBuilding);
                }
            }
        }
        else
        {
            _noQuestText.SetActive(true);
        }
    }

    public void OnBuildBuilding(string name)
    {
        if (name == _questObjects.quests[_questStage].BuildingName)
        {
            _questPanelItems[0].SetToCollect(CollectReward);
            _notification.SetActive(true);
            SaveSystem.SetQuestComplited(_questObjects.quests[_questStage]);
            BuildingSystem.Instance.OnBuildNewBuilding.RemoveListener(OnBuildBuilding);
        }
    }

    public void CollectReward()
    {
        SaveSystem.AddLvl(_questObjects.quests[_questStage].Exp);

        _questStage++;
        Destroy( _questPanelItems[0].gameObject);
        _questPanelItems.Clear();
        SaveSystem.SetQuestStage(_questStage);

        if (_questStage < _questObjects.quests.Length)
        {
            if (_questObjects.quests[_questStage].Type == QuestType.Building)
            {
                BuildingSystem.Instance.OnBuildNewBuilding.AddListener(OnBuildBuilding);
            }

            QuestPanelItem panelItem = Instantiate(_prefab, _questContainer).GetComponent<QuestPanelItem>();

            _questPanelItems.Add(panelItem);

            string text = Lean.Localization.LeanLocalization.GetTranslationText( "Build the " + _questObjects.quests[_questStage].BuildingName);
            panelItem.OriginalText = "Build the " + _questObjects.quests[_questStage].BuildingName;

            panelItem.UpdateText(text, "+ " + _questObjects.quests[_questStage].Exp);
        }
        else
        {
            _noQuestText.SetActive(true);
        }
    }

    public void TogglePanel()
    {
        if (_isOpen)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }

        _isOpen = !_isOpen;

        _notification.SetActive(false);
    }

    public void OpenPanel()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_questIcon.DOScale(0f, 0.2f));
        sequence.Append(_closeIcon.DOScale(1f, 0.2f));

        RectTransform.DOAnchorPosX(0f, 0.5f);
    }

    public void ClosePanel()
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(_closeIcon.DOScale(0f, 0.2f));
        sequence.Append(_questIcon.DOScale(1f, 0.2f));

        RectTransform.DOAnchorPosX(_startPosX, 0.5f);
    }
}
