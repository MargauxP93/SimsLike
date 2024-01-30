using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    [SerializeField] bool isSelected = false;
    [SerializeField] bool canDeselect = true;
    [SerializeField] float rotateValue = 90;
    GameItem collideItem= null; 
    public bool CanDeselect => canDeselect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetPosition(Vector3 _pos)
    {
        if (!isSelected) return;
        transform.position = _pos;
    }
    public void SelectItem()
    {
        isSelected = true;
    }
    public void DeselectItem()
    {
        if (!canDeselect) return;
        isSelected = false;
    }
    public void RotateItem(float _axis)
    {
        if (!isSelected) return;

        transform.eulerAngles += Vector3.up * rotateValue * _axis;
                 
    }
    private void OnTriggerEnter(Collider _other)
    {
        collideItem=_other.GetComponent<GameItem>();
        if (!collideItem) return;
        canDeselect = false;
    }
    private void OnTriggerExit(Collider _other)
    {
        canDeselect = true;
        collideItem = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color=!canDeselect?Color.red:Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
}
