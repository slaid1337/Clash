using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LvlController : Singletone<LvlController>
{
    [SerializeField] private TMP_Text _lvlNumber;
    [SerializeField] private TMP_Text _lvltext;
    [SerializeField] private RectTransform _lvlFill;
    [SerializeField] private LvlObject _lvlObject;
    private int _currentLvl;
    private int _currentLvlNumber;
    private float _startFill;
    [SerializeField] protected ParticleSystem _particleSystem;
    public UnityEvent OnLvlUp;

    public override void Awake()
    {
        base.Awake();

        _currentLvl = SaveSystem.GetLvl();
        _startFill = _lvlFill.sizeDelta.x;
        UpdateLvl();
        SaveSystem.OnLvlChenged += UpdateLvl ;
    }

    private void UpdateLvl()
    {
        for (int i = 0; i < _lvlObject.Experience.Length; i++)
        {
            if (_currentLvl < _lvlObject.Experience[i])
            {
                _currentLvlNumber = i;

                _lvlNumber.text = (i).ToString();
                _lvltext.text = _currentLvl + "/" + _lvlObject.Experience[i];
                _lvlFill.sizeDelta = new Vector2(Mathf.Lerp(0, _startFill, Mathf.InverseLerp(_lvlObject.Experience[i - 1], _lvlObject.Experience[i], _currentLvl)), _lvlFill.sizeDelta.y);
                _currentLvl = i;
                return;
            }
            else if (_currentLvl == _lvlObject.Experience[i])
            {
                _currentLvlNumber = i + 1;

                _lvlNumber.text = (i + 1).ToString();
                _lvltext.text = 0 + "/" + _lvlObject.Experience[i + 1];
                _lvlFill.sizeDelta = new Vector2(Mathf.Lerp(0, _startFill, Mathf.InverseLerp(_lvlObject.Experience[i], _lvlObject.Experience[i + 1], _currentLvl)), _lvlFill.sizeDelta.y);
                _currentLvl = i + 1;
                return;
            }
        }
    }

    private void UpdateLvl(int lvl)
    {
        for (int i = 0; i < _lvlObject.Experience.Length; i++)
        {
            if (lvl < _lvlObject.Experience[i])
            {
                if (_currentLvlNumber != i)
                {
                    OnLvlUp?.Invoke();
                    print("new lvl");
                }

                _currentLvlNumber = i;

                _lvlNumber.text = (i).ToString();
                _lvltext.text = lvl + "/" + _lvlObject.Experience[i];
                _lvlFill.sizeDelta = new Vector2(Mathf.Lerp(0, _startFill, Mathf.InverseLerp(_lvlObject.Experience[i-1], _lvlObject.Experience[i], lvl)), _lvlFill.sizeDelta.y);
                _currentLvl = i;
                return;
            }
            else if (lvl == _lvlObject.Experience[i])
            {
                if (_currentLvlNumber != i + 1)
                {
                    OnLvlUp?.Invoke();
                    print("new lvl");
                }

                _currentLvlNumber = i + 1;

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
