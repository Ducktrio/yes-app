"use client";
import { createContext, useContext, useEffect, useState } from "react";
import api from "@/lib/api";
import { useRouter } from "next/navigation";
import { AuthResponse, LoginPayload, User } from "@/types/User";
import { toast } from "@/lib/toast";
import { Role } from "@/types/Role";
import useFetchRoles from "@/hooks/useFetchRoles";

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
  const roles = useFetchRoles(user?.id!);

  /**
   * check authentication status
   */
  const checkAuth = async () => {
    const token = localStorage.getItem("authToken");
    if (!token) {
      setUser(null);
      setLoading(false);
      return;
    }
    try {
      const res = await api.get<User>("/Users/me");
      setUser(res.data);
    } catch (err) {
      console.error("Auth check failed", err);
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  const login = async (payload: LoginPayload): Promise<void> => {
    await api
      .post<AuthResponse>("Users/login", payload)
      .then(async (res) => {
        localStorage.setItem("authToken", res.data.token);
        await checkAuth();
        return;
      })
      .catch((err) => {
        if (err.response) {
          toast("error", err.response.data.title);
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  };

  const logout = () => {
    localStorage.removeItem("authToken");
    setUser(null);
    router.push("/login");
  };

  useEffect(() => {
    checkAuth();
  }, []);

 
  useEffect(() => {
    setRole(roles.data?.[0] || null);
  }, [roles]);

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
