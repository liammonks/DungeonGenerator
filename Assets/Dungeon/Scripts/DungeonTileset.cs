using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonTileset", menuName = "ScriptableObjects/DungeonTileset", order = 1)]
public class DungeonTileset : ScriptableObject
{
    public Sprite wall_N;
    public Sprite wall_E;
    public Sprite wall_S;
    public Sprite wall_W;
    public Sprite wall_N_E;
    public Sprite wall_N_W;
    public Sprite wall_E_S;
    public Sprite wall_S_W;
    public Sprite wall_E_W;
    public Sprite wall_N_E_W;
    public Sprite wall_N_E_S;
    public Sprite wall_N_W_S;
    public Sprite wall_S_E_W;

    public Sprite wall_NE;
    public Sprite wall_NW;
    public Sprite wall_SE;
    public Sprite wall_SW;
    public Sprite wall_NE_NW;
    public Sprite wall_SE_SW;

    public Sprite wall_E_NW;
    public Sprite wall_E_SW;
    public Sprite wall_S_NE;
    public Sprite wall_S_NW;
    public Sprite wall_W_NE;
    public Sprite wall_W_SE;
    public Sprite wall_N_NE_NW_SE;
    public Sprite wall_N_NE_NW_SW;
    
    public Sprite[] floorSprites;

    private Dictionary<uint, Sprite> sprites;
    private bool init = false;
    
    // Arrange sprites in dictionary
    private void Init()
    {
        sprites = new Dictionary<uint, Sprite>();
        sprites.Add(00000001, wall_N); // N
        sprites.Add(00010001, wall_N); // N NE
        sprites.Add(00100001, wall_N); // N NW
        sprites.Add(00110001, wall_N); // N NE NW

        sprites.Add(00000010, wall_E); // E
        sprites.Add(00010010, wall_E); // E NE
        sprites.Add(01000010, wall_E); // E SE
        sprites.Add(01010000, wall_E); // NE SE
        sprites.Add(01010010, wall_E); // E NE SW

        sprites.Add(00000100, wall_S); // S
        sprites.Add(01000100, wall_S); // S SE
        sprites.Add(10000100, wall_S); // S SW
        sprites.Add(11000100, wall_S); // S SE SW

        sprites.Add(00000101, wall_S); // S N
        sprites.Add(01000101, wall_S); // S SE N
        sprites.Add(10000101, wall_S); // S SW N
        sprites.Add(11000101, wall_S); // S SE SW N

        sprites.Add(00010101, wall_S); // S N NE
        sprites.Add(01010101, wall_S); // S SE N NE 
        sprites.Add(10010101, wall_S); // S SW N NE 
        sprites.Add(11010101, wall_S); // S SE SW N NE

        sprites.Add(00100101, wall_S); // S N NW
        sprites.Add(01100101, wall_S); // S SE N NW
        sprites.Add(10100101, wall_S); // S SW N NW 
        sprites.Add(11100101, wall_S); // S SE SW N NW

        sprites.Add(00110101, wall_S); // S N NE NW
        sprites.Add(01110101, wall_S); // S SE N NE NW
        sprites.Add(10110101, wall_S); // S SW N NE NW 
        sprites.Add(11110101, wall_S); // S SE SW N NE NW

        sprites.Add(00001000, wall_W); // W
        sprites.Add(00101000, wall_W); // W NW
        sprites.Add(10001000, wall_W); // W SW
        sprites.Add(10100000, wall_W); // NW SW
        sprites.Add(10101000, wall_W); // W NW SW

        sprites.Add(00010000, wall_NE); // NE
        sprites.Add(00100000, wall_NW); // NW
        sprites.Add(01000000, wall_SE); // SE
        sprites.Add(10000000, wall_SW); // SW

        sprites.Add(00000110, wall_E_S); // S E
        sprites.Add(01000110, wall_E_S); // S E SE
        sprites.Add(11010110, wall_E_S); // S E SE NE SW

        sprites.Add(00001100, wall_S_W); // S W
        sprites.Add(10001100, wall_S_W); // S W SW
        sprites.Add(11101100, wall_S_W); // S W SW NW SE

        sprites.Add(11110111, wall_N_E_S); // S SE SW N NE NW E
        sprites.Add(11111101, wall_N_W_S); // S SE SW N NE NW W

        sprites.Add(01110001, wall_N_NE_NW_SE); // N NE NW SE
        sprites.Add(10110001, wall_N_NE_NW_SW); // N NE NW SW

        sprites.Add(00001010, wall_E_W); // E W
        sprites.Add(00111010, wall_E_W); // E W NE NW
        sprites.Add(11001010, wall_E_W); // E W SE SW
        sprites.Add(11011010, wall_E_W); // E W NE SE SW
        sprites.Add(11101010, wall_E_W); // E W NW SE SW
        sprites.Add(01111010, wall_E_W); // E W NE NW SE
        sprites.Add(10111010, wall_E_W); // E W NE NW SW
        sprites.Add(11111010, wall_E_W); // E W NE NW SE SW

        sprites.Add(11111011, wall_N_E_W);  // N E W NE NW SE SW
        sprites.Add(11111110, wall_S_E_W);  // S E W NE NW SE SW

        sprites.Add(00110000, wall_NE_NW);  // NE NW
        sprites.Add(11000000, wall_SE_SW);  // SE SW

        sprites.Add(01110011, wall_N_E);  // N E NE NW SE
        sprites.Add(10111001, wall_N_W);  // N W NE NW SW

        sprites.Add(01110010, wall_E_NW);  // E NE SE NW
        sprites.Add(11010010, wall_E_SW);  // E NE SE SW
        sprites.Add(11010100, wall_S_NE);  // S SE SW NE
        sprites.Add(11100100, wall_S_NW);  // S SE SW NW
        sprites.Add(10111000, wall_W_NE);  // W NW SW NE
        sprites.Add(11101000, wall_W_SE);  // W NW SW SE
    }

    public Sprite GetTile(uint spriteID)
    {
        // Initialise if not done already
        if (!init) { Init(); }
        // Return sprite based on ID
        if (!sprites.ContainsKey(spriteID))
        {
            Debug.LogError("Sprite not found for ID " + spriteID);
            return null;
        }
        return sprites[spriteID];
    }

    public Sprite GetFloor()
    {
        return floorSprites[Random.Range(0, floorSprites.Length)];
    }
}