"use client";
56;

import {
  Button,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeadCell,
  TableRow,
  Modal,
  ModalHeader,
} from "flowbite-react";
import { Tabs, TabItem, Badge } from "flowbite-react";
import { useCallback, useState } from "react";
import { BsDoorClosedFill, BsDoorOpenFill, BsXSquare } from "react-icons/bs";
import useFetchServiceTickets from "@/hooks/useFetchServiceTickets";
import useFetchServices from "@/hooks/useFetchServices";
import useFetchRooms from "@/hooks/useFetchRooms";
import { HiCash, HiCheck, HiExclamationCircle } from "react-icons/hi";
import { timeAgo } from "@/utils/datetime-timeago";
import { GoIssueClosed, GoIssueOpened, GoIssueTracks } from "react-icons/go";
import CreateTicket from "@/app/_components/create-ticket";

export default function ReceptionTicketsPage() {
  const [filterStatus, setFilterStatus] = useState(0);
  const [createModal, setCreateModal] = useState(false);
  const rawTickets = useFetchServiceTickets();
  const tickets = useFetchServiceTickets({ status: filterStatus });
  const rooms = useFetchRooms();
  const services = useFetchServices();

  const handleCreated = useCallback(() => {
    setCreateModal(false);
    rawTickets.refetch();
    tickets.refetch();
  }, []);

  return (
    <>
      <h1 className="text-4xl">Tickets</h1>
      <Tabs variant="underline" onActiveTabChange={(x) => setFilterStatus(x)}>
        <TabItem
          title={
            <>
              Open
              <Badge className="ml-2" color="light">
                {rawTickets.data?.filter((raw) => raw.status === 0).length}
              </Badge>
            </>
          }
          icon={GoIssueOpened}
        />
        <TabItem
          title={
            <>
              In Progress
              <Badge className="ml-2" color="light">
                {rawTickets.data?.filter((raw) => raw.status === 1).length}
              </Badge>
            </>
          }
          icon={GoIssueTracks}
        />
        <TabItem
          title={
            <>
              Resolved
              <Badge className="ml-2" color="light">
                {rawTickets.data?.filter((raw) => raw.status === 2).length}
              </Badge>
            </>
          }
          icon={GoIssueClosed}
        />
      </Tabs>

      <div className="grid grid-cols-3 gap-8">
        <div className="col-span-2 flex flex-col gap-4 bg-gray-100 rounded-lg">
          <Table className="overflow-y-scroll max-h-[40vh]">
            <TableHead>
              <TableRow>
                <TableHeadCell>Ticket ID</TableHeadCell>
                <TableHeadCell>Service</TableHeadCell>
                <TableHeadCell>Room</TableHeadCell>
                <TableHeadCell>Status</TableHeadCell>
                <TableHeadCell>Created At</TableHeadCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {tickets.data?.map((ticket) => (
                <TableRow key={ticket.id}>
                  <TableCell>{ticket.id}</TableCell>
                  <TableCell>
                    {
                      services.data?.find((x) => x.id == ticket.service_id)
                        ?.name
                    }
                  </TableCell>
                  <TableCell>
                    {rooms.data?.find((x) => x.id == ticket.room_id)?.label}
                  </TableCell>
                  <TableCell className="flex flex-wrap">
                    {ticket?.status === 0 && (
                      <>
                        <Badge
                          icon={GoIssueOpened}
                          color="yellow"
                          className="mt-2 animate-pulse"
                        >
                          Open
                        </Badge>
                      </>
                    )}

                    {ticket?.status === 1 && (
                      <Badge icon={GoIssueTracks} color="blue" className="mt-2">
                        In Progress
                      </Badge>
                    )}

                    {ticket?.status == 2 && (
                      <Badge
                        icon={GoIssueClosed}
                        color="success"
                        className="mt-2"
                      >
                        Resolved
                      </Badge>
                    )}
                  </TableCell>

                  <TableCell>{timeAgo(Number(ticket.created_at))}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>

        <div className="flex flex-col gap-4">
          <h4 className="text-lg text-gray-800">Issue new service ticket</h4>
          <Button
            className="w-full bg-green-600 hover:bg-green-800 hover:shadow"
            onClick={() => setCreateModal(true)}
          >
            <GoIssueOpened className="inline mr-2 w-5 h-5" />
            Issue New Ticket
          </Button>
        </div>
      </div>

      <Modal
        show={createModal}
        onClose={() => setCreateModal(false)}
        dismissible
      >
        <ModalHeader title="Issue new ticket"></ModalHeader>
        <CreateTicket onSuccess={handleCreated} />
      </Modal>
    </>
  );
}
