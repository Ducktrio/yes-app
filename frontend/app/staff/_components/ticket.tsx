"use client";

import ServiceTicketService from "@/api/ServiceTicketService";
import useFetchCustomers from "@/hooks/useFetchCustomers";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchServices from "@/hooks/useFetchServices";
import useFetchServiceTickets from "@/hooks/useFetchServiceTickets";
import { toast } from "@/lib/toast";
import { timeAgo } from "@/utils/datetime-timeago";
import { useMutation } from "@tanstack/react-query";
import { Alert, Badge, Button, Card, Spinner } from "flowbite-react";
import { useEffect, useState } from "react";
import { BsPencil } from "react-icons/bs";
import { GoIssueClosed, GoIssueOpened, GoIssueTracks } from "react-icons/go";

export default function StaffTicket({ id }: { id: string }) {
  const ticket = useFetchServiceTickets({ id: id });
  const rooms = useFetchRooms();
  const services = useFetchServices();
  const [errors, setErrors] = useState("");

  const markProgress = useMutation({
    mutationFn: ServiceTicketService.take,
    onSuccess: () => {
      toast("success", "You have taken this ticket");
      ticket.refetch();
      rooms.refetch();
    },
    onError: (error) => {
      toast("error", "Failed taking this ticket");
      setErrors(error.message);
    },
  });

  const markResolve = useMutation({
    mutationFn: ServiceTicketService.close,
    onSuccess: () => {
      toast("success", "You have solved this ticket");
      ticket.refetch();
      rooms.refetch();
    },
    onError: (error) => {
      toast("error", "Failed resolving this ticket");
      setErrors(error.message);
    },
  });

  return (
    <>
      <div className="flex flex-row gap-8">
        <div className="flex-2/3 flex flex-col gap-8">
          <Card className="bg-stone-800 flex items-center justify-center text-center text-stone-100 text-4xl ">
            {services.isLoading && <Spinner />}
            {
              services.data?.find((x) => x.id === ticket.data?.[0].service_id)
                ?.name as string
            }
          </Card>

          <Card>
            <p>Ticket {ticket.data?.[0].id}</p>
            <span className="text-2xl font-bold flex items-center gap-4">
              Room{" "}
              {
                rooms.data?.find((x) => x.id === ticket.data?.[0].room_id)
                  ?.label as string
              }
              {ticket.data?.[0].status === 0 && (
                <>
                  <span className="relative flex size-3">
                    <span className="absolute inline-flex h-full w-full animate-ping rounded-full bg-yellow-400 opacity-75"></span>{" "}
                    <span className="relative inline-flex size-3 rounded-full bg-yellow-500"></span>
                  </span>
                </>
              )}
            </span>

            <div className="flex">
              <Badge
                className="rounded-full px-4"
                size="lg"
                icon={
                  ticket.data?.[0].status === 0
                    ? GoIssueOpened
                    : ticket.data?.[0].status === 1
                      ? GoIssueTracks
                      : GoIssueClosed
                }
                color={
                  ticket.data?.[0].status === 0
                    ? "warning"
                    : ticket.data?.[0].status === 1
                      ? "blue"
                      : "success"
                }
              >
                {ticket.data?.[0].status === 0
                  ? "Open"
                  : ticket.data?.[0].status === 1
                    ? "In Progress"
                    : "Closed"}
              </Badge>
            </div>

            <span className="flex items-center">
              <BsPencil className="mr-2 w-5 h-5" />{" "}
              {timeAgo(Number(ticket.data?.[0].created_at))}
            </span>
          </Card>
        </div>

        <div className="flex-1/3 flex flex-col gap-8">
          {errors && <Alert color="red">{errors}</Alert>}
          <p>
            {ticket.isLoading && <Spinner />}
            {ticket.data?.[0].details}
          </p>
          <hr />

          {ticket.data?.[0].status == 0 && (
            <>
              <Button
                onClick={() =>
                  markProgress.mutate(ticket.data?.[0].id as string)
                }
              >
                <GoIssueTracks className="mr-2 w-5 h-5" /> Mark is in progress
              </Button>
            </>
          )}

          {ticket.data?.[0].status == 1 && (
            <>
              <Button
                className="bg-green-600 hover:bg-green-800"
                onClick={() =>
                  markResolve.mutate(ticket.data?.[0].id as string)
                }
              >
                <GoIssueClosed className="mr-2 w-5 h-5" /> Mark as resolved
              </Button>
            </>
          )}

          {ticket.data?.[0].status == 2 && (
            <Alert color="success">
              This ticket has been resolved{" "}
              {timeAgo(Number(ticket.data?.[0].updated_at))}
            </Alert>
          )}
        </div>
      </div>
    </>
  );
}
