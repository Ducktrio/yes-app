export interface RoomTicket {
  id: string;
  customer_id: string;
  room_id: string;
  check_in_date: string;
  check_out_date: string;
  number_of_occupants: number;
  status: number;
  created_at: string;
}

export interface CreateRoomTicketPayload {
  customer_id: string;
  room_id: string;
  number_of_occupants: number;
}

export interface UpdateRoomTicketPayload {
  customer_id?: string;
  room_id?: string;
  number_of_occupants?: number;
}

