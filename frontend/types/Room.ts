export interface Room {
    id: string;
    roomType_id: string;
    label:string;
    status: number;
}

export interface CreateRoomPayload {
    roomType_id: string;
    label: string;
}

export interface UpdateRoomPayload {
    roomType_id?: string;
    label?: string;
    status?: number;
}