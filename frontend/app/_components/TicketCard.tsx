import { Room } from "@/types/Room";
import { Service } from "@/types/Service";
import { ServiceTicket } from "@/types/ServiceTicket";
import { Badge, Card, Textarea } from "flowbite-react";
import { GoIssueClosed, GoIssueOpened, GoIssueTracks } from "react-icons/go";

interface TicketCardProps {
  ticket: ServiceTicket;
  room: Room;
  service: Service;
  onUpdate?: () => void;
  onClick?: () => void;
}

export default function TicketCard({
  ticket,
  room,
  service,
  onUpdate,
  onClick,
}: TicketCardProps) {
  return (
    <>
      <Card className="min-w-sm hover:shadow-xl" onClick={onClick}>
        <div className="bg-gray-600 rounded p-8 flex flex-col justify-center items-center gap-4">
          <h5 className="text-2xl font-bold text-white">{service?.name}</h5>

          <Badge
            className="rounded-full px-4"
            size="lg"
            icon={
              ticket.status === 0
                ? GoIssueOpened
                : ticket.status === 1
                  ? GoIssueTracks
                  : GoIssueClosed
            }
            color={
              ticket.status === 0
                ? "success"
                : ticket.status === 1
                  ? "warning"
                  : "failure"
            }
          >
            {ticket.status === 0
              ? "Open"
              : ticket.status === 1
                ? "In Progress"
                : "Closed"}
          </Badge>
        </div>

        <div className="block space-y-4">
          <h5 className="font-semibold">Room {room?.label}</h5>

          <Textarea
            disabled
            rows={4}
            value={ticket.details}
            className="w-full"
          ></Textarea>
        </div>

        {ticket.status === 0 && (
          <span className="flex items-center gap-2 text-gray-400 text-sm">
            Posted 1 min ago
          </span>
        )}
      </Card>
    </>
  );
}
