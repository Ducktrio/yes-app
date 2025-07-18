export interface ServiceTicket {
    id: string;
    customer_id: string;
    room_id: string;
    service_id: string;
    details: string;
    status: number;
}

export interface CreateServiceTicketPayload {
    customer_id: string;
    room_id: string;
    service_id: string;
    details: string;
}

export interface UpdateServiceTicketPayload {
    customer_id?: string | null;
    room_id?: string | null;
    service_id?: string | null;
    details?: string | null;
}