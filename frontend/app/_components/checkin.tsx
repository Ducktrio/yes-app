"use client";

import useFetchCustomers from "@/hooks/useFetchCustomers";
import useFetchRooms from "@/hooks/useFetchRooms";
import {
  Alert,
  Button,
  Spinner,
  TextInput,
  Timeline,
  TimelineBody,
  TimelineContent,
  TimelineItem,
  TimelinePoint,
  TimelineTime,
  TimelineTitle,
} from "flowbite-react";
import { useState } from "react";
import {
  BsArrowDown,
  BsArrowUp,
  BsDoorClosedFill,
  BsPeople,
  BsPeopleFill,
  BsPerson,
  BsPersonFill,
  BsPlus,
} from "react-icons/bs";
import RoomCard from "./RoomCard";
import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import { RoomType } from "@/types/RoomType";
import { Room } from "@/types/Room";
import { Customer } from "@/types/Customer";
import RoomTicketService from "@/api/RoomTicketService";
import { useMutation } from "@tanstack/react-query";
import { toast } from "@/lib/toast";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import { usePathname, useRouter, useSearchParams } from "next/navigation";

interface Props {
  room?: Room | null;
  customer?: Customer | null;
  onSuccess?: () => void;
}

export default function Checkin({ room, customer, onSuccess }: Props) {
  const [errors, setErrors] = useState<ValidationErrorResponse[]>([]);
  const router = useRouter();
  const pathname = usePathname();
  const params = useSearchParams();
  const mutation = useMutation({
    mutationFn: RoomTicketService.createRoomTicket,
    mutationKey: ["createRoomTicket"],

    onError: (error) => {
      toast("error", error.message);

      if (error instanceof ValidationError) {
        setErrors(error.errors);
      }
    },
    onSuccess: (data: Room) => {
      toast("success", "Room Check created");
      onSuccess?.();
      router.push(`/reception/checks/${data.id}`);
    },
  });

  const [selectedRoom, setSelectedRoom] = useState<Room | null>(room ?? null);
  const [selectedCustomer, setSelectedCustomer] = useState<Customer | null>(
    customer ?? null,
  );

  const [customerQuery, setCustomerQuery] = useState("");

  const customers = useFetchCustomers({
    full_name: customerQuery,
  });

  const [roomQuery, setRoomQuery] = useState("");

  const rooms = useFetchRooms({
    label: roomQuery,
    status: 0, // Availables only
  });
  const roomTypes = useFetchRoomTypes();

  const [occupants, setOccupants] = useState<number>(1);

  const handleSubmit = () => {
    mutation.mutate({
      customer_id: selectedCustomer?.id as string,
      room_id: selectedRoom?.id as string,
      number_of_occupants: occupants,
    });
  };

  return (
    <>
      {errors.length > 0 && (
        <Alert color="red">
          <ul>
            {errors.map((error) => (
              <li key={error.resourceName} className="text-red-600">
                {error.errorMessage}
              </li>
            ))}
          </ul>
        </Alert>
      )}
      <Timeline>
        <TimelineItem>
          <TimelinePoint icon={BsPersonFill} />
          <TimelineContent className="space-y-8">
            <TimelineTitle>Enter customer data</TimelineTitle>
            <TimelineBody className="space-y-8">
              <TextInput
                value={customerQuery}
                onChange={(e) => setCustomerQuery(e.target.value)}
              />

              <div className="flex flex-wrap gap-8 ">
                {customers.isLoading && <Spinner />}
                {customers.data?.map((customer) => (
                  <div
                    key={customer.id}
                    className={`rounded-lg border border-stone-800 text-stone-800 p-8 flex flex-col justify-center items-center gap-4
 ${selectedCustomer?.id === customer.id ? "bg-blue-200" : ""}`}
                    onClick={() => {
                      setSelectedCustomer(customer);
                    }}
                  >
                    {" "}
                    <BsPerson className="w-8 h-8"></BsPerson>
                    <span className="text-lg text-center">
                      {customer.courtesy_title} {customer.full_name}
                    </span>
                  </div>
                ))}
              </div>

              <a
                className="text-blue-600 underline"
                onClick={() =>
                  router.push(
                    `/reception/customer?redirect_to=${pathname}${params.size > 0 ? `?${params.toString()}` : ""}`,
                  )
                }
              >
                Register new customer
              </a>
            </TimelineBody>
          </TimelineContent>
        </TimelineItem>

        <TimelineItem>
          <TimelinePoint icon={BsDoorClosedFill} />
          <TimelineContent className="space-y-8">
            <TimelineTitle>Enter room</TimelineTitle>
            <TimelineBody className="space-y-8">
              <TextInput
                value={roomQuery}
                onChange={(e) => setRoomQuery(e.target.value)}
              />

              <div className="flex flex-wrap p-8 gap-8">
                {rooms.isLoading && <Spinner />}
                {rooms.data?.map((room) => (
                  <div
                    key={room.id}
                    className={`rounded-lg ${room.id === selectedRoom?.id ? "bg-blue-400 p-2" : ""}`}
                  >
                    <RoomCard
                      key={room.id}
                      room={room}
                      roomType={
                        roomTypes.data?.find(
                          (x) => x.id == room.roomType_id,
                        ) as RoomType
                      }
                      onClick={() => {
                        setSelectedRoom(room);
                      }}
                    />
                  </div>
                ))}
              </div>
            </TimelineBody>
          </TimelineContent>
        </TimelineItem>

        <TimelineItem>
          <TimelinePoint icon={BsPeopleFill} />
          <TimelineContent className="space-y-8">
            <TimelineTitle>Occupants</TimelineTitle>
            <TimelineBody className="space-y-8">
              <div className="grid grid-cols-4 grid-rows-2 gap-8 max-w-sm">
                <div className="col-span-3 row-span-2 p-4 shadow rounded-lg text-4xl flex items-center justify-center">
                  {occupants}{" "}
                  {occupants > 1 ? (
                    <BsPeople className="ml-4" />
                  ) : (
                    <BsPerson className="ml-4" />
                  )}
                </div>
                <div
                  className="col-span-1 row-span-1 p-4 border shadow rounded-lg flex items-center justify-center text-stone-800 hover:border-gray-400 hover:shadow-lg"
                  onClick={() => setOccupants(occupants + 1)}
                >
                  <BsArrowUp />
                </div>

                <div
                  className="col-span-1 row-span-1 p-4 border shadow rounded-lg flex items-center justify-center text-stone-800 hover:border-gray-400 hover:shadow-lg"
                  onClick={() => setOccupants(occupants - 1)}
                >
                  <BsArrowDown />
                </div>
              </div>
            </TimelineBody>
          </TimelineContent>
        </TimelineItem>

        <TimelineItem>
          <Button
            className="bg-green-600 hover:shadow hover:bg-green-800"
            onClick={handleSubmit}
            disabled={mutation.isPending}
          >
            {mutation.isIdle && (
              <>
                <BsPlus className="mr-4 w-5 h-5" /> Checkin
              </>
            )}
            {mutation.isPending && (
              <>
                <Spinner className="mr-4 w-5 h-5" />
                Processing...
              </>
            )}
          </Button>
        </TimelineItem>
      </Timeline>
    </>
  );
}
