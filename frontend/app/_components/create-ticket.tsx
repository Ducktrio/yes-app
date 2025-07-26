import ServiceTicketService from "@/api/ServiceTicketService";
import useFetchCustomers from "@/hooks/useFetchCustomers";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchServices from "@/hooks/useFetchServices";
import { MdKeyboardReturn } from "react-icons/md";
import {
  Button,
  Kbd,
  TextInput,
  Timeline,
  TimelineBody,
  TimelineContent,
  TimelineItem,
  TimelinePoint,
  TimelineTitle,
  Textarea,
} from "flowbite-react";
import { toast } from "@/lib/toast";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import { CreateServiceTicketPayload } from "@/types/ServiceTicket";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { GoGitCompare, GoNote, GoPerson } from "react-icons/go";
import { GetRoomQuery } from "@/api/RoomService";
import { GetServiceQuery } from "@/api/ServiceService";
import { GetCustomerQuery } from "@/api/CustomerService";

interface CreateTicketProps {
  onSuccess?: () => void;
  fill?: Partial<CreateServiceTicketPayload>;
}

export default function CreateTicket({ fill, onSuccess }: CreateTicketProps) {
  const [form, setForm] = useState<CreateServiceTicketPayload>({
    customer_id: fill?.customer_id || "",
    service_id: fill?.service_id || "",
    room_id: fill?.room_id || "",
    details: "",
  });

  const [validationError, setValidationError] = useState<
    ValidationErrorResponse[]
  >([]);

  const [roomQuery, setRoomQuery] = useState<GetRoomQuery | null>(null);
  const [serviceQuery, setServiceQuery] = useState<GetServiceQuery | null>(
    null,
  );
  const [customerQuery, setCustomerQuery] = useState<GetCustomerQuery | null>(
    null,
  );

  const services = useFetchServices(serviceQuery!);
  const rooms = useFetchRooms(roomQuery!);
  const customers = useFetchCustomers(customerQuery!);

  const mutation = useMutation({
    mutationKey: ["create-ticket"],
    mutationFn: ServiceTicketService.createServiceTicket,
    onSuccess: () => {
      onSuccess?.();
      toast("success", "Ticket created successfully!");
    },
    onError: (error) => {
      if (error instanceof ValidationError) {
        setValidationError(error.errors);
      }
      toast("error", error.message);
    },
  });

  const onSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (mutation.isPending) {
      toast("warning", "Please wait for the current request to finish.");
      return;
    }
    mutation.mutate(form);
  };

  return (
    <>
      <form
        onSubmit={onSubmit}
        className="space-y-4 space-x-4 p-12 overflow-y-scroll"
      >
        <Timeline>
          <TimelineItem>
            <TimelinePoint icon={GoGitCompare} />
            <TimelineContent className="space-y-4">
              <TimelineTitle>Choose service</TimelineTitle>

              <TimelineBody className="flex flex-wrap border bg-gray-200 text-gray-800 p-4 rounded gap-4">
                {services.data?.map((service) => (
                  <Button
                    key={service.id}
                    color={
                      form.service_id === service.id ? "default" : "alternative"
                    }
                    onClick={() => setForm({ ...form, service_id: service.id })}
                  >
                    {service.name}
                  </Button>
                ))}
              </TimelineBody>
            </TimelineContent>
          </TimelineItem>

          <TimelineItem>
            <TimelinePoint icon={GoGitCompare} />

            <TimelineContent className="flex flex-col gap-4">
              <TimelineTitle>Choose room</TimelineTitle>

              <TimelineBody className="space-y-4">
                <TextInput
                  type="search"
                  value={roomQuery?.label}
                  onChange={(e) => setRoomQuery({ label: e.target.value })}
                  placeholder="Search room by label..."
                />
                <div className="flex flex-wrap border bg-gray-200 text-gray-800 p-4 rounded gap-4">
                  {rooms.data?.map((room) => (
                    <Button
                      key={room.id}
                      color={
                        room.id === form.room_id ? "default" : "alternative"
                      }
                      onClick={() => setForm({ ...form, room_id: room.id })}
                    >
                      {room.label}
                    </Button>
                  ))}
                </div>
              </TimelineBody>
            </TimelineContent>
          </TimelineItem>

          <TimelineItem className="block space-y-2">
            <TimelinePoint icon={GoPerson} />

            <TimelineContent className="flex flex-col gap-4">
              <TimelineTitle>Choose customer</TimelineTitle>

              <TimelineBody className="space-y-4">
                <TextInput
                  type="search"
                  value={customerQuery?.full_name}
                  onChange={(e) =>
                    setCustomerQuery({ full_name: e.target.value })
                  }
                  placeholder="Search room by label..."
                />

                <div className="flex flex-wrap border bg-gray-200 text-gray-800 p-4 rounded gap-4">
                  {customers.data?.map((customer) => (
                    <Button
                      color={
                        customer.id === form.customer_id
                          ? "default"
                          : "alternative"
                      }
                      onClick={() =>
                        setForm({ ...form, customer_id: customer.id })
                      }
                      key={customer.id}
                    >
                      {customer.courtesy_title} {customer.full_name}
                    </Button>
                  ))}
                </div>
              </TimelineBody>
            </TimelineContent>
          </TimelineItem>

          <TimelineItem className="block space-y-2">
            <TimelinePoint icon={GoNote} />

            <TimelineContent>
              <TimelineTitle>Details</TimelineTitle>
              <TimelineBody className="block">
                <Textarea
                  className="w-full"
                  value={form.details}
                  onChange={(e) =>
                    setForm({ ...form, details: e.target.value })
                  }
                  placeholder="Enter ticket details..."
                />
              </TimelineBody>
            </TimelineContent>
          </TimelineItem>
        </Timeline>

        <Button type="submit" className="bg-green-600 text-white p-8 w-full">
          <>
            Create <Kbd className="ml-4" icon={MdKeyboardReturn}></Kbd>
          </>
        </Button>
      </form>
    </>
  );
}
