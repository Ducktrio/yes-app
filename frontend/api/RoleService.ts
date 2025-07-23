import api from "@/lib/api";
import { Role } from "@/types/Role";


export interface GetRoleQuery {
    id?:string;
    userId?: string;
}

export default class RoleService {

    static async getRoles(query?: GetRoleQuery): Promise<Role[]> {
        return await api.get<Role[]>("/Roles", { params: query })
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

    static async getRoleByUser(userId: string) {
        return await api.get<Role[]>(`/Roles?user_id=${userId}`)
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

    static async getRoleById(id: string) {
        return await api.get<Role>(`/Roles?id=${id}`)
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

}