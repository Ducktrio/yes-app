export interface Service {
    id: string;
    name: string;
    price: number;
}

export interface CreateServicePayload {
    name: string;
    price: number;
}

export interface UpdateServicePayload {
    name?: string;
    price?: number;
}