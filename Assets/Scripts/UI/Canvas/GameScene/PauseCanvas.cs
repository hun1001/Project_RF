using UnityEngine;

public class PauseCanvas : BaseCanvas
{
    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        Time.timeScale = 0f;
    }
}
