import axios from "axios";

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
});

// Add token to every request if exists
api.interceptors.request.use((config: any) => {
  if (typeof window !== "undefined") {
    const token = localStorage.getItem("authToken");
    if (token) {
      /** @ts-expect-error
       * @ts-ignore
       * problem with axios types, but this works just file per documentation */
      config.headers.Authorization = `Bearer ${token}`;
      /**
       * @ts-expect-error
       * @ts-ignore
       * problem with axios types, but this works just file per documentation */
      config.headers["Content-Type"] = "application/json";
    }
  }
  return config;
});

export default api;
