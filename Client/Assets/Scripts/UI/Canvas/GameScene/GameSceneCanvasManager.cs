public class GameSceneCanvasManager : BaseSceneCanvasManager
{
    protected override void Awake()
    {
        base.Awake();
        _activeCanvas = CanvasType.Information;
    }

    protected override void InputEscape()
    {
        if (_openDelay <= 0f)
        {
            if (_activeCanvas == CanvasType.Information)
            {
                ChangeCanvas(CanvasType.Pause, _activeCanvas);
            }
            else if (_activeCanvas != CanvasType.GameOver)
            {
                ChangeBeforeCanvas();
            }
        }
    }
}
