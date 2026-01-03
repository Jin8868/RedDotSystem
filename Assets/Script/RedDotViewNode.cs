using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDotViewNode : MonoBehaviour
{
    public string redDotPath;
    
    public int defaultRedDotNum;

    public Text redDotAllNumText;

    public Text reDotCurNumText;
    
    private void OnEnable()
    {
        RedDotSystem.Instance.AddRedDotNode(redDotPath,defaultRedDotNum,RefreshText);
    }
    
    public void AddRedDot()
    {
        RedDotSystem.Instance.AddRedDotNode(redDotPath,1,RefreshText);
    }

    public void SubRedDot()
    {
        RedDotSystem.Instance.SubRedDotNode(redDotPath,1);
    }

    private void RefreshText(RedDotDataNode node)
    {
        if (redDotAllNumText == null)
        {
            return;
        }

        redDotAllNumText.text = node.GetAllRedDotNum().ToString();
        reDotCurNumText.text = node.GetRedDotNum().ToString();
        //Debug.Log($"nodeName={node.redDotPath},allNum={node.GetAllRedDotNum()},curNum={node.GetRedDotNum()}");
    }
    
    private void OnDisable()
    {
        //SubRedDot();
    }
}