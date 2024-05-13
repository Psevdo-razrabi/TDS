using Enemy.interfaces;
using UnityEngine;

namespace Enemy
{
    public class Die<T> : IDie<T>
    {
        private readonly GameObject _gameObject;
        private readonly EventController _eventController;

        public Die(GameObject gameObject, EventController eventController)
        {
            _gameObject = gameObject;
            _eventController = eventController;
        }
        
        public void Died()
        {
            Debug.Log("я вмер");

            if (typeof(T) == typeof(Enemy))
            {
                _eventController.OnEnemyDie();
            }
            
            _gameObject.SetActive(false);
            //regdol
        }
    }
}