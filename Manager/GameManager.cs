using UnityEngine;

/// <summary>
/// GameManager Å¬·¡½º
/// 
/// YWS : 2024.07.05
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Material groundMat;
    [SerializeField] private Material numMat;

    int echoId = 0;
    private float[] echoRadiusArr = new float[Constants.ECHO_MAX_COUNT];
    private Vector4[] echoPosition = new Vector4[Constants.ECHO_MAX_COUNT];

    private ObjectPool objectPool = new ObjectPool();
    private SoundManager soundManager = new SoundManager();
    private SceneManagerEX sceneManager = new SceneManagerEX();
    private UIManager uiManager = new UIManager();
    private DataManager dataManager = new DataManager();
    private PlayerPresenter playerPresenter;

    public ObjectPool ObjectPool { get => objectPool; }
    public SoundManager SoundManager { get => soundManager; }
    public SceneManagerEX SceneManager { get => sceneManager; }
    public UIManager UIManager { get => uiManager; }
    public DataManager DataManager { get => dataManager; }
    public PlayerPresenter PlayerPresenter { get => playerPresenter; }



    private void OnDisable()
    {
        soundManager.Release();
        objectPool.Release();
        uiManager.Release();
    }



    private void InitManagers()
    {
        dataManager.Init();

        GameObject update = new GameObject("UpdateManager");
        update.transform.SetParent(transform);
        update.AddComponent<UpdateManager>();

        GameObject pool = new GameObject("Pool");
        pool.transform.SetParent(transform);
        objectPool.Init(pool.transform);

        GameObject sound = new GameObject("Sound");
        sound.transform.SetParent(transform);
        soundManager.Init(sound.transform);

        GameObject ui = new GameObject("UI");
        ui.transform.SetParent(transform);
        playerPresenter = ui.AddComponent<PlayerPresenter>();
        uiManager.Init(ui.transform);
        

        groundMat.SetFloatArray("_EchoRadius", echoRadiusArr);
        groundMat.SetVectorArray("_EchoPosition", echoPosition);

        numMat.SetFloatArray("_EchoRadius", echoRadiusArr);
        numMat.SetVectorArray("_EchoPosition", echoPosition);
    }

    private void InitSetting()
    {
        Object[] UIPrefabs = Resources.LoadAll("Prefab/UI");

        foreach (Object obj in UIPrefabs)
        {
            GameObject go = obj as GameObject;
            GameObject instance = Instantiate<GameObject>(go);

            if (instance != null)
            {
                if (instance.TryGetComponent(out ISettingData settingData))
                    settingData.LoadSettingData();
            }

            Destroy(instance);
        }
    }

    protected override void Init()
    {
        InitSetting();
        InitManagers();

        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

    public void UpdateEchoRadiusAndPosition(int echoID, float radius, Vector4 position)
    {
        echoRadiusArr[echoID] = radius;
        echoPosition[echoID] = position;
    }

    public int GetEchoID()
    {
        echoId = echoId % Constants.ECHO_MAX_COUNT;

        return echoId++;
    }

    private void FixedUpdate()
    {
        groundMat.SetFloatArray("_EchoRadius", echoRadiusArr);
        groundMat.SetVectorArray("_EchoPosition", echoPosition);

        numMat.SetFloatArray("_EchoRadius", echoRadiusArr);
        numMat.SetVectorArray("_EchoPosition", echoPosition);
    }
}