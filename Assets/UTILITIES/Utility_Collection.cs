using UnityEngine;
using UnityEngine.AI;
namespace UTILITIES
{
    public static class Utility_Collection
    {
        public static Vector3 GetRandomLocationAtAreaForNavmesh(GameObject areaObject)
        {
            float areaCenterX = areaObject.transform.position.x;
            float areaCenterZ = areaObject.transform.position.z;

            float areaHalfWidth = areaObject.transform.localScale.x / 2f;
            float areaHalfDepth = areaObject.transform.localScale.z / 2f;

            // Generate random positions within the area
            float randomX = UnityEngine.Random.Range(areaCenterX - areaHalfWidth, areaCenterX + areaHalfWidth);
            float randomZ = UnityEngine.Random.Range(areaCenterZ - areaHalfDepth, areaCenterZ + areaHalfDepth);
            float randomY = 0;

            // Use the RANDOM positions for NavMesh sampling
            NavMeshHit hit;

            if (NavMesh.SamplePosition(new Vector3(randomX, 0, randomZ), out hit, 30f, NavMesh.AllAreas))
            {
                randomX = hit.position.x;
                randomZ = hit.position.z;
            }
            else
            {
                Debug.Log("NavMesh.SamplePosition failed. Going to alternative location.");
                return areaObject.transform.position + new Vector3(5, 0, 5);
            }


            // Use the RANDOM positions for raycasting
            RaycastHit groundHit;

            if (Physics.Raycast(new Vector3(randomX, 50f, randomZ), Vector3.down, out groundHit, Mathf.Infinity))
            {
                randomY = groundHit.point.y;
            }

            return new Vector3(randomX, randomY, randomZ);
        }
        public static Vector3 GetRandom_X_Z_LocationAtArea(GameObject areaObject)
        {
            float randomX = UnityEngine.Random.Range(areaObject.transform.position.x - areaObject.transform.localScale.x / 2, areaObject.transform.position.x + areaObject.transform.localScale.x / 2);
            float randomZ = UnityEngine.Random.Range(areaObject.transform.position.z - areaObject.transform.localScale.z / 2, areaObject.transform.position.z + areaObject.transform.localScale.z / 2);
            return new Vector3(randomX, 0, randomZ);
        }
    }
}

