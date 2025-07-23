import api from "@/lib/api";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import {
  AuthResponse,
  CreateUserPayload,
  LoginPayload,
  User,
} from "@/types/User";

export interface GetUserQuery {
  id?: string;
  roleId?: string;
  username?: string;
}

export default class UserService {
  static async getUsers(query?: GetUserQuery): Promise<User[]> {
    return await api
      .get<User[]>("/Users", { params: query })
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async getUserById(id: string) {
    await api
      .get<User>(`/Users?id=${id}`)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async getUserByRole(roleId: string) {
    await api
      .get<User[]>(`/Users?role_id=${roleId}`)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async getUserByUsername(username: string) {
    await api
      .get<User[]>(`/Users?username=${username}`)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async createUser(user: CreateUserPayload) {
    await api
      .post<User>("/Users", user)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response.status === 400) {
          // Validation error
          throw new ValidationError(
            err.response.data as ValidationErrorResponse[],
          );
        }
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async updateUser(id: string, user: Partial<CreateUserPayload>) {
    await api
      .put<User>(`/Users/${id}`, user)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response.status === 400) {
          // Validation error
          throw new ValidationError(err.response.data);
        }
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async deleteUser(id: string) {
    await api
      .delete(`/Users/${id}`)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async loginUser(credential: LoginPayload): Promise<AuthResponse> {
    return await api
      .post<AuthResponse>("/Users/login", credential)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        } else if (err.name === "ERR_NETWORK") {
          throw new Error("Network error. Please check your connection.");
        }
        throw err;
      });
  }
}
