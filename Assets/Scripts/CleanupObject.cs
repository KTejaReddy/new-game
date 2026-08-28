using UnityEngine;

public class CleanupObject : MonoBehaviour
{
    private Transform player;
    public float destroyDistanceBehind = 30f;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (transform.position.z < player.position.z - destroyDistanceBehind)
        {
            gameObject.SetActive(false);
        }
    }
}
