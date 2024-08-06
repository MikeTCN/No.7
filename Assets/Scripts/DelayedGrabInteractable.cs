using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DelayedGrabInteractable : XRGrabInteractable
{
    [SerializeField] private float delayBeforeGrabbable = 5f; // 可在Inspector中設置的延遲時間
    [SerializeField] private int sceneIndexToLoad = 1; // 可在Inspector中設置的場景索引

    private float timer = 0f;
    private bool isGrabbable = false;
    private bool isTransitioning = false;

    private SceneTransitionManager sceneTransitionManager;

    protected override void Awake()
    {
        base.Awake();
        // 初始化時禁用抓取
        interactionLayers = 0; // 禁用所有交互層

        // 獲取 SceneTransitionManager 實例
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        if (sceneTransitionManager == null)
        {
            Debug.LogError("SceneTransitionManager not found in the scene!");
        }
    }

    private void Update()
    {
        if (!isGrabbable)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeGrabbable)
            {
                EnableGrabbing();
            }
        }
    }

    private void EnableGrabbing()
    {
        isGrabbable = true;
        // 啟用所有交互層
        interactionLayers = -1; // 啟用所有交互層
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isTransitioning && sceneTransitionManager != null)
        {
            isTransitioning = true;
            StartSceneTransition();
        }
    }

    private void StartSceneTransition()
    {
        sceneTransitionManager.GoToSceneAsync(sceneIndexToLoad);
    }
}