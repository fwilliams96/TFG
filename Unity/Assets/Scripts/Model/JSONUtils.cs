﻿using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;

public class JSONUtils {

    public static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node,
                                           bool showNodeName)
    {
        if (showNodeName)
        {
            sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
        }
        sbJSON.Append("{");

        // Building a sorted list of key-value pairs where key is case-sensitive 
        // nodeName value is an ArrayList of string or XmlElement so that we know 
        // whether the nodeName is an array or not. 
        SortedList<string, object> childNodeNames = new SortedList<string, object>();

        // Add in all node attributes. 
        if (node.Attributes != null)
        {
            foreach (XmlAttribute attr in node.Attributes)
                StoreChildNode(childNodeNames, attr.Name, attr.InnerText);
        }

        // Add in all nodes. 
        foreach (XmlNode cnode in node.ChildNodes)
        {
            if (cnode is XmlText)
            {
                StoreChildNode(childNodeNames, "value", cnode.InnerText);
            }
            else if (cnode is XmlElement)
            {
                StoreChildNode(childNodeNames, cnode.Name, cnode);
            }
        }

        // Now output all stored info. 
        foreach (string childname in childNodeNames.Keys)
        {
            List<object> alChild = (List<object>)childNodeNames[childname];
            if (alChild.Count == 1)
                OutputNode(childname, alChild[0], sbJSON, true);
            else
            {
                sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                foreach (object Child in alChild)
                    OutputNode(childname, Child, sbJSON, false);
                sbJSON.Remove(sbJSON.Length - 2, 2);
                sbJSON.Append(" ], ");
            }
        }
        sbJSON.Remove(sbJSON.Length - 2, 2);
        sbJSON.Append(" }");
    }

    private static void StoreChildNode(SortedList<string, object> childNodeNames,
           string nodeName, object nodeValue)
    {
        // Pre-process contraction of XmlElements. 
        if (nodeValue is XmlElement)
        {
            // Convert <aa></aa> into "aa":null 
            // <aa>xx</aa> into "aa":"xx". 
            XmlNode cnode = (XmlNode)nodeValue;
            if (cnode.Attributes.Count == 0)
            {
                XmlNodeList children = cnode.ChildNodes;
                if (children.Count == 0)
                {
                    nodeValue = null;
                }
                else if (children.Count == 1 && (children[0] is XmlText))
                {
                    nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
        }
        // Add nodeValue to ArrayList associated with each nodeName. 
        // If nodeName doesn't exist then add it. 
        List<object> ValuesAL;

        if (childNodeNames.ContainsKey(nodeName))
        {
            ValuesAL = (List<object>)childNodeNames[nodeName];
        }
        else
        {
            ValuesAL = new List<object>();
            childNodeNames[nodeName] = ValuesAL;
        }
        ValuesAL.Add(nodeValue);
    }

    private static void OutputNode(string childname, object alChild,
            StringBuilder sbJSON, bool showNodeName)
    {
        if (alChild == null)
        {
            if (showNodeName)
            {
                sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
            }
            sbJSON.Append("null");
        }
        else if (alChild is string)
        {
            if (showNodeName)
            {
                sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
            }
            string sChild = (string)alChild;
            sChild = sChild.Trim();
            sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
        }
        else
        {
            XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
        }
        sbJSON.Append(", ");
    }

    private static string SafeJSON(string sIn)
    {
        StringBuilder sbOut = new StringBuilder(sIn.Length);
        foreach (char ch in sIn)
        {
            if (Char.IsControl(ch) || ch == '\'')
            {
                int ich = (int)ch;
                sbOut.Append(@"\u" + ich.ToString("x4"));
                continue;
            }
            else if (ch == '\"' || ch == '\\' || ch == '/')
            {
                sbOut.Append('\\');
            }
            sbOut.Append(ch);
        }
        return sbOut.ToString();
    }

    public static JSONNode XMLFileToJSON(string pathFromXML)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(pathFromXML);
        return XMLToJSON(xmlDoc);
    }

    public static JSONNode XMLStringToJSON(string xmlString)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);
        return XMLToJSON(xmlDoc);
    }

    private static JSONNode XMLToJSON(XmlDocument xmlDoc)
    {
        StringBuilder sbJSON = new StringBuilder();
        sbJSON.Append("{ ");
        global::JSONUtils.XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
        sbJSON.Append("}");
        return JSON.Parse(sbJSON.ToString());
    }

    public static JSONNode StringToJSON(string json)
    {
        return JSON.Parse(json);
    }
}
