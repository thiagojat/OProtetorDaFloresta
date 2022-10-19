using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinsHandler : MonoBehaviour
{
    public List<PinScript> pins;
    public List<EnemyAIHandler> enemies;

    private void Start()
    {
        int i = 0;
        foreach (EnemyAIHandler enemy in enemies)
        {
            pins[i].targetTransform = enemy.transform;
            i++;
        }
    }
}
