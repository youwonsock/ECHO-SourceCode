

/// <summary>
/// Progress Bar�� ����ϴ� View�� �����ؾ� �ϴ� �������̽�
/// 
/// YWS : 2024.07.04
/// </summary>
public interface IProgressUIView
{
    /// <summary>
    /// Progress Bar�� UI�� ������Ʈ
    /// </summary>
    /// <param name="value">new value</param>
    public void UpdateUI(float value);
}
