using GameResult;
using Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotationObject : MonoBehaviour
{
    [SerializeField]
    private Button _rotationGameBtn;

    [SerializeField]
    private Button _rotationOkGameBtn;

    [SerializeField]
    private Transform _transform;

    private float _rotationAmount = 90f;

    [SerializeField]
    private Canvas _canvas;

    private void Awake()
    {
        //_canvas = GetComponent<Canvas>();
        _canvas.enabled = true;
        _rotationOkGameBtn.enabled = true;
        _rotationGameBtn.enabled = true;
        _rotationOkGameBtn.onClick.AddListener(OnRotationOkBtnClicked);
        _rotationGameBtn.onClick.AddListener(OnRotationBtnClicked);
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        Rotate();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        _transform = null;
    //    }
    //}

    //private void Update()
    //{
    //    OnPointerDown();
    //}

    //private void OnPointerDown()
    //{
    //    if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        _transform = null;
    //        _rotationOkGameBtn.enabled = false;
    //        _rotationGameBtn.enabled = false;
    //        _canvas.enabled = false;
    //    }
    //}

    private void OnRotationOkBtnClicked()
    {
        _transform = null;
        _rotationOkGameBtn.enabled = false;
        _rotationGameBtn.enabled = false;
        _canvas.enabled = false;
    }

    private void OnRotationBtnClicked()
    {
        Rotate();
    }


    private void Rotate()
    {
        if (_transform != null)
        {
            Vector3 currentRotation = _transform.eulerAngles;
            currentRotation.y += _rotationAmount;
            _transform.eulerAngles = currentRotation;
        }
    }
}
