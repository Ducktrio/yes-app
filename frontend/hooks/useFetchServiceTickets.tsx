import ServiceTicketService, { GetServiceTicketQuery } from "@/api/ServiceTicketService";
import { useQuery } from "@tanstack/react-query";

export default function useFetchServiceTickets(query?: GetServiceTicketQuery) {
  return useQuery({
    queryKey: ["serviceTickets", query],
    queryFn: () => ServiceTicketService.getServiceTickets(query),
    refetchOnWindowFocus: false,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}
