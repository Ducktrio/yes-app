import RoomTypeService, { GetRoomTypeQuery } from "@/api/RoomTypeService";
import { useQuery } from "@tanstack/react-query";

export default function useFetchRoomTypes(query?: GetRoomTypeQuery) {
  return useQuery({
    queryKey: ["roomTypes", query],
    queryFn: () => RoomTypeService.getRoomTypes(query),
    refetchOnWindowFocus: false,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}