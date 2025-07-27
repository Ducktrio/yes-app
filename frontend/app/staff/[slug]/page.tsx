"use client";

import useFetchCustomers from "@/hooks/useFetchCustomers";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchServiceTickets from "@/hooks/useFetchServiceTickets";
import { Card, Spinner } from "flowbite-react";
import { use } from "react";

export default function StaffTicketPage({
  params,
}: {
  params: Promise<{ slug: string }>;
}) {
  const { slug } = use(params);
  const ticket = useFetchServiceTickets({ id: slug });
  const rooms = useFetchRooms();
  const customers = useFetchCustomers();

  return (
    <>
      <div className="flex flex-row gap-8">
        <div className="flex-2/3 flex flex-col gap-8">
          <Card className="bg-stone-800 flex items-center justify-center text-4xl">
            {ticket.isLoading && <Spinner />}
            {ticket.data?.[0].details}
          </Card>

          <Card>
            <p>Ticket {ticket.data?.[0].id}</p>
            <span className="text-2xl font-bold">
              Room{" "}
              {
                rooms.data?.find((x) => x.id === ticket.data?.[0].room_id)
                  ?.label as string
              }
            </span>
          </Card>
        </div>

        <div className="flex-1/3 flex flex-col gap-8"></div>
      </div>
    </>
  );
}
