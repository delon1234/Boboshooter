using UnityEngine;
using UnityEngine.UI;

public class GenerateLevel : MonoBehaviour
{
    // Serialise the Assets via Unity
    public Sprite TreasureRoom;
    public Sprite BossRoom;
    public Sprite ShopRoom;
    public Sprite UnexploredRoom;
    public Sprite DefaultRoom;
    public Sprite CurrentRoom;

    // icon will specify the type of room
    // location will specify the Cartesian Coordinates of the Map 
    private void DrawIconOnMap(Room room)
    {   
        // create new MapTile (each icon is one MapTile)
        GameObject MapTile = new GameObject("MapTile");

        // Add an Image component and assign the Icon
        Image RoomImage = MapTile.AddComponent<Image>();
        RoomImage.sprite = room.getIcon();

        // Draw the Icon on the specified Location
        RectTransform rectTransform = RoomImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Level.Height, Level.Width) * Level.IconScale;
        rectTransform.position = room.getLocation() * ( (Level.IconScale + Level.Padding)  * Level.Height * Level.Scale);

        // Assign the newly created Icon to Canvas Map
        RoomImage.transform.SetParent(transform, false);
    }
    
    int failsafe = 0;
    private void HandleNestedGeneration(Room room)
    {
        if (failsafe++ > 50) {
            Debug.Log("failsafe hit");
            return;
        }
        DrawIconOnMap(room);
        Level.Rooms.Add(room);

        // 4 directions
        // Left
        if (Random.value < Level.RoomGenerationChance)
        {
            Vector2 pos = room.getLeftLocation();             
            if (!Level.CheckIfRoomExists(pos)) {
                Room newRoom = new Room(DefaultRoom, pos);
                HandleNestedGeneration(newRoom);
            }
        }
        // Right
        if (Random.value < Level.RoomGenerationChance)
        {
            Vector2 pos = room.getRightLocation();             
            if (!Level.CheckIfRoomExists(pos)) {
                Room newRoom = new Room(DefaultRoom, pos);
                HandleNestedGeneration(newRoom);
            }
        }
        // Up
        if (Random.value < Level.RoomGenerationChance)
        {
            Vector2 pos = room.getUpLocation();             
            if (!Level.CheckIfRoomExists(pos)) {
                Room newRoom = new Room(DefaultRoom, pos);
                HandleNestedGeneration(newRoom);
            }
        }
        // Down
        if (Random.value < Level.RoomGenerationChance)
        {
            Vector2 pos = room.getDownLocation();             
            if (!Level.CheckIfRoomExists(pos)) {
                Room newRoom = new Room(DefaultRoom, pos);
                HandleNestedGeneration(newRoom);
            }
        }
    }

    private void Start()
    {
        // Update Global Variables with assigned assets
        Level.TreasureRoomIcon = TreasureRoom;
        Level.BossRoomIcon = BossRoom;
        Level.ShopRoomIcon = ShopRoom;
        Level.UnexploredRoomIcon = UnexploredRoom;
        Level.DefaultRoomIcon = DefaultRoom;
        Level.CurrentRoomIcon = CurrentRoom;

        // Generate the First Room
        Room StartingRoom = new Room(CurrentRoom, new Vector2(0,0));
        HandleNestedGeneration(StartingRoom);
    }


}
