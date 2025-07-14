using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Repositories;

namespace Yes.Services;

public interface IRoomTypeService
{
    Task<List<RoomTypeContract>> Get(string? id = null, string? roomId = null);
    Task<RoomTypeContract?> Create(CreateRoomTypeContract createRoomType);
    Task<RoomTypeContract?> Update(string id, UpdateRoomTypeContract updateRoomType);
    Task<RoomTypeContract?> Delete(string id);
}

public class RoomTypeService(IRoomTypeRepository roomTypeRepository, IMapper mapper) : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository = roomTypeRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<RoomTypeContract>> Get(string? id = null, string? roomId = null)
    {
        return _mapper.Map<List<RoomTypeContract>>(await _roomTypeRepository.Get(id, roomId));
    }

    public async Task<RoomTypeContract?> Create(CreateRoomTypeContract createRoomType)
    {
        var roomType = await _roomTypeRepository.Create(_mapper.Map<RoomType>(createRoomType));
        return _mapper.Map<RoomTypeContract>(roomType);
    }

    public async Task<RoomTypeContract?> Update(string id, UpdateRoomTypeContract updateRoomType)
    {
        var existingRoomType = (await _roomTypeRepository.Get(id, null)).FirstOrDefault();
        if (existingRoomType == null) return null;
        _mapper.Map(updateRoomType, existingRoomType);
        var updatedRoomType = await _roomTypeRepository.Update(existingRoomType);
        return _mapper.Map<RoomTypeContract>(updatedRoomType);
    }

    public async Task<RoomTypeContract?> Delete(string id)
    {
        var deletedRoomType = await _roomTypeRepository.Delete(id);
        return _mapper.Map<RoomTypeContract>(deletedRoomType);
    }
}
