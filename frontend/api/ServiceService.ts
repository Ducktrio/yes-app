export interface GetServiceQuery{
    id?: string;
    serviceTicketId?: string;
}

import api from "@/lib/api";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import { CreateServicePayload, Service, UpdateServicePayload } from "@/types/Service";

export default class ServiceService {
    static async getServices(query?: GetServiceQuery): Promise<Service[]> {
        return await api.get<Service[]>("Services", { params: query })
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

    static async createService(payload: CreateServicePayload): Promise<Service> {
        return await api.post<Service>("Services", payload)
            .then((res) => {
                return res.data;
            })
            .catch((err) => {
                if (err.response.status === 400) { // Validation error
                    const validationErrors: ValidationErrorResponse[] = err.response.data;
                    throw new ValidationError(validationErrors);
                }
                throw err;
            });
    }

    static async updateService(id: string, payload: UpdateServicePayload): Promise<Service> {
        return await api.put<Service>(`Services/${id}`, payload)
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

    static async deleteService(id: string): Promise<void> {
        return await api.delete(`Services/${id}`)
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