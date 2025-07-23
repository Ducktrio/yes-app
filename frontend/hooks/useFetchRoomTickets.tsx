import RoomTicketService, { GetRoomTicketQuery } from "@/api/RoomTicketService";
import { useQuery } from "@tanstack/react-query";

export default function useFetchRoomTickets(query?: GetRoomTicketQuery) {
  return useQuery({
    queryKey: ["roomTickets", query],
    queryFn: () => RoomTicketService.getRoomTickets(query),
    refetchOnWindowFocus: false,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}