import axios from "axios";

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
});

// Add token to every request if exists
api.interceptors.request.use((config) => {
  if (typeof window !== "undefined") {
    const token = localStorage.getItem("authToken");
    if (token) {
      // @ts-ignore
      config.headers.Authorization = `Bearer ${token}`;
      config.headers["Content-Type"] = "application/json";
    }
  }
  return config;
});

export default api;
