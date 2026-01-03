using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotSystem
{
    private RedDotDataNode m_rootNode;

    #region 单例模式

    private static RedDotSystem m_Instance;

    public static RedDotSystem Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new RedDotSystem();
            }

            return m_Instance;
        }
    }

    private RedDotSystem()
    {
        m_rootNode = new RedDotDataNode(RedDotNameDefine.RedDotRoot, 0, null);
    }

    #endregion

    public void AddRedDotNode(string redDotPath,int redDotNum,Action<RedDotDataNode> refreshCallBack)
    {
        var redDotSplitName = SplitRedDotPath(redDotPath);
        if (redDotSplitName == null || redDotSplitName.Length <= 0)
        {
            return;
        }

        RedDotDataNode curRedDotNode = m_rootNode;
        for (int i = 0; i < redDotSplitName.Length; i++)
        {
            string redDotName = redDotSplitName[i];

            bool isLast = i >= redDotSplitName.Length - 1;
            
            if (isLast)
            {
                curRedDotNode = curRedDotNode.AddRedDot(redDotPath,redDotName, redDotNum,refreshCallBack);
            }
            else
            {
                curRedDotNode = curRedDotNode.AddRedDot(redDotPath,redDotName,0,null);
            }
           
        }

        if (redDotNum > 0)
        {
            curRedDotNode.RefreshAddRedDotNum(redDotNum);
        }
    }

    public void SubRedDotNode(string redDotPath, int redDotNum)
    {
        if (redDotNum <= 0)
        {
            return;
        }
        
        var redDotSplitName = SplitRedDotPath(redDotPath);
        if (redDotSplitName == null || redDotSplitName.Length <= 0)
        {
            return;
        }
        RedDotDataNode curRedDotNode = m_rootNode;
        for (int i = 0; i < redDotSplitName.Length; i++)
        {
            if (curRedDotNode == null)
            {
                return;
            }
            
            string redDotName = redDotSplitName[i];
            if (i ==  redDotSplitName.Length - 1)
            {
                curRedDotNode = curRedDotNode.SubRedDot(redDotPath,redDotName, redDotNum);
            }
            else
            {
                curRedDotNode = curRedDotNode.SubRedDot(redDotPath,redDotName,0);
            }
        }

        if (curRedDotNode.GetIsSubChange())
        {
            curRedDotNode.RefreshSubRedDotNum(redDotNum);
        }
    }

    public void RefreshRedDotTree()
    {
        if (m_rootNode == null )
        {
            return;
        }

        m_rootNode.RefreshAllRedDotNode();
    }

    public void Dispose()
    {
        if (m_rootNode == null)
        {
            return;
        }
        
        m_rootNode.Dispose();
        m_rootNode = null;
    }
    
    private string[] SplitRedDotPath(string redDotPath)
    {
        if (string.IsNullOrEmpty(redDotPath))
        {
            return null;
        }

        var result = redDotPath.Split(RedDotNameDefine.RED_DOT_SPLIT);

        return result;
    }
}