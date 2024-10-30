using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;

public class GameLoader : Singletone<GameLoader>
{
    [SerializeField] private Image _overlayImage;
    [SerializeField] private RectTransform _loadIcon;

    public UnityEvent OnLoad;

    private bool _isLoad;
    private bool _adClosed;

    public void Start()
    {
        Application.targetFrameRate = 60;

        YandexSDK.instance.onInitializeData += Load;

        YandexSDK.instance.onInterstitialShown += delegate
        {
            _adClosed = true;
        };
        YandexSDK.instance.onInterstitialShown += CheckOnReady;
    }

    private void Load()
    {
        OnLoad?.Invoke();

        Sequence sequence = DOTween.Sequence();

        
        sequence.Append(_overlayImage.DOFade(0f, 1f).OnComplete(delegate
        {
            _isLoad = true;
            Destroy(_overlayImage.gameObject);
            CheckOnReady();
        }));
        sequence.Join(_loadIcon.DOScale(0f, 0.8f));

        SaveSystem.test();

        StartCoroutine(SaveCloud());
    }

    public void CheckOnReady()
    {
        if (_isLoad)
        {
            YandexSDK.Ready();
            YandexSDK.StartTab();
            gameObject.SetActive(false);
        }
    }

    public void DeleteSaves()
    {
        SaveSystem.DeleteAllSaves();
    }

    public void DeletePLayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private IEnumerator SaveCloud()
    {
        yield return new WaitForSeconds(5);

        SaveSystem.SendCloudData();

        print("Save to cloud");

        SaveSystem.SetSaveDate(DateTime.Now.ToString());
        SaveSystem.SetSaveDate(DateTime.Now.ToString(), isCloud: true);

        YandexPrefs.SetString("testDate", DateTime.Now.ToString());

        StartCoroutine(SaveCloud());
    }
}