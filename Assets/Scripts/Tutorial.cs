using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialDialogWindows[] _dialogWindows;
    private int[] _layersArr;
    private int _stage;
    [SerializeField] private LayerMask _layersMask;
    private bool _canPlay;
    [SerializeField] private bool _isSingle;


    private void Start()
    {
        bool IsFirstStartLvl = PlayerPrefs.GetInt("Tutor" + SceneManager.GetActiveScene().buildIndex, 0) == 0;

        if ((IsFirstStartLvl || IsFirstStartLvl == _isSingle) && _dialogWindows.Length > 0)
        {
            StartTutorial();
            PlayerPrefs.SetInt("Tutor" + SceneManager.GetActiveScene().buildIndex, 1);
            StartCoroutine(StartCor());
        }
    }

    private IEnumerator StartCor()
    {
        yield return new WaitForSeconds(0.5f);

        _canPlay = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canPlay)
        {
            if (_dialogWindows[_stage].TriggerCollider == null && !IsPointerOverUIObject())
            {
                NextStage();
            }
            else
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, Mathf.Infinity, _layersMask);

                foreach (var hit in hits)
                {
                    if (hit.collider == _dialogWindows[_stage].TriggerCollider)
                    {
                        NextStage();
                    }
                }
            }
        }
    }

    public void StartTutorial()
    {
        _dialogWindows[_stage].Actions.Invoke();
        foreach (var item in _dialogWindows[_stage].Fingers)
        {
            item.SetActive(true);
        }

        _layersArr = new int[_dialogWindows[_stage].Objects.Length];

        for (int i = 0; i < _dialogWindows[_stage].Objects.Length; i++)
        {
            Image image = null;
            SpriteRenderer spriteRenderer = null;
            Collider2D collider2D = null;

            if (_dialogWindows[_stage].Objects[i].TryGetComponent<Image>(out image))
            {
                Canvas canvas = image.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 55;

            }
            else if (_dialogWindows[_stage].Objects[i].TryGetComponent<SpriteRenderer>(out spriteRenderer))
            {
                _layersArr[i] = spriteRenderer.sortingOrder;
                spriteRenderer.sortingOrder = 55;
            }

            if (_dialogWindows[_stage].Objects[i].TryGetComponent<Collider2D>(out collider2D))
            {
                collider2D.enabled = false;
            }
        }

        
    }

    public void NextStage()
    {
        foreach (var item in _dialogWindows[_stage].Fingers)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < _dialogWindows[_stage].Objects.Length; i++)
        {
            Canvas canvas = null;
            SpriteRenderer spriteRenderer = null;
            Collider2D collider2D = null;

            if (_dialogWindows[_stage].Objects[i].TryGetComponent<Canvas>(out canvas))
            {
                Destroy(canvas);
            }
            else if (_dialogWindows[_stage].Objects[i].TryGetComponent<SpriteRenderer>(out spriteRenderer))
            {
                spriteRenderer.sortingOrder = _layersArr[i];
            }

            if (_dialogWindows[_stage].Objects[i].TryGetComponent<Collider2D>(out collider2D))
            {
                collider2D.enabled = true;
            }
        }

        _stage++;

        if (_stage == _dialogWindows.Length)
        {
            gameObject.SetActive(false);
            return;
        }

        foreach (var item in _dialogWindows[_stage].Fingers)
        {
            item.SetActive(true);
        }

        _layersArr = new int[_dialogWindows[_stage].Objects.Length];

        for (int i = 0; i < _dialogWindows[_stage].Objects.Length; i++)
        {
            Image image = null;
            SpriteRenderer spriteRenderer = null;
            Collider2D collider2D = null;

            if (_dialogWindows[_stage].Objects[i].TryGetComponent<Image>(out image))
            {
                Canvas canvas = image.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 55;

            }
            else if (_dialogWindows[_stage].Objects[i].TryGetComponent<SpriteRenderer>(out spriteRenderer))
            {
                _layersArr[i] = spriteRenderer.sortingOrder;
                spriteRenderer.sortingOrder = 55;
            }

            if (_dialogWindows[_stage].Objects[i].TryGetComponent<Collider2D>(out collider2D))
            {
                collider2D.enabled = false;
            }
        }

        _dialogWindows[_stage].Actions.Invoke();
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };

        List<RaycastResult> results = new List<RaycastResult>();

        // Указываем маску слоев для UI
        int layerMask = 1 << LayerMask.NameToLayer("UI");

        // Выполняем Raycast только для UI
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        int count = 0;

        foreach (var item in results)
        {
            // Проверяем, что объект находится на слое UI
            if (item.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                count++;
            }
        }

        return count > 0;
    }
}

[Serializable]
public class TutorialDialogWindows
{
    public GameObject[] Fingers;
    public Collider2D TriggerCollider;
    public GameObject[] Objects;
    public UnityEvent Actions;
}