using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGrid : MonoBehaviour
{
    [SerializeField] MeshRenderer surface = null;
    [SerializeField] int spaceValue = 2;
    [SerializeField] float snapValue = 0;
    
    public Vector3 Extents => IsValid ? surface.bounds.extents : Vector3.zero;
    public bool IsValid => surface;


    private void OnDrawGizmos()
    {
        DrawGrid();
    }
    public Vector3 GetSnapPosition(Vector3 _pos)
    {
        float _x = Mathf.RoundToInt(_pos.x) + snapValue;
        float _z = Mathf.RoundToInt(_pos.z) + snapValue;
        float _xLimit=Extents.x;
        float _zLimit=Extents.z;
        _x=Mathf.Clamp(_x, -_xLimit+1, _xLimit-1);
        _z=Mathf.Clamp(_z, -_zLimit+1, _zLimit-1);
        return new Vector3(_x,_pos.y,_z);
    }
    void DrawGrid()
    {
        if(!IsValid) return;
        Transform _surface=surface.transform;
        float _xLimit= Extents.x;
        float _zLimit= Extents.z;  
        for (float i = -Extents.x; i <= Extents.x; i++)
        {
            for (float j = -Extents.z; j <= Extents.z; j++)
            {
                if ((i == -_xLimit || i == _xLimit) || (j == -_zLimit || j == _zLimit)) continue;
                Gizmos.color = Color.grey;
                Vector3 _gridPoint = new Vector3(i, Extents.y, j) + _surface.position;
                Gizmos.DrawWireSphere(_gridPoint, .15f);
                Gizmos.color = Color.white;
            }
        }
    }
}
