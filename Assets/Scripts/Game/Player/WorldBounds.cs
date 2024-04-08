using UnityEngine;

namespace Game.Player
{
    public class WorldBounds : MonoBehaviour
    {
        private float _maxPosX = 40f, _minPosX =-40f, _maxPosZ = 40f, _minPosZ = -40f;

        public void CheckBounds()
        {
         if(transform.position.x > _maxPosX)
             transform.position = new Vector3(_maxPosX,transform.position.y,transform.position.z);
         if(transform.position.x < _minPosX)
             transform.position = new Vector3(_minPosX,transform.position.y,transform.position.z);
         if(transform.position.z > _maxPosZ)
             transform.position = new Vector3(transform.position.x,transform.position.y,_maxPosZ);
         if(transform.position.z < _minPosZ)
             transform.position = new Vector3(transform.position.x,transform.position.y,_minPosZ);
        }
    }
}