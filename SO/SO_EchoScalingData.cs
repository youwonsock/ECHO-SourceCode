using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// Echo Scaling Data Å¬·¡½º
/// 
/// YWS : 2024.07.01
/// </summary>
[CreateAssetMenu(fileName = "Echo Scaling Data", menuName = "Scriptable Object/Echo Scaling Data", order = int.MaxValue)]
public class SO_EchoScalingData : SerializedScriptableObject
{
    [SerializeField] private PoolingType type;
    [SerializeField][MinValue(0)] private float scalingTime = 10;
    [SerializeField][MinValue(0)] private float maxScale = 100;
    private float inverseScalingTime = 0;



    private void OnEnable()
    {
        inverseScalingTime = 1 / scalingTime;
    }


    public PoolingType Type { get => type; }
    public float ScalingTime { get => scalingTime; }
    public float InverseScalingTime { get => inverseScalingTime; }
    public float MaxScale { get => maxScale; }
}
