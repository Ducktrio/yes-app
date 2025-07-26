"use client";

import RoomTicketService from "@/api/RoomTicketService";
import CreateTicket from "@/app/_components/create-ticket";
import useFetchCustomers from "@/hooks/useFetchCustomers";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchRoomTickets from "@/hooks/useFetchRoomTickets";
import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import { toast } from "@/lib/toast";
import { convertUnixToDate } from "@/utils/datatime-unix";
import { timeAgo } from "@/utils/datetime-timeago";
import { useMutation } from "@tanstack/react-query";
import {
  Badge,
  Button,
  Card,
  Modal,
  ModalBody,
  ModalHeader,
} from "flowbite-react";
import { useRouter } from "next/navigation";
import { use, useState } from "react";
import {
  BsDoorClosedFill,
  BsDoorOpenFill,
  BsPeopleFill,
  BsPerson,
  BsPersonFill,
} from "react-icons/bs";
import { GoGitCommit, GoIssueOpened } from "react-icons/go";

export default function CheckPage({
  params,
}: {
  params: Promise<{ slug: string }>;
}) {
  const [ticketModal, setTicketModal] = useState(false);
  const { slug } = use(params);
  const check = useFetchRoomTickets({ id: slug });
  const room = useFetchRooms({ roomTicketId: slug });
  const customer = useFetchCustomers({ roomTicketId: slug });
  const roomType = useFetchRoomTypes({ roomId: room.data?.[0].id });
  const router = useRouter();

  const voidTicket = useMutation({
    mutationFn: RoomTicketService.deleteRoomTicket,
    onSuccess: () => {
      toast("success", "Ticket has been voided");

      router.push("/reception/checks");
    },
  });

  const checkin = useMutation({
    mutationFn: RoomTicketService.checkin,
    onSuccess: () => {
      toast("success", "Checked in successfully");
      check.refetch();
      room.refetch();
    },
  });

  const checkout = useMutation({
    mutationFn: RoomTicketService.checkout,
    onSuccess: () => {
      toast("success", "Checked out successfully");
      check.refetch();
      room.refetch();
    },
  });

  return (
    <>
      <h1 className="text-4xl">Check #{slug}</h1>

      <hr />

      <div className="flex flex-row gap-8">
        <div className="flex-2/3 flex flex-col gap-8">
          <Card className="bg-stone-600 p-8 text-4xl text-stone-100 flex items-center justify-center">
            {room.data?.[0].label}
          </Card>
          <Card className="p-8 ">
            <div className="flex items-center justify-center gap-8 text-2xl">
              <BsPerson />{" "}
              <span>
                {customer.data?.[0].courtesy_title}{" "}
                {customer.data?.[0].full_name}
              </span>
            </div>
          </Card>

          <div className="flex-1 flex flex-row gap-8">
            <Card className="p-8 flex-1 flex items-center justify-center text-center gap-8">
              <span className="text-lg">Occupants</span>

              <span className="text-4xl">
                {check.data?.[0].number_of_occupants}
              </span>
            </Card>

            <Card className="p-8 flex-1 flex items-center justify-center text-center gap-8">
              <span className="text-lg">Status</span>

              <span className="text-4xl">
                <Badge
                  className="rounded-full text-lg animate-pulse py-2 px-4"
                  color={
                    check.data?.[0].check_in_date == null
                      ? "warning"
                      : check.data?.[0].check_out_date == null
                        ? "success"
                        : "gray"
                  }
                >
                  {check.data?.[0].check_in_date == null
                    ? "Pending Checkin"
                    : check.data?.[0].check_out_date == null
                      ? "Checked In"
                      : "Checked Out"}
                </Badge>
              </span>
            </Card>
          </div>
        </div>

        <div className="flex-1/3 flex flex-col gap-8">
          {check.data?.[0].check_in_date == null && (
            <div className="p-8 flex flex-col justify-between flex-1 bg-stone-100 border border-stone-200 text-stone-600 rounded-lg">
              <div className="text-center text-xl">Receipt</div>
              <div className="block">
                <span className="text-sm text-stone-400 text-center">
                  {check.data &&
                    convertUnixToDate(
                      check.data?.[0].created_at as string,
                    ).toString()}
                </span>

                <div>
                  For: {customer.data?.[0].courtesy_title}{" "}
                  {customer.data?.[0].full_name}
                </div>
              </div>

              <hr />

              <div className="flex flex-row gap-8 items-center justify-between text-xl">
                <div className="flex items-center gap-4 ">
                  <BsDoorClosedFill /> {roomType.data?.[0].name} |{" "}
                  {(check.data?.[0].number_of_occupants as number) > 1 ? (
                    <BsPeopleFill />
                  ) : (
                    <BsPersonFill />
                  )}{" "}
                  {check.data?.[0].number_of_occupants}
                </div>
                <div>
                  Rp.{" "}
                  {Intl.NumberFormat("id").format(
                    roomType.data?.[0].price as number,
                  )}
                </div>
              </div>
              <hr />

              <span className="text-2xl">
                Subtotal: Rp{" "}
                {Intl.NumberFormat("id").format(
                  roomType.data?.[0].price as number,
                )}
              </span>

              <div className="flex flex-row items-center justify-evenly">
                <button
                  className="px-4 py-2 border border-red-600 text-red-600 rounded-lg hover:bg-red-600 hover:text-white"
                  onClick={() => voidTicket.mutate(slug)}
                >
                  Void Ticket
                </button>

                <button
                  className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-800 flex items-center"
                  onClick={() => {
                    checkin.mutate(slug);
                  }}
                >
                  <GoGitCommit className="mr-2 w-5 h-5" /> Check In
                </button>
              </div>
            </div>
          )}

          {check.data?.[0].check_in_date != null &&
            check.data?.[0].check_out_date == null && (
              <>
                <div className="flex flex-col gap-8 items-stretch">
                  <p className="text-lg">
                    Customer's checked in{" "}
                    {timeAgo(Number(check.data?.[0].check_in_date))}.
                  </p>

                  <hr />

                  <p>Post ticket for customer's service request</p>
                  <Button
                    className="bg-green-600"
                    onClick={() => setTicketModal(true)}
                  >
                    <GoIssueOpened className="mr-2 w-5 h-5" /> Issue Service
                    Ticket
                  </Button>

                  <hr />

                  <p>Checkout Quick Button</p>
                  <Button color="default" onClick={() => checkout.mutate(slug)}>
                    <BsDoorOpenFill className="mr-2 w-5 h-5" /> Check out Ticket
                  </Button>
                </div>
              </>
            )}
        </div>
      </div>

      <Modal show={ticketModal} onClose={() => setTicketModal(false)}>
        <ModalHeader />
        <ModalBody>
          <CreateTicket
            onSuccess={() => setTicketModal(false)}
            fill={{
              customer_id: customer.data?.[0].id as string,
              room_id: room.data?.[0].id as string,
            }}
          />
        </ModalBody>
      </Modal>
    </>
  );
}
