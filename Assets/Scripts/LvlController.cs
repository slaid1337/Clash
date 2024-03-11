using TMPro;
using UnityEngine;

public class LvlController : Singletone<LvlController>
{
    [SerializeField] private TMP_Text _lvlNumber;
    [SerializeField] private TMP_Text _lvltext;
    [SerializeField] private RectTransform _lvlFill;
    [SerializeField] private LvlObject _lvlObject;
    private int _currentLvl;
    private float _startFill;

    private void Awake()
    {
        _currentLvl = SaveSystem.GetLvl();
        _startFill = _lvlFill.sizeDelta.x;
        UpdateLvl(_currentLvl);
        SaveSystem.OnLvlChenged += UpdateLvl;
    }

    private void UpdateLvl(int lvl)
    {
        for (int i = 0; i < _lvlObject.Experience.Length; i++)
        {
            if (lvl < _lvlObject.Experience[i])
            {
                _lvlNumber.text = (i).ToString();
                _lvltext.text = lvl + "/" + _lvlObject.Experience[i];
                _lvlFill.sizeDelta = new Vector2(Mathf.Lerp(0, _startFill, Mathf.InverseLerp(_lvlObject.Experience[i-1], _lvlObject.Experience[i], lvl)), _lvlFill.sizeDelta.y);
                _currentLvl = i;
                return;
            }
            else if (lvl == _lvlObject.Experience[i])
            {
                _lvlNumber.text = (i + 1).ToString();
                _lvltext.text = 0 + "/" + _lvlObject.Experience[i + 1];
                _lvlFill.sizeDelta = new Vector2(Mathf.Lerp(0, _startFill, Mathf.InverseLerp(_lvlObject.Experience[i], _lvlObject.Experience[i + 1], lvl)), _lvlFill.sizeDelta.y);
                _currentLvl = i + 1;
                return;
            }
        }
    }

    public int GetCurrentLevel()
    {
        return _currentLvl;
    }

    private void OnDestroy()
    {
        SaveSystem.OnLvlChenged -= UpdateLvl;
    }
}
