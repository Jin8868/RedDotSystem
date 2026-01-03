using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotDataNode
{
    private string m_redDotName;

    private int m_redDotNum;

    private Action<RedDotDataNode> m_refreshCallBack;

    private int m_allRedDotNum;

    private Dictionary<string, RedDotDataNode> m_RedDotDict = new Dictionary<string, RedDotDataNode>();

    private RedDotDataNode m_parentRedDotNode;

    private bool m_isSubChange;

    public RedDotDataNode(string redDotName, int redDotNum, Action<RedDotDataNode> refreshCallBack)
    {
        m_redDotName = redDotName;
        m_redDotNum = redDotNum;
        m_refreshCallBack = refreshCallBack;
    }

    public void SetRedDotNum(int num)
    {
        m_redDotNum = num;
    }

    public int GetRedDotNum()
    {
        return m_redDotNum;
    }

    public int GetAllRedDotNum()
    {
        return m_allRedDotNum;
    }

    public int AddAllRedDotNum(int value)
    {
        m_allRedDotNum += value;
        return m_allRedDotNum;
    }

    public int SubAllRedDotNum(int value)
    {
        m_allRedDotNum -= value;
        m_allRedDotNum = m_allRedDotNum < 0 ? 0 : m_allRedDotNum;

        return m_allRedDotNum;
    }

    public void SetRefreshCallBack(Action<RedDotDataNode> callback)
    {
        m_refreshCallBack = callback;
    }

    public int AddRedDotNum(int count)
    {
        m_redDotNum += count;
        return m_redDotNum;
    }

    public int SubRedDotNum(int count)
    {
        var tempNum = m_redDotNum;
        m_redDotNum -= count;
        m_redDotNum = m_redDotNum < 0 ? 0 : m_redDotNum;
        m_isSubChange = tempNum != m_redDotNum;
        return m_redDotNum;
    }

    public bool GetIsSubChange()
    {
        return m_isSubChange;
    }

    public RedDotDataNode AddRedDot(string redDotPath, string redDotNodeName, int redDotNum,
        Action<RedDotDataNode> refreshCallBack)
    {
        if (m_RedDotDict.Count <= 0)
        {
            var resultNode = new RedDotDataNode(redDotNodeName, redDotNum, refreshCallBack);
            resultNode.m_parentRedDotNode = this;
            m_RedDotDict.Add(redDotNodeName, resultNode);
            return resultNode;
        }

        if (m_RedDotDict.TryGetValue(redDotNodeName, out var redDotDataNode))
        {
            redDotDataNode.AddRedDotNum(redDotNum);
            if (refreshCallBack != null)
            {
                redDotDataNode.SetRefreshCallBack(refreshCallBack);
            }

            return redDotDataNode;
        }
        else
        {
            var resultNode = new RedDotDataNode(redDotNodeName, redDotNum, refreshCallBack);
            resultNode.m_parentRedDotNode = this;
            m_RedDotDict.Add(redDotNodeName, resultNode);
            return resultNode;
        }
    }

    public RedDotDataNode SubRedDot(string redDotPath, string redDotNodeName, int redDotNum)
    {
        if (m_RedDotDict.TryGetValue(redDotNodeName, out var redDotDataNode))
        {
            redDotDataNode.SubRedDotNum(redDotNum);
            return redDotDataNode;
        }

        return null;
    }

    public int RefreshAllRedDotNode()
    {
        m_allRedDotNum = m_redDotNum;

        if (m_RedDotDict.Count <= 0)
        {
            m_refreshCallBack?.Invoke(this);
            return m_allRedDotNum;
        }

        foreach (var redDotDataNode in m_RedDotDict.Values)
        {
            if (redDotDataNode == null)
            {
                continue;
            }

            m_allRedDotNum += redDotDataNode.RefreshAllRedDotNode();
        }

        m_refreshCallBack?.Invoke(this);
        return m_allRedDotNum;
    }

    public void RefreshAddRedDotNum(int addNum)
    {
        if (addNum <= 0)
        {
            return;
        }

        AddAllRedDotNum(addNum);
        var curNode = this;
        curNode.m_refreshCallBack?.Invoke(curNode);
        while (curNode.m_parentRedDotNode != null)
        {
            curNode.m_parentRedDotNode.AddAllRedDotNum(addNum);
            curNode.m_parentRedDotNode.m_refreshCallBack?.Invoke(curNode.m_parentRedDotNode);
            curNode = curNode.m_parentRedDotNode;
        }
    }

    public void RefreshSubRedDotNum(int subNum)
    {
        var curNode = this;
        SubAllRedDotNum(subNum);
        curNode.m_refreshCallBack?.Invoke(curNode);
        while (curNode.m_parentRedDotNode != null)
        {
            curNode.m_parentRedDotNode.SubAllRedDotNum(subNum);
            curNode.m_parentRedDotNode.m_refreshCallBack?.Invoke(curNode.m_parentRedDotNode);
            curNode = curNode.m_parentRedDotNode;
        }
    }

    public void Dispose()
    {
        if (m_RedDotDict == null)
        {
            m_refreshCallBack = null;
            return;
        }

        if (m_RedDotDict.Count <= 0)
        {
            m_refreshCallBack = null;
            m_RedDotDict = null;
            return;
        }

        foreach (var redDotDataNode in m_RedDotDict.Values)
        {
            if (redDotDataNode == null)
            {
                continue;
            }

            redDotDataNode.Dispose();
        }

        m_RedDotDict.Clear();
        m_refreshCallBack = null;
        m_RedDotDict = null;
    }
}