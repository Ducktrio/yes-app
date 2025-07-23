export interface GetServiceTicketQuery {
  id?: string;
  customerId?: string;
  roomId?: string;
  serviceId?: string;
  status?: number | null;
}

import api from "@/lib/api";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import {
  CreateServiceTicketPayload,
  ServiceTicket,
  UpdateServiceTicketPayload,
} from "@/types/ServiceTicket";

export default class ServiceTicketService {
  static async getServiceTickets(
    query?: GetServiceTicketQuery,
  ): Promise<ServiceTicket[]> {
    return await api
      .get<ServiceTicket[]>("ServiceTickets", { params: query })
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

  static async createServiceTicket(
    payload: CreateServiceTicketPayload,
  ): Promise<ServiceTicket> {
    return await api
      .post<ServiceTicket>("ServiceTickets", payload)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response.status === 400) {
          // Validation error
          const validationErrors: ValidationErrorResponse[] = err.response.data;
          throw new ValidationError(validationErrors);
        }
        throw err;
      });
  }
  static async updateServiceTicket(
    id: string,
    payload: UpdateServiceTicketPayload,
  ): Promise<ServiceTicket> {
    return await api
      .put<ServiceTicket>(`ServiceTickets/${id}`, payload)
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
  static async deleteServiceTicket(id: string): Promise<void> {
    return await api
      .delete(`ServiceTickets/${id}`)
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

  static async take(id: string): Promise<ServiceTicket> {
    return await api
      .post<ServiceTicket>(`ServiceTickets/take/${id}`)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response.status === 403) {
          // Forbidden error
          throw new Error("You are not allowed to take this service ticket.");
        }
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }
  static async close(id: string): Promise<ServiceTicket> {
    return await api
      .post<ServiceTicket>(`ServiceTickets/close/${id}`)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response.status === 403) {
          // Forbidden error
          throw new Error("You are not allowed to close this service ticket.");
        }
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }
}
