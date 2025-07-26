"use client";
import { createContext, useContext, useEffect, useState } from "react";
import api from "@/lib/api";
import { useRouter } from "next/navigation";
import { AuthResponse, LoginPayload, User } from "@/types/User";
import { toast } from "@/lib/toast";
import { Role } from "@/types/Role";
import useFetchRoles from "@/hooks/useFetchRoles";
import UserService from "@/api/UserService";
import { useMutation, useQuery } from "@tanstack/react-query";

interface AuthContextType {
  user: User | null;
  role: Role | null;
  loading: boolean;
  login: (payload: LoginPayload) => Promise<void>;
  logout: () => void;
  checkAuth: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [role, setRole] = useState<Role | null>(null);
  const [loading, setLoading] = useState(true);
  const router = useRouter();
  const roles = useFetchRoles({
    userId: user?.id || "",
  });

  const loginQuery = useMutation({
    mutationKey: ["login"],
    mutationFn: UserService.loginUser,
    onSettled: async (data) => {
      localStorage.setItem("authToken", data?.token as string);
      await checkAuth();
      switch (role?.title) {
        case "Manager":
          router.push("/manager");
          break;
        case "Receptionist":
          router.push("/reception");
          break;
        case "Staff":
          router.push("/staff");
          break;
      }
    },
    onError: (error) => {
      toast("error", error.message);
    },
  });

  /**
   * check authentication status
   */
  const checkAuth = async () => {
    const token = localStorage.getItem("authToken");
    if (!token) {
      setUser(null);
      setRole(null);
      setLoading(false);
      return;
    }
    try {
      const res = await api.get<User>("/Users/me");
      setUser(res.data);
      setRole(roles.data?.find((role) => role.id === res.data.role_id) || null);
    } catch (err: any) {
      if (err.response && err.response.status === 401) {
        // Bad Request, or token may expired
        localStorage.removeItem("authToken");
        toast("error", "Session expired. Please login again.");
        router.push("/login");
        setUser(null);
        setRole(null);

        return;
      }
      console.error("Auth check failed", err);
      setUser(null);
      setRole(null);
    } finally {
      setLoading(false);
    }
  };

  const login = async (payload: LoginPayload): Promise<void> => {
    loginQuery.mutate(payload);
  };

  const logout = () => {
    localStorage.removeItem("authToken");
    setUser(null);
    setRole(null);
    toast("success", "Logged out successfully");
    router.push("/login");
  };

  useEffect(() => {
    checkAuth();
  }, []);

  useEffect(() => {
    setLoading(loginQuery.isPending);
  }, [loginQuery.isPending]);

  useEffect(() => {
    setRole(roles.data?.[0] || null);
  }, [roles, user]);

  const value: AuthContextType = {
    user,
    role,
    loading,
    login,
    logout,
    checkAuth,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => useContext(AuthContext);
