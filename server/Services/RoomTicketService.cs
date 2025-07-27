using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Repositories;

namespace Yes.Services;

public interface IRoomTicketService
{
    Task<List<RoomTicketContract>> Get(string? id = null, string? customerId = null, string? roomId = null, int? status = null, bool? checkedIn = null);
    Task<RoomTicketContract?> Create(CreateRoomTicketContract createRoomTicket);
    Task<RoomTicketContract?> Update(string id, UpdateRoomTicketContract updateRoomTicket);
    Task<RoomTicketContract?> Delete(string id);
    Task<RoomTicketContract?> CheckIn(string id);
    Task<RoomTicketContract?> CheckOut(string id);
}

public class RoomTicketService(IRoomTicketRepository roomTicketRepository, IServiceTicketRepository serviceTicketRepository, IRoomRepository roomRepository, IMapper mapper) : IRoomTicketService
{
    private readonly IRoomTicketRepository _roomTicketRepository = roomTicketRepository;
    private readonly IServiceTicketRepository _serviceTicketRepository = serviceTicketRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<RoomTicketContract>> Get(string? id = null, string? customerId = null, string? roomId = null, int? status = null, bool? checkedIn = null)
    {
        return _mapper.Map<List<RoomTicketContract>>(await _roomTicketRepository.Get(id, customerId, roomId, status, checkedIn));
    }

    public async Task<RoomTicketContract?> Create(CreateRoomTicketContract createRoomTicket)
    {
        var roomTicket = await _roomTicketRepository.Create(_mapper.Map<RoomTicket>(createRoomTicket));
        return _mapper.Map<RoomTicketContract>(roomTicket);
    }

    public async Task<RoomTicketContract?> Update(string id, UpdateRoomTicketContract updateRoomTicket)
    {
        var existingRoomTicket = (await _roomTicketRepository.Get(id, null, null, null, null)).FirstOrDefault();
        if (existingRoomTicket == null) return null;
        _mapper.Map(updateRoomTicket, existingRoomTicket);
        var updatedRoomTicket = await _roomTicketRepository.Update(existingRoomTicket);
        return _mapper.Map<RoomTicketContract>(updatedRoomTicket);
    }

    public async Task<RoomTicketContract?> Delete(string id)
    {
        var deletedRoomTicket = await _roomTicketRepository.Delete(id);
        return _mapper.Map<RoomTicketContract>(deletedRoomTicket);
    }

    public async Task<RoomTicketContract?> CheckIn(string id)
    {
        var roomTicket = (await _roomTicketRepository.Get(id, null, null, null, null)).FirstOrDefault();
        if (roomTicket == null) return null;
        roomTicket.CheckInDate = DateTime.UtcNow;
        var updatedRoomTicket = await _roomTicketRepository.Update(roomTicket);

        var room = (await _roomRepository.Get(roomTicket.Room_id, null, null, null, null, null)).FirstOrDefault();
        if (room == null) return null;
        room.Status = 1;
        await _roomRepository.Update(room);
        return _mapper.Map<RoomTicketContract>(updatedRoomTicket);
    }

    public async Task<RoomTicketContract?> CheckOut(string id)
    {
        var roomTicket = (await _roomTicketRepository.Get(id, null, null, null, null)).FirstOrDefault();
        if (roomTicket == null) return null;
        roomTicket.CheckOutDate = DateTime.UtcNow;
        roomTicket.Status = 1;
        var updatedRoomTicket = await _roomTicketRepository.Update(roomTicket);

        var room = (await _roomRepository.Get(roomTicket.Room_id, null, null, null, null, null)).FirstOrDefault();
        if (room == null) return null;
        room.Status = 2;
        await _roomRepository.Update(room);

        ServiceTicket serviceTicket = new()
        {
            Customer_id = roomTicket.Customer_id,
            Room_id = roomTicket.Room_id,
            Service_id = "SVC_001",
            Details = "Room checkout cleaning service"
        };
        await _serviceTicketRepository.Create(serviceTicket);
        return _mapper.Map<RoomTicketContract>(updatedRoomTicket);
    }
}
