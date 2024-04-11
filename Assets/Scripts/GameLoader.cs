using Eiko.YaSDK;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections;
using Eiko.YaSDK.Data;

public class GameLoader : Singletone<GameLoader>
{
    [SerializeField] private Image _overlayImage;
    [SerializeField] private RectTransform _loadIcon;

    public UnityEvent OnLoad;

    public void Start()
    {
        base.Awake();

        YandexSDK.instance.onInitializeData += Load;
    }

    private void Load()
    {
        string localString = PlayerPrefs.GetString("date", "");
        string cloudString = YandexPrefs.GetString("date", "");

        /*if (!string.IsNullOrEmpty(localString))
        {
            DateTime localLastSave = DateTime.Parse(localString);

            if (!string.IsNullOrEmpty(cloudString))
            {
                DateTime cloudLastSave = DateTime.Parse(cloudString);

                print("local: " + localString + " cloud: " + cloudString);

                if (cloudLastSave > localLastSave)
                {
                    await SaveSystem.LoadCloudData();
                    SaveSystem.SetSaveDate(DateTime.Now.ToString());
                    SaveSystem.SetSaveDate(DateTime.Now.ToString(), isCloud: true);
                    print("Cloud Save");
                }
                else if (localLastSave > cloudLastSave)
                {
                    await SaveSystem.SendCloudData();
                    SaveSystem.SetSaveDate(DateTime.Now.ToString());
                    SaveSystem.SetSaveDate(DateTime.Now.ToString(), isCloud: true);
                    print("Local Save");
                }
            }
            else
            {
                await SaveSystem.SendCloudData();
                SaveSystem.SetSaveDate(DateTime.Now.ToString());
                SaveSystem.SetSaveDate(DateTime.Now.ToString(), isCloud: true);
                print("Local Save");
            }
        }
        else if (!string.IsNullOrEmpty(cloudString))
        {
            await SaveSystem.LoadCloudData();
            SaveSystem.SetSaveDate(DateTime.Now.ToString());
            SaveSystem.SetSaveDate(DateTime.Now.ToString(), isCloud: true);
            print("Cloud Save");
        }
        else
        {
            SaveSystem.SetSaveDate(DateTime.Now.ToString());
            print("New Save");
        }*/


        if (!string.IsNullOrEmpty(cloudString))
        {
            SaveSystem.LoadCloudData();
            print("Cloud Save");
        }

        OnLoad?.Invoke();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_loadIcon.DOScale(0f, 0.8f));
        sequence.Append(_overlayImage.DOFade(0f, 1.4f).OnComplete(delegate
        {
            Destroy(_overlayImage.gameObject);
        }));

        SaveSystem.test();

        StartCoroutine(SaveCloud());
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