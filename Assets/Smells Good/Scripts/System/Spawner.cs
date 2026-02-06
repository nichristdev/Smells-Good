using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Spawner : MonoBehaviour
{
    [Title("Spawning")]
    public List<ObjectSpawned> ObjectsToSpawn;
    public float MinSpawnTime;
    public float MaxSpawnTime;
    public bool ImmediatelySpawn = true;
    float NextSpawn;

    [Title("Spawn Position")]
    public Axis axis;
    public Direction direction;

    [SerializeField] bool SpawnNearPlayer;
    [SerializeField] float NearPlayerRange;
    PlayerFreeFall player;

    [HideIf("axis", Axis.Vertical)]
    [SerializeField] float MinX;
    [HideIf("axis", Axis.Vertical)]
    [SerializeField] float MaxX;

    [HideIf("axis", Axis.Horizontal)]
    [SerializeField] float MinY;
    [HideIf("axis", Axis.Horizontal)]
    [SerializeField] float MaxY;

    private void Start()
    {
        player = FindObjectOfType<PlayerFreeFall>();

        if (!ImmediatelySpawn)
        {
            NextSpawn = Time.time + Random.Range(MinSpawnTime, MaxSpawnTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= NextSpawn && !ProgressionManager.Completed)
        {
            NextSpawn = Time.time + Random.Range(MinSpawnTime, MaxSpawnTime);
            float ProbabilityPicker = Random.Range(0, 101);

            for (int i = 0; i < ObjectsToSpawn.Count; i++)
            {
                if (ProbabilityPicker > ObjectsToSpawn[i].MinChanceToSpawn && ProbabilityPicker < ObjectsToSpawn[i].MaxChanceToSpawn)
                {
                    GameObject Obj;

                    switch (axis)
                    {
                        case Axis.Horizontal:

                            if (!SpawnNearPlayer)
                            {
                                Obj = Instantiate(ObjectsToSpawn[i].Obj, new Vector2(Random.Range(MinX, MaxX), transform.position.y), ObjectsToSpawn[i].Obj.transform.rotation);
                            }else
                            {
                                Obj = Instantiate(ObjectsToSpawn[i].Obj, new Vector2(Random.Range(player.transform.position.x - NearPlayerRange, player.transform.position.x + NearPlayerRange), transform.position.y), ObjectsToSpawn[i].Obj.transform.rotation);
                            }

                            switch (direction)
                            {
                                case Direction.Up:
                                    Obj.GetComponent<ObjectMover>().MoveDirection = Direction.Up;
                                    break;
                                case Direction.Down:
                                    Obj.GetComponent<ObjectMover>().MoveDirection = Direction.Down;
                                    break;
                            }

                            break;
                        case Axis.Vertical:

                            if (!SpawnNearPlayer)
                            {
                                Obj = Instantiate(ObjectsToSpawn[i].Obj, new Vector2(transform.position.x, Random.Range(MinY, MaxY)), ObjectsToSpawn[i].Obj.transform.rotation);
                            }else
                            {
                                Obj = Instantiate(ObjectsToSpawn[i].Obj, new Vector2(transform.position.x, Random.Range(player.transform.position.y - NearPlayerRange, player.transform.position.y + NearPlayerRange)), ObjectsToSpawn[i].Obj.transform.rotation);
                            }

                            switch (direction)
                            {
                                case Direction.Left:
                                    Obj.GetComponent<ObjectMover>().MoveDirection = Direction.Left;
                                    break;
                                case Direction.Right:
                                    Obj.GetComponent<ObjectMover>().MoveDirection = Direction.Right;
                                    break;
                            }

                            break;

                        case Axis.Both:
                            if (!SpawnNearPlayer)
                            {
                                Obj = Instantiate(ObjectsToSpawn[i].Obj, new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY)), ObjectsToSpawn[i].Obj.transform.rotation);
                            }
                            else
                            {
                                Obj = Instantiate(ObjectsToSpawn[i].Obj, new Vector2(Random.Range(player.transform.position.x - NearPlayerRange, player.transform.position.x + NearPlayerRange), Random.Range(player.transform.position.y - NearPlayerRange, player.transform.position.y + NearPlayerRange)), ObjectsToSpawn[i].Obj.transform.rotation);
                            }

                            switch (direction)
                            {
                                case Direction.Left:
                                    Obj.GetComponent<ObjectMover>().MoveDirection = Direction.Left;
                                    break;
                                case Direction.Right:
                                    Obj.GetComponent<ObjectMover>().MoveDirection = Direction.Right;
                                    break;
                            }

                            break;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        switch (axis)
        {
            case Axis.Horizontal:
                Gizmos.DrawLine(new Vector2(MinX, -1000), new Vector2(MinX, 1000));
                Gizmos.DrawLine(new Vector2(MaxX, -1000), new Vector2(MaxX, 1000));
                break;
            case Axis.Vertical:
                Gizmos.DrawLine(new Vector2(-1000, MinY), new Vector2(1000, MinY));
                Gizmos.DrawLine(new Vector2(-1000, MaxY), new Vector2(1000, MaxY));
                break;
            case Axis.Both:
                Gizmos.DrawLine(new Vector2(MinX, -1000), new Vector2(MinX, 1000));
                Gizmos.DrawLine(new Vector2(MaxX, -1000), new Vector2(MaxX, 1000));
                Gizmos.DrawLine(new Vector2(-1000, MinY), new Vector2(1000, MinY));
                Gizmos.DrawLine(new Vector2(-1000, MaxY), new Vector2(1000, MaxY));
                break;
        }
    }
}

public enum Axis
{
    Horizontal,
    Vertical,
    Both
}

public enum Direction
{ 
    Up,
    Down,
    Left,
    Right
}

[System.Serializable]
public class ObjectSpawned
{
    public GameObject Obj;
    public float MinChanceToSpawn;
    public float MaxChanceToSpawn;
}
