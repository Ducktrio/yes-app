"use client";
import { useQuery } from "@tanstack/react-query";
import CustomerService, { GetCustomerQuery } from "@/api/CustomerService";

export default function useFetchCustomers(query?: GetCustomerQuery) {
  return useQuery({
    queryKey: ["customers", query],
    queryFn: () => CustomerService.getCustomers(query),
    refetchOnWindowFocus: false,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}

