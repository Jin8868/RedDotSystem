using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDotButton : MonoBehaviour
{
    public RedDotViewNode curRedDotNode;
    public Button addRedDotButton;
    public Button subRedDotButton;
   
    void Awake()
    {
        addRedDotButton.onClick.AddListener(OnClickAddRedDot);
        subRedDotButton.onClick.AddListener(OnClickSubRedDot);
    }
    
    private void OnClickSubRedDot()
    {
       curRedDotNode.SubRedDot();
    }

    private void OnClickAddRedDot()
    {
        curRedDotNode.AddRedDot();
        
    }

    private void OnDestroy()
    {
        addRedDotButton.onClick.RemoveListener(OnClickAddRedDot);
        subRedDotButton.onClick.RemoveListener(OnClickSubRedDot);
    }
}
