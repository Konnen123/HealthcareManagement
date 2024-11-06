using Domain.Utils;

namespace Domain.Errors;

public static class LocationErrors
{
    public static Error NotFoundByRoomAndFloor(int room, int floor) => new Error("Location.NotFoundByRoomAndFloor", $"Location with room {room} and floor {floor} not found.");
    public static Error RoomAlreadyExists(int room, int floor) => new Error("Location.RoomAlreadyExists", $"Location with room {room} and floor {floor} already exists.");
}