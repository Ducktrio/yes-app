import RoomService, { GetRoomQuery } from "@/api/RoomService";
import { useQuery } from "@tanstack/react-query";

export default function useFetchRooms(query?: GetRoomQuery) {
    return useQuery({
        queryKey: ["rooms", query],
        queryFn: () => RoomService.getRooms(query),
        refetchOnWindowFocus: false,
        staleTime: 1000 * 60 * 5, // 5 minutes
    });
    }