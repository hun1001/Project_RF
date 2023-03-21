using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private RectTransform _joystickBackground;

        [SerializeField]
        private RectTransform _knob;

        private Vector2 _pointPosition;

        private Vector2 _joystickOriginPosition = Vector2.zero;
        private Vector2 _direction = Vector2.zero;

        private Action _onPointerDown = null;
        public Action OnPointerDownAction
        {
            set => _onPointerDown += value;
        }

        private Action _onPointerUp = null;
        public Action OnPointerUpAction
        {
            set => _onPointerUp += value;
        }

        private void Awake()
        {
            _joystickOriginPosition = _joystickBackground.position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown?.Invoke();
            _joystickBackground.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            _pointPosition = new Vector2((eventData.position.x - _joystickBackground.position.x) / ((_joystickBackground.rect.size.x - _knob.rect.size.x) / 2), (eventData.position.y - _joystickBackground.position.y) / ((_joystickBackground.rect.size.y - _knob.rect.size.y) / 2));
            _pointPosition = (_pointPosition.magnitude > 1.0f) ? _pointPosition.normalized : _pointPosition;
            _knob.transform.position = new Vector2(_joystickBackground.position.x + (_pointPosition.x * ((_joystickBackground.rect.size.x - _knob.rect.size.x) / 2)), _joystickBackground.position.y + (_pointPosition.y * ((_joystickBackground.rect.size.y - _knob.rect.size.y) / 2)));

            _direction = _pointPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onPointerUp?.Invoke();
            _joystickBackground.position = _joystickOriginPosition;
            _knob.position = _joystickOriginPosition;
            _direction = Vector2.zero;
        }

        public Vector2 Direction => _direction.normalized;
        public float Magnitude => _direction.magnitude;
    }
}
