export interface User {
  id: string;
  role_id: string;
  username: string;
  description: string;
}

export interface LoginPayload {
  username: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface CreateUserPayload {
  role_id: string;
  username: string;
  password: string;
  description: string;
}