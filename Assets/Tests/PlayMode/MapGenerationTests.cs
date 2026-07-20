using NUnit.Framework;
using UnityEngine;

public class MapGenerationTests
{
    private MapGenerationState state;

    [SetUp]
    public void SetUp()
    {
        state = new MapGenerationState();
    }

    // Nothing needs to be done after each run because SetUp creates a new state everytime anyways
    [TearDown]
    public void TearDown()
    {
    }

    // Ensures that initial number of Rooms is 0
    [Test]
    public void State_InitialState()
    {
        Assert.AreEqual(0, state.NumberOfRooms);
    }

    // Ensures CreateRoom also increments number of room by 1
    [Test]
    public void CreateRoom_IncreasesRoomCount()
    {
        state.CreateRoom(Vector2.zero, 0);
        Assert.AreEqual(1, state.NumberOfRooms);
    }

    // Logically check GetFurthestAvailableRoom - Basic check for distance
    [Test]
    public void GetFurthestAvailableRoom_RetrievesFurthestDistance()
    {
        Room room1 = state.CreateRoom(new Vector2(0, 0), 0);
        Room room2 = state.CreateRoom(new Vector2(0, 1), 1);
        Room room3 = state.CreateRoom(new Vector2(0, 2), 2);
        Room room4 = state.CreateRoom(new Vector2(0, 3), 3);
        Room room5 = state.CreateRoom(new Vector2(0, 4), 4);

        Room result = state.GetFurthestAvailableRoom();
        Assert.AreEqual(room5, result);
    }

    // Logically check GetFurthestAvailableRoom - Branching
    [Test]
    public void GetFurthestAvailableRoom_SkipsSpecialRoomsBranched()
    {
        Room room1 = state.CreateRoom(new Vector2(0, 0), 0);
        Room room2 = state.CreateRoom(new Vector2(0, 1), 1);
        Room room3 = state.CreateRoom(new Vector2(1, 0), 1);

        // Sets the top room to special, then it should retrieve the other same distance normal room
        room2.SetType(RoomType.Boss);
        Room result = state.GetFurthestAvailableRoom();
        Assert.AreEqual(room3, result);
    }

    // Logically check GetFurthestAvailableRoom - Linear
    [Test]
    public void GetFurthestAvailableRoom_SkipsSpecialRoomsLinear()
    {
        Room room1 = state.CreateRoom(new Vector2(0, 0), 0);
        Room room2 = state.CreateRoom(new Vector2(0, 1), 1);
        Room room3 = state.CreateRoom(new Vector2(0, 2), 2);
        Room room4 = state.CreateRoom(new Vector2(0, 3), 3);
        Room room5 = state.CreateRoom(new Vector2(0, 4), 4);

        // If Furthest is now Boss room, then it should retrieve next furthest room
        room5.SetType(RoomType.Boss);
        Room result = state.GetFurthestAvailableRoom();
        Assert.AreEqual(room4, result);

        // If again, Distance 4 and 3 is now Special, it should find Distance 2 Normal
        room4.SetType(RoomType.Shop);
        result = state.GetFurthestAvailableRoom();
        Assert.AreEqual(room3, result);

        // If Distance 1 is Special, irrelevant, should still find Distance 2 Normal
        room2.SetType(RoomType.Shop);
        result = state.GetFurthestAvailableRoom();
        Assert.AreEqual(room3, result);
    }
}