public class MenuSceneCanvasManager : BaseSceneCanvasManager
{
    protected override void Awake()
    {
        base.Awake();
        _activeCanvas = CanvasType.Menu;
    }

    protected override void InputEscape()
    {
        if (_openDelay <= 0f)
        {
            if (TutorialManager.Instance.IsTutorial == false)
            {
                if (_activeCanvas != CanvasType.Menu)
                {
                    ChangeBeforeCanvas();
                }
            }
        }
    }
}
