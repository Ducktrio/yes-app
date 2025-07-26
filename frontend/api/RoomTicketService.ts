import api from "@/lib/api";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import {
  CreateRoomTicketPayload,
  RoomTicket,
  UpdateRoomTicketPayload,
} from "@/types/RoomTicket";

export interface GetRoomTicketQuery {
  id?: string;
  customerId?: string;
  roomId?: string;
  status?: number;
  checkedIn?: boolean;
}

export default class RoomTicketService {
  static async getRoomTickets(
    query?: GetRoomTicketQuery,
  ): Promise<RoomTicket[]> {
    return await api
      .get<RoomTicket[]>("/RoomTickets", { params: query })
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

  static async createRoomTicket(payload: CreateRoomTicketPayload) {
    return await api
      .post<RoomTicket>("/RoomTickets", payload)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response.status === 400) {
          // Validation error
          // data is arrays of ValidationError
          const validationErrors: ValidationErrorResponse[] = err.response.data;
          throw new ValidationError(validationErrors);
        }
        throw err;
      });
  }

  static async updateRoomTicket(
    id: string,
    payload: Partial<UpdateRoomTicketPayload>,
  ) {
    return await api
      .put<RoomTicket>(`/RoomTickets/${id}`, payload)
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

  static async deleteRoomTicket(id: string) {
    return await api
      .delete(`/RoomTickets/${id}`)
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

  static async checkin(id: string) {
    return await api
      .put("/RoomTickets/checkin/" + id)
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

  static async checkout(id: string) {
    return await api
      .put("/RoomTickets/checkout/" + id)
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

