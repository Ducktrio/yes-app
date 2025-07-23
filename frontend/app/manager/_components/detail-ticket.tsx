import ServiceTicketService from "@/api/ServiceTicketService";
import { toast } from "@/lib/toast";
import { Room } from "@/types/Room";
import { Service } from "@/types/Service";
import { ServiceTicket } from "@/types/ServiceTicket";
import { useMutation } from "@tanstack/react-query";
import {
  Button,
  Spinner,
  Table,
  TableBody,
  TableCell,
  TableRow,
} from "flowbite-react";

interface Props {
  ticket: ServiceTicket;
  room: Room;
  service: Service;
  onChange?: () => void;
}
export default function ManagerDetailTicket({
  ticket,
  room,
  service,
  onChange,
}: Props) {
  const deleteMutation = useMutation({
    mutationKey: ["deleteTicket", ticket?.id],
    mutationFn: ServiceTicketService.deleteServiceTicket,
    onSuccess: () => {
      toast("success", "Ticket has been voided");
      onChange?.();
    },
  });

  if (ticket)
    return (
      <div className="flex flex-col gap-12">
        <h1 className="text-4xl font-bold">Ticket Details</h1>
        <hr />

        <Table>
          <TableBody>
            {ticket !== null &&
              Object.entries(ticket).map(([key, value]) => (
                <TableRow key={key}>
                  <TableCell className="font-bold">{key}</TableCell>
                  <TableCell>
                    {typeof value === "object" && value !== null
                      ? JSON.stringify(value)
                      : String(value)}
                  </TableCell>
                </TableRow>
              ))}
          </TableBody>
        </Table>

        <hr />

        <Button
          className="bg-red-600 hover:shadow-red-200 hover:shadow hover:bg-red-800"
          onClick={() => {
            deleteMutation.mutate(ticket.id);
          }}
          disabled={deleteMutation.isPending}
        >
          {deleteMutation.isIdle && <>Void Ticket</>}
          {deleteMutation.isPending && (
            <>
              <Spinner color="failure" className="mr-4 w-5 h-5"></Spinner>{" "}
              Processing...
            </>
          )}
        </Button>
      </div>
    );
  else {
    return <>No ticket selected</>;
  }
}
