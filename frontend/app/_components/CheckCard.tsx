import { Customer } from "@/types/Customer";
import { Room } from "@/types/Room";
import { RoomTicket } from "@/types/RoomTicket";
import { Badge, Card } from "flowbite-react";
import {
  BsDoorClosedFill,
  BsDoorOpen,
  BsDoorOpenFill,
  BsPeopleFill,
  BsPersonFill,
} from "react-icons/bs";

interface Props {
  check: RoomTicket;
  room: Room;
  customer: Customer;
  onClick?: () => void;
}

export default function CheckCard({ check, room, customer, onClick }: Props) {
  return (
    <>
      <div className="flex flex-col gap-4" onClick={onClick}>
        <Card className="bg-stone-800 text-stone-200 flex flex-row gap-4 justify-center items-center text-2xl">
          {check.check_in_date == null ? (
            <BsDoorOpenFill />
          ) : check.check_out_date == null ? (
            <BsDoorClosedFill />
          ) : (
            <BsDoorOpen />
          )}
          {room?.label}
        </Card>

        <Card>
          <span className="flex items-center gap-2">
            <BsPeopleFill /> {check.number_of_occupants}
          </span>

          <span className="flex items-center gap-2">
            <BsPersonFill /> {customer.courtesy_title} {customer.full_name}
          </span>

          <span className="flex items-center gap-2">
            Status{" "}
            <Badge
              color={
                check.check_in_date == null
                  ? "warning"
                  : check.check_out_date == null
                    ? "success"
                    : "gray"
              }
            >
              {check.check_in_date == null
                ? "Pending Checkin"
                : check.check_out_date == null
                  ? "Checked In"
                  : "Checked Out"}
            </Badge>
          </span>
        </Card>
      </div>
    </>
  );
}
