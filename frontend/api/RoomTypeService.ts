export interface GetRoomTypeQuery{
    id?: string;
    roomId?: string;
}

import api from "@/lib/api";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import { CreateRoomTypePayload, RoomType, UpdateRoomTypePayload } from "@/types/RoomType";


export default class RoomTypeService {
    static async getRoomTypes(query?: GetRoomTypeQuery): Promise<RoomType[]> {
        return await api.get<RoomType[]>("RoomTypes", { params: query })
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
    static async createRoomType(payload: CreateRoomTypePayload): Promise<RoomType> {
        return await api.post<RoomType>("RoomTypes", payload)
            .then((res) => {
                return res.data;
            })
            .catch((err) => {
                if (err.response.status === 400) { // Validation error
                    // data is arrays of ValidationError
                    const validationErrors: ValidationErrorResponse[] = err.response.data;
                    throw new ValidationError(validationErrors);
                }
                throw err;
            });
    }
    static async updateRoomType(id: string, payload: UpdateRoomTypePayload): Promise<RoomType> {
        return await api.put<RoomType>(`RoomTypes/${id}`, payload)
            .then((res) => {
                return res.data;
            })
            .catch((err) => {
                if (err.response) {
                    throw new Error(err.response.data);
                }
                throw err;
            });
    }
    static async deleteRoomType(id: string): Promise<void> {
        return await api.delete(`RoomTypes/${id}`)
            .then((res) => {
                return res.data;
            })
            .catch((err) => {
                if (err.response) {
                    throw new Error(err.response.data);
                }
                throw err;
            });
    }

   
}