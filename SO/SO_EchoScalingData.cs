using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// Echo Scaling Data 클래스
/// 
/// YWS : 2024.07.01
/// </summary>
[CreateAssetMenu(fileName = "Echo Scaling Data", menuName = "Scriptable Object/Echo Scaling Data", order = int.MaxValue)]
public class SO_EchoScalingData : SerializedScriptableObject
{
    [SerializeField] private PoolingType type;                      // ObjectPooling Type
    [SerializeField][MinValue(0)] private float scalingTime = 10;   // 최대 크기에 도달하는대 걸리는 시간
    [SerializeField][MinValue(0)] private float maxScale = 100;     // 최대 크기
    private float inverseScalingTime = 0;                           // scalingTime 의 역수(Echo Script에서 추가 연산을 하지 않기 위해)   



    private void OnEnable()
    {
        inverseScalingTime = 1 / scalingTime;
    }


    public PoolingType Type { get => type; }
    public float ScalingTime { get => scalingTime; }
    public float InverseScalingTime { get => inverseScalingTime; }
    public float MaxScale { get => maxScale; }
}
