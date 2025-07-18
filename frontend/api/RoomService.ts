import api from "@/lib/api";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import { CreateRoomPayload, Room, UpdateRoomPayload } from "@/types/Room";

export interface GetRoomQuery {
    id?: string;
    roomType_id?: string;
    status?: number;
    label?: string;
    roomTicketId?: string;
    serviceTicketId?: string;
}

export default class RoomService {
    static async getRooms(query?: GetRoomQuery) {
        return await api.get<Room[]>("/Rooms", { params: query })
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

    static async createRoom(payload: CreateRoomPayload): Promise<Room> {
        return await api.post<Room>("/Rooms", payload)
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

    static async updateRoom(id: string, payload: Partial<UpdateRoomPayload>): Promise<Room> {
        return await api.put<Room>(`/Rooms/${id}`, payload)
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

    static async deleteRoom(id: string): Promise<void> {
        return await api.delete(`/Rooms/${id}`)
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