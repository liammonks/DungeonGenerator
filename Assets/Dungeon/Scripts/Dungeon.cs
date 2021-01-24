using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private int roomCount = 5;
    [SerializeField] private int seed = 57692450;
    [SerializeField] private DungeonTileset tileset = null;

    private List<DungeonRoom> rooms = new List<DungeonRoom>();
    private DungeonRoom activeRoom;
    private DungeonTiles tiles;

    private void Awake()
    {
        seed = Random.Range(0, int.MaxValue);
        tiles = gameObject.AddComponent<DungeonTiles>();
        OnGenerateNewDungeon();
    }

    private void Update()
    {
        if (activeRoom != null)
        {
            activeRoom.DrawRoom();
        }
    }

    private void OnGenerateNewDungeon()
    {
        seed++;
        if (seed == int.MaxValue)
        {
            seed = 0;
        }
        Generate();
        PlayerManager.Instance.LoadPlayer(activeRoom.RoomBounds[0].GetCenter());
    }

    // Generates a list of rooms based on a seed
    // Activates the first room in the list
    private void Generate()
    {
        if (roomCount < 1) { return; }
        Random.InitState(seed);
        rooms.Clear();
        for (int i = 0; i < roomCount; ++i)
        {
            rooms.Add(new DungeonRoom(Random.Range(0, 4)));
        }
        ActivateRoom(rooms[0]);
    }

    private void ActivateRoom(DungeonRoom toActivate)
    {
        UnloadActiveRoom();
        activeRoom = toActivate;
        // Position camera in center of the room
        //CameraController.Instance.SetPosition(activeRoom.Center);
        tiles.Load(activeRoom, tileset);
    }

    private void UnloadActiveRoom()
    {
        if (activeRoom == null) { return; }
        // Delete previous room
        tiles.Unload();
    }
    
}
