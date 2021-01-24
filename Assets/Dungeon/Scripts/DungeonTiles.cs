using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTiles : MonoBehaviour
{
    
    private GameObject tileParent;
    private Dictionary<Vector2, SpriteRenderer> wallTiles;
    private Dictionary<Vector2, SpriteRenderer> floorTiles;
    private DungeonRoom room;
    private DungeonTileset tileset;
    private Vector2Int minBound;
    private Vector2Int maxBound;

    public void Load(DungeonRoom a_room, DungeonTileset a_tileset)
    {
        this.room = a_room;
        this.tileset = a_tileset;
        // Get the bounds of the room of which the tiles must cover
        minBound = room.Min();
        maxBound = room.Max();
        CreateTiles();
        SetSprites();
    }
    
    public void Unload()
    {
        if (tileParent == null) { return; }
        Destroy(tileParent);
        wallTiles.Clear();
    }

    private void CreateTiles()
    {
        tileParent = new GameObject("Walls");
        // Place tiles around room
        wallTiles = new Dictionary<Vector2, SpriteRenderer>();
        floorTiles = new Dictionary<Vector2, SpriteRenderer>();
        // Iterate from left to right
        for (float x = minBound.x; x <= maxBound.x; x += 0.5f)
        {
            // Iterate from bottom to top
            for (float y = minBound.y; y <= maxBound.y; y += 0.5f)
            {
                Vector2 pos = new Vector2(x, y);
                // Check if position is inside, outside or on the edge of a wall
                bool intersects = room.IntersectsRoom(pos);
                bool inside = room.InsideRoom(pos);
                if (!intersects) { continue; } // Continue if outside of room
                // Create default tile
                GameObject tileObj = new GameObject($"Tile ({x}, {y})", typeof(SpriteRenderer));
                tileObj.transform.SetParent(tileParent.transform);
                tileObj.transform.position = pos;
                // WALL TILES
                if (intersects && !inside)
                {
                    wallTiles.Add(pos, tileObj.GetComponent<SpriteRenderer>());
                }
                // FLOOR TILES
                if (inside)
                {
                    floorTiles.Add(pos, tileObj.GetComponent<SpriteRenderer>());
                }
                // If this tile is outside of the room, give it a collider
                //if (!room.IntersectsRoom(pos))
                //{
                //    newTile.AddComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
                //}

            }
        }
    }

    private void SetSprites()
    {
        for (float x = minBound.x; x <= maxBound.x; x += 0.5f)
        {
            for (float y = minBound.y; y <= maxBound.y; y += 0.5f)
            {
                Vector2 pos = new Vector2(x, y);
                // Walls
                if (wallTiles.ContainsKey(pos))
                {
                    uint spriteID = 0;
                    spriteID += floorTiles.ContainsKey(new Vector2(x, y + 0.5f))        ? (uint)1 : 0; // NORTH
                    spriteID += floorTiles.ContainsKey(new Vector2(x + 0.5f, y))        ? (uint)10 : 0; // EAST
                    spriteID += floorTiles.ContainsKey(new Vector2(x, y - 0.5f))        ? (uint)100 : 0; // SOUTH
                    spriteID += floorTiles.ContainsKey(new Vector2(x - 0.5f, y))        ? (uint)1000 : 0; // WEST
                    spriteID += floorTiles.ContainsKey(new Vector2(x + 0.5f, y + 0.5f)) ? (uint)10000 : 0; // NORTH-EAST
                    spriteID += floorTiles.ContainsKey(new Vector2(x - 0.5f, y + 0.5f)) ? (uint)100000 : 0; // NORTH-WEST
                    spriteID += floorTiles.ContainsKey(new Vector2(x + 0.5f, y - 0.5f)) ? (uint)1000000 : 0; // SOUTH-EAST
                    spriteID += floorTiles.ContainsKey(new Vector2(x - 0.5f, y - 0.5f)) ? (uint)10000000 : 0; // SOUTH-WEST

                    wallTiles[pos].sprite = tileset.GetTile(spriteID);
                    //if (wallTiles[pos].sprite != null)
                    //{
                    //    Debug.Log($"Sprite for tile ({pos.x}, {pos.y}) set, N({north}), E({east}), S({south}), W({west})", wallTiles[pos].gameObject);
                    //}
                }
                if (floorTiles.ContainsKey(pos))
                {
                    floorTiles[pos].sprite = tileset.GetFloor();
                }
            }
        }
    }
    
}
