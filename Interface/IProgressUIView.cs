

/// <summary>
/// Progress Bar를 사용하는 View가 구현해야 하는 인터페이스
/// 
/// YWS : 2024.07.04
/// </summary>
public interface IProgressUIView
{
    /// <summary>
    /// Progress Bar의 UI를 업데이트
    /// </summary>
    /// <param name="value">new value</param>
    public void UpdateUI(float value);
}
