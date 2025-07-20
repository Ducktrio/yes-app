using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Repositories;

namespace Yes.Services;

public interface IRoomService
{
    Task<List<RoomContract>> Get(string? id = null, string? roomTypeId = null, string? label = null, int? status = null, string? roomTicketId = null, string? serviceTicketId = null);
    Task<RoomContract?> Create(CreateRoomContract createRoom);
    Task<RoomContract?> Update(string id, UpdateRoomContract updateRoom);
    Task<RoomContract?> Delete(string id);
}

public class RoomService(IRoomRepository roomRepository, IMapper mapper) : IRoomService
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<RoomContract>> Get(string? id = null, string? roomTypeId = null, string? label = null, int? status = null, string? roomTicketId = null, string? serviceTicketId = null)
    {
        return _mapper.Map<List<RoomContract>>(await _roomRepository.Get(id, roomTypeId, label, status, roomTicketId, serviceTicketId));
    }

    public async Task<RoomContract?> Create(CreateRoomContract createRoom)
    {
        var room = await _roomRepository.Create(_mapper.Map<Room>(createRoom));
        return _mapper.Map<RoomContract>(room);
    }

    public async Task<RoomContract?> Update(string id, UpdateRoomContract updateRoom)
    {
        var existingRoom = (await _roomRepository.Get(id, null, null, null, null, null)).FirstOrDefault();
        if (existingRoom == null) return null;
        _mapper.Map(updateRoom, existingRoom);
        var updatedRoom = await _roomRepository.Update(existingRoom);
        return _mapper.Map<RoomContract>(updatedRoom);
    }

    public async Task<RoomContract?> Delete(string id)
    {
        var deletedRoom = await _roomRepository.Delete(id);
        return _mapper.Map<RoomContract>(deletedRoom);
    }
}
