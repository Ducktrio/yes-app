export interface RoomType {
    id: string;
    name: string;
    description?: string | null;
    price: number;
}

export interface CreateRoomTypePayload {
    name: string;
    description?: string | null;
    price: number;
}

export interface UpdateRoomTypePayload {
    name?: string | null;
    description?: string | null;
    price?: number | null;
}