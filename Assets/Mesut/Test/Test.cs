using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using JR;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<Vector3> _vectorList = new List<Vector3>();

    List<float> _firstList = new List<float>();
    List<float> _secondList = new List<float>();

    void Start()
    {
        for (int i = 0; i < 5_000_000; i++)
        {
            var newVector = Vector3.one;
            newVector.x *= GetRandom();
            newVector.x *= GetRandom();
            newVector.x *= GetRandom();

            newVector = newVector.normalized;

            _vectorList.Add(newVector);
        }

    }

    float GetRandom()
    {
        return UnityEngine.Random.Range(-100, 100); ;
    }

    private void Update()
    {
        Dot1();
        Dot2();

        Debug.Log("========");

        Dot2();
        Dot1();

        Calculate();
    }

    private void Check()
    {
        for(int i = 0; i < _firstList.Count; i++)
        {
            var first = _firstList[i];
            var second = _secondList[i];

            if (Mathf.Approximately(first, second))
            {
                Debug.Log("Iterasyon: " + i);
                Debug.Log("First: " + first);
                Debug.Log("Seconds: " + second);
                Debug.Log("They are not same");
                break;
            }
            else
            {
                if (i == _firstList.Count - 1) return;

                Debug.Log("First Vector: " + _vectorList[i]);
                Debug.Log("Seconds Vector: " + _vectorList[i + 1]);
                Debug.Log("First: " + first);
                Debug.Log("Second:" + second);
            }
        }
    }

    private void Calculate1()
    {
        for (int i = 0; i < 5_000_000 - 1; i++)
        {
            Vector3 one = _vectorList[i];
            Vector3 two = _vectorList[i + 1];

            var dot = Vector3.Dot(one, two);
            _firstList.Add(dot);
        }
    }

    private void Calculate2()
    {
        for (int i = 0; i < 5_000_000 - 1; i++)
        {
            Vector3 one = _vectorList[i];
            Vector3 two = _vectorList[i + 1];

            var dot = one.x * two.x + one.y * two.y * one.z * two.z;
            _secondList.Add(dot);
        }
    }

    private void Dot1()
    {
        var start = DateTime.Now;

        for (int i = 0; i < 5_000_000 - 1; i++)
        {
            Vector3 one = _vectorList[i];
            Vector3 two = _vectorList[i + 1];

            var dot = Vector3.Dot(one, two);
        }

        var end = DateTime.Now;
        var timer = end.Subtract(start);

        _firstList.Add(timer.Milliseconds);
    }

    private void Dot2()
    {
        Calculator calculator = new Calculator();
        var start2 = DateTime.Now;

        for (int i = 0; i < 5_000_000 - 1; i++)
        {
            Vector3 one = _vectorList[i];
            Vector3 two = _vectorList[i + 1];

            var dot = one.x * two.x + one.y * two.y + one.z * two.z;

        }

        var end2 = DateTime.Now;

        var timer2 = end2.Subtract(start2);
        _secondList.Add(timer2.Milliseconds);
    }

    private void Calculate()
    {
        Debug.Log("First average: " + (_firstList.Sum() / _firstList.Count));
        Debug.Log("Second Average: " + (_secondList.Sum() / _secondList.Count));
    }

    public float Dot(Vector3 me, Vector3 other)
    {
        return me.x * other.x + me.y * other.y + me.z * other.z;
    }
}

public class Calculator
{
    public float Dot(ref Vector3 one, ref Vector3 two)
    {
        return one.x * two.x + one.y * two.y + one.z * two.z;
    }
}

public static class Vector3Extensions
{
    public static float Dot (ref Vector3 me, ref Vector3 other)
    {
        return me.x * other.x + me.y * other.y + me.z * other.z;
    }
}
