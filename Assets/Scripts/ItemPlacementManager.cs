using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ItemPlacementManager : Singleton<ItemPlacementManager>
{
    [SerializeField] Vector3 itemLocation=Vector3.zero;
    [SerializeField] GameItem currentItem = null;
    [SerializeField] SnapGrid grid = null;
    [SerializeField] bool wrongSelectionCheck = false;
    [SerializeField] float selectorCooldown = 0;

    public bool IsValid => grid;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.Instance.OnEditableSurface +=SetPosition;
        Cursor.Instance.OnSelection += SetItem;
        Cursor.Instance.RotateInput.performed += RotateCurrentItem;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameUI.IsOver) return;
        UpdateItemPosition();
        if (Cursor.Instance.SelectionInput.triggered)
            DropItem();
        
        WrongSelectorCooldown();
    }
    void SetPosition(Vector3 _pos)
    {
        itemLocation = IsValid ? grid.GetSnapPosition(_pos):_pos;
    }
    public void CreateItem(GameItem _item)
    {
        SetItem(Instantiate(_item));

    }
    void SetItem(GameItem _item)
    {
        if (currentItem||wrongSelectionCheck) return;
        currentItem = _item;
        currentItem. SelectItem();
        wrongSelectionCheck = true;

    }
    void UpdateItemPosition()
    {
        if(!IsValid||!currentItem)return;
        currentItem.SetPosition(itemLocation);
    }
    void DropItem()
    {
        if (!currentItem||!currentItem.CanDeselect|| wrongSelectionCheck) return;
        currentItem.DeselectItem();
        currentItem = null;
        
    }
    void RotateCurrentItem(InputAction.CallbackContext _context)
    {
        float _rotateValue = Cursor.Instance.RotateInput.ReadValue<float>();
        if (!currentItem) return;
        currentItem.RotateItem(_rotateValue);
    }
    public void WrongSelectorCooldown()
    {
        if (!wrongSelectionCheck) return;
        selectorCooldown += Time.deltaTime;
        if(selectorCooldown>.5f)
        {
            selectorCooldown = 0;
            wrongSelectionCheck = false;
        }
    }

}
