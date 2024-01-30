using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : Singleton<Cursor>
{
    public event Action<GameItem> OnSelection = null;
    public event Action<Vector3> OnEditableSurface = null;

    [SerializeField] LayerMask editableSurfaceLayer = 0 ,itemLayer= 0 ;
    [SerializeField] CameraGame gameCamera = null ;
    [SerializeField, Range(1, 50)] float detectionDistance = 20;
    Controls controls = null;
    InputAction mousePositionInput = null;
    InputAction selectionInput = null;
    InputAction rotateInput = null;

    public Vector3 CursorLocation => GetMousePosition();
    public bool IsValid => gameCamera;

    public InputAction SelectionInput=>selectionInput;
    public InputAction RotateInput => rotateInput;

    protected override void Awake()
    {
        base.Awake();
        controls=new Controls();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interact(editableSurfaceLayer, OnEditableSurface, detectionDistance);
        if(selectionInput.triggered)
        {
            InteractWithComponent(itemLayer, OnSelection, detectionDistance);
        }
        
    }
    private void OnEnable()
    {
        mousePositionInput = controls.SimsCursor.MousePosition;
        mousePositionInput.Enable();
        selectionInput = controls.SimsCursor.Select;
        selectionInput.Enable();
        rotateInput=controls.SimsCursor.Rotate;
        rotateInput.Enable();
       
    }
    void Interact(LayerMask _validMask,Action<Vector3> _callback,float _distance=20)
    {
        if (!IsValid) return;
        Ray _ray = gameCamera.GetRayFromCursor(this);
        bool _hit = Physics.Raycast(_ray, out RaycastHit _hitRay, _distance, _validMask);
        Debug.DrawRay(_ray.origin,_ray.direction*_distance,_hit?Color.green:Color.red); 
        if(!_hit) return;
        _callback?.Invoke(_hitRay.point);
    }
    
    T InteractWithComponent<T>(LayerMask _validMask,Action<T>_callback,float _distance=20) where T:MonoBehaviour
    {//l'event envoyé doit obligatoirement prendre un monobehaviour au minimum en param
        if(!IsValid) return null;
        Ray _ray = gameCamera.GetRayFromCursor(this); //récupères le rayon entre le curseur et la cam
        bool _hit =Physics.Raycast(_ray,out RaycastHit _hitRay,_distance, _validMask); // fait ke raycast pour detect le layer en param
        if (!_hit) return null;
        //variable de type Template
        T _get = _hitRay.transform.GetComponent<T>();// si object avec layer touché,essaye de récupérer le compo en rapport avec le callback en param
        _callback?.Invoke(_get);//trigger l'event en passant le param récupéré en argument de l'event
        return _get;
    }
    Vector3 GetMousePosition()
    {
        
        Vector2 _mousePos = mousePositionInput.ReadValue<Vector2>();
        return new Vector3(_mousePos.x,_mousePos.y,0);
    }
}
