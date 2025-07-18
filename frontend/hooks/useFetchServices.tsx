import ServiceService, { GetServiceQuery } from "@/api/ServiceService";
import { useQuery } from "@tanstack/react-query";

export default function useFetchServices(query?:GetServiceQuery) {
  return useQuery({
    queryKey: ["services", query],
    queryFn: () => ServiceService.getServices(query),
    refetchOnWindowFocus: false,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}