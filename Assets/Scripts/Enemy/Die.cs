using Enemy.interfaces;
using UnityEngine;

namespace Enemy
{
    public class Die<T> : IDie<T>
    {
        private readonly GameObject _gameObject;

        public Die(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public void Died()
        {
            Debug.Log("я вмер");
            
            _gameObject.SetActive(false);
            //regdol
        }
    }
}