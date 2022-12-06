using System;
using _Game.Scripts.Base.Component;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class StartGameComponent : MonoBehaviour, IComponent, IConstructable
    {

        public void Initialize(ComponentContainer _componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");
         
        }

        public void OnConstruct()
        {
          
        }
        

       
    }
}