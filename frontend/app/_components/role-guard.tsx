"use client";
import { useAuth } from "@/contexts/AuthContext";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

/**
 * RoleGuard wrapper function. Use it to wrap Node that are role specific
 * e.g.
 * <RoleGuard allowed={['manager']}>
 * <div></div>
 * </RoleGuard>
 */
export function RoleGuard({
  allowed,
  children,
}: {
  allowed: string[];
  children: React.ReactNode;
}) {
  const { user, role, loading } = useAuth();
  const router = useRouter();

  useEffect(() => {
    if (!loading && (!user || !allowed.includes(role?.title as string))) {
      router.push("/unauthorized");
    }
  }, [loading, user]);

  if (loading || !user || !allowed.includes(role?.title as string)) {
    return <p>Loading...</p>;
  }

  return <>{children}</>;
}
