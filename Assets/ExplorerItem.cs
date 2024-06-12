using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class ExplorerItem : MonoBehaviour
{
    [SerializeField]
    Image _explorerImage;
    [SerializeField]
    TextMeshProUGUI _explorerNameTextMesh;
    Explorer _explorer;

    public Explorer Explorer { get => _explorer; set => _explorer = value; }

    
    public void LinkExplorer(Explorer explorer)
    {
        _explorer = explorer;
        _explorerNameTextMesh.text = explorer.Name;
    }

    public void SetImage(Sprite sprite)
    {
        _explorerImage.sprite = sprite;
    }
   
}