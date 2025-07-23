"use client";
import { useQuery } from "@tanstack/react-query";
import RoleService, { GetRoleQuery } from "@/api/RoleService";

export default function useFetchRoles(query?:GetRoleQuery) {
  return useQuery({
    queryKey: ["roles", query?.userId],
    queryFn: () => RoleService.getRoles(query),
    refetchOnWindowFocus: false,
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}
