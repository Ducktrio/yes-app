"use client";

import UserService, { GetUserQuery } from "@/api/UserService";
import { useQuery } from "@tanstack/react-query";

export default function useFetchUsers(query?: GetUserQuery) {
  
  
  return useQuery({
    queryKey: ["users", query],
    queryFn: () => UserService.getUsers(query),
    refetchOnWindowFocus: false,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}