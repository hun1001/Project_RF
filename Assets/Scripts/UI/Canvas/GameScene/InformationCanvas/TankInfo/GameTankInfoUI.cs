using UnityEngine;

public class GameTankInfoUI : MonoBehaviour
{
    [SerializeField]
    private TankDirectionUIHandle _tankDirUIHandle = null;

    [SerializeField]
    private RectTransform _tankBodyRectTransform = null;

    [SerializeField]
    private RectTransform _tankTurretRectTransform = null;

    public void UpdateTankBodyRotate(Quaternion rotation)
    {
        _tankBodyRectTransform.localRotation = rotation;
    }
    
    public void UpdateTankTurretRotate(Quaternion rotation)
    {
        _tankTurretRectTransform.localRotation = rotation;
    }

    public void Forward() => _tankDirUIHandle.Forward();
    public void Backward() => _tankDirUIHandle.Backward();
    public void TurnRight() => _tankDirUIHandle.TurnRight();
    public void TurnLeft() => _tankDirUIHandle.TurnLeft();

    public void Stop() => _tankDirUIHandle.Stop();
    public void TurnStop() => _tankDirUIHandle.TurnStop();
}
