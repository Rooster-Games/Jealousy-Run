using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JR;
using RGLevel;
using UnityEngine;

public class PathInjector : MonoBehaviour
{
    [SerializeField] LevelPath _levelPath;
    [SerializeField] GameType _gameType;
    [SerializeField] CinemachineDollyCart _playerDollyCart;
    [SerializeField] CinemachineDollyCart _peopleDollyCart;

    public void InjectPath(CinemachineSmoothPath path)
    {
        _levelPath.SmoothPath = path;
        _gameType.SmoothPath = path;
        _playerDollyCart.m_Path = path;
        _peopleDollyCart.m_Path = path;
    }
}
