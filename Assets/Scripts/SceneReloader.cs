using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneReloader : MonoBehaviour
{
    [SerializeField] private Button buttonReload;
    private void Awake()
    {
        buttonReload.onClick.AddListener(ReloadLevel);
    }
    public void ReloadLevel()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.Play(Sounds.ButtonClick);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
