using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static bool IsOver { get; private set; } = false;
    Rect menuRect=new Rect(0,0,Screen.width*.2f,Screen.height);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        menuRect = GUI.Window(0, menuRect, GameWindow, "");
        IsOver = menuRect.Contains(Event.current.mousePosition);
    }
    void GameWindow(int _id)
    {
        GUILayout.Box("Sims Like Catalog");
        DrawGameItemUI();
        GUI.DragWindow();
    }
    void DrawGameItemUI()
    {
        GameItem[] _items = GameBDD.Instance.Catalog;
        int _max = _items.Length;
        for (int i = 0; i < _max; i++)
        {
            if (GUILayout.Button(_items[i].name))
                ItemPlacementManager.Instance.CreateItem(_items[i]);

        }
    }
}
