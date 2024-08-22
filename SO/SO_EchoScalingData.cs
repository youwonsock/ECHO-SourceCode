using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// Echo Scaling Data Ŭ����
/// 
/// YWS : 2024.07.01
/// </summary>
[CreateAssetMenu(fileName = "Echo Scaling Data", menuName = "Scriptable Object/Echo Scaling Data", order = int.MaxValue)]
public class SO_EchoScalingData : SerializedScriptableObject
{
    [SerializeField] private PoolingType type;                      // ObjectPooling Type
    [SerializeField][MinValue(0)] private float scalingTime = 10;   // �ִ� ũ�⿡ �����ϴ´� �ɸ��� �ð�
    [SerializeField][MinValue(0)] private float maxScale = 100;     // �ִ� ũ��
    private float inverseScalingTime = 0;                           // scalingTime �� ����(Echo Script���� �߰� ������ ���� �ʱ� ����)   



    private void OnEnable()
    {
        inverseScalingTime = 1 / scalingTime;
    }


    public PoolingType Type { get => type; }
    public float ScalingTime { get => scalingTime; }
    public float InverseScalingTime { get => inverseScalingTime; }
    public float MaxScale { get => maxScale; }
}
