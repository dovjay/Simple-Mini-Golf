using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Select Level")]
    [SerializeField]
    private GameObject selectLevelGroup;
    [SerializeField]
    private GridLayoutGroup levelContainer;
    [SerializeField]
    private GameObject levelButtonPrefab;
    [SerializeField]
    private LevelList levelList;

    [Header("Option")]
    [SerializeField]
    private GameObject settingsGroup;

    private AudioManager audioManager;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        GenerateLevelList();
    }

    private void OnDisable() {
        for (int i = 0; i < levelContainer.transform.childCount; i++)
        {
            Button button = levelContainer.transform.GetChild(i).GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }
    }

    public void GenerateLevelList()
    {
        foreach (Level level in levelList.levels)
        {
            GameObject buttonObject = Instantiate(levelButtonPrefab, levelContainer.transform);
            TextMeshProUGUI buttonText = buttonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            buttonText.text = $"{levelList.levels.IndexOf(level) + 1}";
            Button button = buttonObject.GetComponent<Button>();

            if (!level.isUnlocked()) button.interactable = false;

            Transform starGroup = button.transform.GetChild(1);
            int hideStarCount = 3;
            while (hideStarCount > level.GetTotalStar())
            {
                hideStarCount--;
                starGroup.GetChild(hideStarCount).gameObject.SetActive(false);
            }

            button.onClick.AddListener(() => StartLevel(level.scenePath));
        }
    }

    public void DestroyLevelList() {
        foreach (Transform child in levelContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SelectLevelButton() {
        audioManager.PlaySFX(audioManager.UIClick);
        selectLevelGroup.SetActive(true);
        settingsGroup.SetActive(false);
    }

    public void SettingsButton() {
        audioManager.PlaySFX(audioManager.UIClick);
        settingsGroup.SetActive(true);
        selectLevelGroup.SetActive(false);
    }

    public void QuitGameButton() {
        audioManager.PlaySFX(audioManager.UIClick);
        Application.Quit(0);
    }

    public void StartLevel(string scenePath) {
        Level level = FindLevel(scenePath);
        if (level == null)
        {
            Debug.LogError("Level not found!");
            return;
        }

        if (!level.isUnlocked())
        {
            Debug.LogWarning("Level is locked!");
            return;
        }

        SceneManager.LoadScene(scenePath);
    }

    private Level FindLevel(string scenePath) {
        foreach (Level level in levelList.levels)
        {
            if (level.scenePath == scenePath)
            {
                return level;
            }
        }

        return null;
    }
}
