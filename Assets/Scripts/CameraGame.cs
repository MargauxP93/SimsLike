using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;

public class CameraGame : MonoBehaviour
{
    public event Action<GameObject> OnWallDetection = null;

    [SerializeField] Transform target=null;
    [SerializeField] LayerMask wallLayer=0;
    [SerializeField] Camera sensorCamera = null;
    [SerializeField, Range(0,360)] float orbitAngle = 0;
    //[SerializeField] float orbitAngle = 0;
    [SerializeField] float radius = 20;
    [SerializeField] float height = 5;
    [SerializeField] float rotateCameraSpeed = 2;
    [SerializeField] float customTickRate = 0.2f;
    [SerializeField] float detectionDistance = 20;

    GameObject lastWall= null;
    Controls controls = null;
    InputAction rotate = null;

    public bool IsValid => target && sensorCamera;
    public Camera SensorCamera => sensorCamera;

    private void Awake()
    {
        controls = new Controls();
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        CameraOrbitRotation();
        CameraOrbitLookAt();
    }
    private void OnEnable()
    {
        rotate = controls.SimsCamera.Rotate;
        rotate.Enable();
    }
    void Init()
    {
        OnWallDetection += HideWall;
        InvokeRepeating(nameof(WallDetection), 0, customTickRate);
    }
    void WallDetection()
    {
        if (!IsValid) return;
        Ray _rayCamera = sensorCamera.ViewportPointToRay(new Vector3(.5f, .5f));
        bool _wallHit = Physics.Raycast(_rayCamera, out RaycastHit _hit, detectionDistance, wallLayer);
        Debug.DrawRay(_rayCamera.origin, _rayCamera.direction * detectionDistance, Color.red,customTickRate);
        if (!_wallHit) return;
        OnWallDetection?.Invoke(_hit.transform.gameObject);
    }
    void HideWall(GameObject _wall)
    {
        if (!_wall) return;
        //MeshRenderer _lastRenderer=lastWall.GetComponent<MeshRenderer>();
        MeshRenderer _newRenderer=_wall.GetComponent<MeshRenderer>();
        if(lastWall&&_wall.GetInstanceID()!=lastWall.GetInstanceID())
        {
            MeshRenderer _lastRenderer = lastWall.GetComponent<MeshRenderer>();
            //lastWall.SetActive(true);
            Color _color = _lastRenderer.material.color;
            _lastRenderer.material.color = new Color(_color.r, _color.g, _color.b, 1f);
        }
        lastWall = _wall;
        //lastWall.SetActive(false);
        Color _newColor=_newRenderer.material.color;
        _newColor.a = 0.0f;
        _newRenderer.material.color = _newColor;
        //_newRenderer.material.color = new Color(_newColor.r, _newColor.g, _newColor.b, 0f);

    }
    void CameraOrbitRotation()
    {
        if(!IsValid) return;


        float _rotationValue =rotate.ReadValue<float>();
        orbitAngle = GetAngle(orbitAngle, _rotationValue);
        float _angle=Mathf.Deg2Rad * orbitAngle;
        float _x=Mathf.Cos(_angle)*radius;
        float _z=Mathf.Sin(_angle)*radius;
        sensorCamera.transform.position = new Vector3(_x, height, _z) + target.position;
    }
    void CameraOrbitLookAt()
    {
        if (!IsValid) return;
        Transform _transform = sensorCamera.transform;
        _transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
    float GetAngle(float _angle,float _axis)
    {
        _angle += _axis * rotateCameraSpeed;
        _angle %= 360;
        return _angle;
    }
    public Ray GetRayFromCursor(Cursor _cursor)
    {
        return sensorCamera.ScreenPointToRay(_cursor.CursorLocation);
    }
}
