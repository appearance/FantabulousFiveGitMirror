﻿using UnityEngine;
using System.Collections;

public static class Extensions {

    // Collections

    public static T RandomItem<T>(this T[] input) 
    {
        return input[Random.Range(0, input.Length - 1)];
    }

    //public static T RandomItem<T>(this T input) where T : Something // do arrays and list share a type?
    //{
    //    return input[Random.Range(0, input. - 1)];
    //}


    // Transforms

    public static void AddChild(this Transform parent, Transform child)
    {
        if (!child.IsChildOf(parent))
        {
            child.SetParent(child);
        }

    }


    public static void AddChildren(this Transform parent, Transform[] children)
    {
        for (int i = 0; i < children.Length; i++)
        {
            Transform child = children[i];
            if (!child.IsChildOf(parent))
            {
                child.SetParent(parent);
            }
        }
    }

    public static void RemoveFromParent(this Transform child)
    {
        if (child.parent != null)
        {
            child.parent = null;
        }
    }



    public static Transform GetChildWithTag(this Transform parent, string tag)
    {
        foreach (Transform tr in parent)
        {
            if (tr.CompareTag(tag))
            {
                return tr;
            }
        }
        return null;
    }

    public static T FindComponentInChildWithTag<T>(this Transform parent, string tag) where T : Component
    {
        foreach (Transform tr in parent)
        {
            if (tr.CompareTag(tag))
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }


    // Quaternions
    // Does this work?
    public static Quaternion RotateBy(this Quaternion left, Quaternion right)
    {
        left = right * left;
        return left;
    }


    // 2D

    // Logic created by @DanInFiction on Twitter, I stole it to make an extension method.
    public static void LookAt2D(this Transform self, Transform target)
    {

        self.right = target.position - self.position;
    }
}
