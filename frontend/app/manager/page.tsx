"use client";

import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchServiceTickets from "@/hooks/useFetchServiceTickets";
import useFetchUsers from "@/hooks/useFetchUsers";
import { Card, Spinner } from "flowbite-react";

export default function ManagerPage() {
  const users = useFetchUsers();
  const rooms = useFetchRooms();
  const tickets = useFetchServiceTickets();
  return (
    <>
      <div className="flex flex-col space-y-4 mt-4">
        <div className="flex flex-row items-center space-x-4">
          <Card className="flex flex-col space-y-2 items-center justify-center p-8 bg-blue-100 text-blue-900 text-center">
            <h2 className="text-lg font-semibold">Total Users</h2>
            {users.isLoading && <Spinner />}
            <span className="text-4xl">{users.data?.length}</span>
          </Card>

          <Card className="flex flex-col space-y-2 items-center justify-center p-8 bg-green-100 text-green-900 text-center">
            <h2 className="text-lg font-semibold">Total Rooms</h2>
            {rooms.isLoading && <Spinner />}
            <span className="text-4xl">{rooms.data?.length}</span>
          </Card>

          <Card className="flex flex-col space-y-2 items-center justify-center p-8 bg-yellow-100 text-yellow-900 text-center">
            <h2 className="text-lg font-semibold">Total Tickets</h2>
            {tickets.isLoading && <Spinner />}
            <span className="text-4xl">{tickets.data?.length}</span>
          </Card>
        </div>
      </div>
    </>
  );
}
