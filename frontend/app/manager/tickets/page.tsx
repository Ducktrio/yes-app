"use client";
import {
  Badge,
  Drawer,
  DrawerHeader,
  DrawerItems,
  TabItem,
  Tabs,
} from "flowbite-react";
import CreateTicket from "@/app/_components/create-ticket";
import TicketCard from "@/app/_components/TicketCard";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchServices from "@/hooks/useFetchServices";
import useFetchServiceTickets from "@/hooks/useFetchServiceTickets";
import { Spinner, Button, Modal, ModalBody, ModalHeader } from "flowbite-react";
import { useState } from "react";
import { GoIssueClosed, GoIssueOpened, GoIssueTracks } from "react-icons/go";
import { ServiceTicket } from "@/types/ServiceTicket";
import { Room } from "@/types/Room";
import { Service } from "@/types/Service";
import ManagerDetailTicket from "../_components/detail-ticket";

export default function ManagerTicketsPage() {
  const [createModal, setCreateModal] = useState(false);
  const [detailDrawer, setDetailDrawer] = useState<{
    ticket: ServiceTicket;
    room: Room;
    service: Service;
  } | null>(null);
  const [filterStatus, setFilterStatus] = useState<number | null>(null);
  const tickets = useFetchServiceTickets({
    status: filterStatus,
  });
  const rooms = useFetchRooms();
  const services = useFetchServices();

  const rawTickets = useFetchServiceTickets();

  const handleCreation = () => {
    setCreateModal(false);
    tickets.refetch();
    rawTickets.refetch();
  };

  return (
    <>
      <h1 className="text-4xl font-bold mb-12">Manage Tickets</h1>

      <div className="flex justify-between mb-4">
        <Tabs
          variant="underline"
          onActiveTabChange={(tab) => setFilterStatus(tab)}
        >
          <TabItem
            title={
              <>
                Open
                <Badge color="light" className="ml-2">
                  {
                    rawTickets.data?.filter((ticket) => ticket.status == 0)
                      .length
                  }
                </Badge>
              </>
            }
            icon={GoIssueOpened}
          />
          <TabItem
            title={
              <>
                In Progress
                <Badge color="light" className="ml-2">
                  {
                    rawTickets.data?.filter((ticket) => ticket.status == 1)
                      .length
                  }
                </Badge>
              </>
            }
            icon={GoIssueTracks}
          />
          <TabItem
            title={
              <>
                Closed
                <Badge color="light" className="ml-2">
                  {
                    rawTickets.data?.filter((ticket) => ticket.status == 2)
                      .length
                  }
                </Badge>
              </>
            }
            icon={GoIssueClosed}
          />
        </Tabs>

        <Button
          className="bg-green-600 hover:shadow-green-200 hover:shadow hover:bg-green-800"
          onClick={() => setCreateModal(true)}
        >
          <GoIssueOpened className="inline mr-2 w-5 h-5" /> Issue new ticket
        </Button>
      </div>

      <hr className="mt-4 mb-4" />

      <div className="flex flex-wrap space-x-4 mt-4">
        {tickets.isLoading && (
          <Spinner className="mx-auto" size="xl" aria-label="Loading tickets" />
        )}
        {!tickets.data && <>No tickets.</>}

        {tickets.data?.map((ticket) => (
          <TicketCard
            key={ticket.id}
            ticket={ticket}
            service={
              services.data?.find(
                (service) => service.id === ticket.service_id,
              ) as Service
            }
            room={
              rooms.data?.find((room) => room.id === ticket.room_id) as Room
            }
            onUpdate={() => {
              // Refetch tickets when a status change occurs
              tickets.refetch();
              rawTickets.refetch();
            }}
            onClick={() => {
              setDetailDrawer({
                ticket,
                room: rooms.data?.find(
                  (room) => room.id === ticket.room_id,
                ) as Room,
                service: services.data?.find(
                  (service) => service.id === ticket.service_id,
                ) as Service,
              });
            }}
          />
        ))}
      </div>

      <Modal
        dismissible
        show={createModal}
        onClose={() => setCreateModal(false)}
      >
        <ModalHeader />
        <ModalBody>
          <CreateTicket onSuccess={handleCreation} />
        </ModalBody>
      </Modal>

      <Drawer
        backdrop={true}
        open={detailDrawer !== null}
        onClose={() => setDetailDrawer(null)}
        position="right"
        className="p-8 w-[25%]"
      >
        <DrawerHeader />
        <DrawerItems>
          <ManagerDetailTicket
            ticket={detailDrawer?.ticket as ServiceTicket}
            room={detailDrawer?.room as Room}
            service={detailDrawer?.service as Service}
            onChange={() => {
              tickets.refetch();
              setDetailDrawer(null);
            }}
          />
        </DrawerItems>
      </Drawer>
    </>
  );
}
