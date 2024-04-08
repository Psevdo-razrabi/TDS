using UnityEngine;

namespace Game.Core
{
    public class RandomSpawnPoint : MonoBehaviour
    {
        public Vector3 GetRandomPoint(Transform min, Transform max)
        {
            Vector3 point = Vector3.zero;
            bool verticalSpawn = Random.Range(0f, 1f) > 0.5f;
            if (verticalSpawn)
            {
                point.z = Random.Range(min.position.z, max.position.z);
                point.x = Random.Range(0f,1f) > 0.5f ? min.position.x : max.position.x;
            }
            else
            {
                point.x = Random.Range(min.position.x, max.position.x);
                point.z = Random.Range(0f,1f) > 0.5f ? min.position.z : max.position.z;
            }
            return point;
        }
        
    }
}