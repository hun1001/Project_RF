using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankDirectionUIHandle : MonoBehaviour
{
    [SerializeField]
    private Image[] _directionImages = null;

    private void Awake() => Stop();
 
    public void Stop()
    {
        for(int i = 0; i < _directionImages.Length; i++)
        {
            _directionImages[i].enabled = false;
        }
    }

    public void Forward()
    {
        _directionImages[0].enabled = true;
    }

    public void Backward()
    {
        _directionImages[1].enabled = true;
    }

    public void TurnRight()
    {
        _directionImages[2].enabled = true;
        _directionImages[2].rectTransform.localScale = Vector3.up;

        _directionImages[3].enabled = true;
        _directionImages[3].rectTransform.localScale = Vector3.up;
    }

    public void TurnLeft()
    {
        _directionImages[2].enabled = true;
        _directionImages[2].rectTransform.localScale = -Vector3.up;

        _directionImages[3].enabled = true;
        _directionImages[3].rectTransform.localScale = -Vector3.up;
    }
}
