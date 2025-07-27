"use client";
import useFetchServiceTickets from "@/hooks/useFetchServiceTickets";
import StaffSidebar from "./_components/sidebar";
import { useState, Suspense } from "react";
import { Modal, ModalBody, ModalHeader, Spinner } from "flowbite-react";
import TicketCard from "../_components/TicketCard";
import useFetchServices from "@/hooks/useFetchServices";
import useFetchRooms from "@/hooks/useFetchRooms";
import { Service } from "@/types/Service";
import { Room } from "@/types/Room";
import { Gi3dHammer } from "react-icons/gi";
import { useRouter } from "next/navigation";
import { ServiceTicket } from "@/types/ServiceTicket";
import StaffTicket from "./_components/ticket";

import { Tabs, TabItem, Badge } from "flowbite-react";
import { GoIssueClosed, GoIssueOpened, GoIssueTracks } from "react-icons/go";

export default function StaffPage() {
  const [serviceId, setServiceId] = useState<string>("");
  const [showTicket, setShowTicket] = useState<ServiceTicket | null>(null);
  const [filterStatus, setFilterStatus] = useState(0);

  const service = useFetchServices({ id: serviceId });
  const tickets = useFetchServiceTickets({
    serviceId: serviceId,
    status: filterStatus,
  });
  const rawTickets = useFetchServiceTickets();
  const rooms = useFetchRooms();

  const router = useRouter();

  return (
    <div className="min-h-lvh flex flex-row">
      <div className="sticky top-0 max-h-lvh">
        <StaffSidebar
          selected={(id: string) => {
            setServiceId(id);
          }}
        />
      </div>
      <Suspense>
        <div className="flex-1 p-12 space-y-8 ">
          <Tabs
            variant="underline"
            onActiveTabChange={(x) => setFilterStatus(x)}
          >
            <TabItem
              title={
                <>
                  Open
                  <Badge className="ml-2" color="light">
                    {
                      rawTickets.data?.filter(
                        (raw) =>
                          raw.status === 0 && raw.service_id == serviceId,
                      ).length
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
                  <Badge className="ml-2" color="light">
                    {
                      rawTickets.data?.filter(
                        (raw) =>
                          raw.status === 1 && raw.service_id == serviceId,
                      ).length
                    }
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
                    {
                      rawTickets.data?.filter(
                        (raw) =>
                          raw.status === 2 && raw.service_id == serviceId,
                      ).length
                    }
                  </Badge>
                </>
              }
              icon={GoIssueClosed}
            />
          </Tabs>

          <div className="flex flex-wrap gap-8">
            {tickets.isLoading ||
              rooms.isLoading ||
              (service.isLoading && <Spinner />)}

            {(tickets.data?.length as number) <= 0 && (
              <>
                <div className="flex-1 flex flex-col gap-8 items-center justify-center">
                  <Gi3dHammer className="w-20 h-20" />
                  <h1 className="text-2xl">No tickets issued for this one.</h1>
                </div>
              </>
            )}

            {tickets.data?.map((ticket) => (
              <TicketCard
                key={ticket.id}
                ticket={ticket}
                room={rooms.data?.find((x) => x.id === ticket.room_id) as Room}
                service={service.data?.[0] as Service}
                onClick={() => setShowTicket(ticket)}
              ></TicketCard>
            ))}
          </div>
        </div>

        <Modal
          size="4xl"
          dismissible
          show={showTicket !== null}
          onClose={() => {
            setShowTicket(null);
            service.refetch();
            tickets.refetch();
            rawTickets.refetch();
            rooms.refetch();
          }}
        >
          <ModalHeader />

          <ModalBody>
            {showTicket !== null && (
              <StaffTicket id={showTicket?.id as string} />
            )}
          </ModalBody>
        </Modal>
      </Suspense>
    </div>
  );
}
