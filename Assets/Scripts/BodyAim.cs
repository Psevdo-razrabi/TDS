using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAim : MonoBehaviour
{
   [SerializeField] private BodyType _bodyType;
   private Enemy.Enemy _enemy;
   
   public BodyType BodyPart => _bodyType;
   public Enemy.Enemy Enemy => _enemy;
   
   private void Awake()
   {
      _enemy = GetComponentInParent<Enemy.Enemy>();
   }
}
