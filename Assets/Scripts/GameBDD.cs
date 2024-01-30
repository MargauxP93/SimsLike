using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBDD : Singleton<GameBDD>
{
    [SerializeField] GameItem[] catalog = null;
    public GameItem[] Catalog=>catalog;
}
