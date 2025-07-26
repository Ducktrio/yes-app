import { Room } from "@/types/Room";
import { RoomType } from "@/types/RoomType";

import { BsDoorClosedFill } from "react-icons/bs";
import { HiCash, HiCheck, HiExclamationCircle } from "react-icons/hi";
import {
  Badge,
  Button,
  Card,
  Table,
  TableBody,
  TableCell,
  TableRow,
} from "flowbite-react";
import { useRouter } from "next/navigation";

export default function ReceptionRoomDetail({
  room,
  roomType,
}: {
  room: Room;
  roomType: RoomType;
}) {
  const router = useRouter();
  return (
    <div className="flex flex-col gap-8 p-8">
      <Card className="p-8 rounded-lg bg-stone-800 text-stone-100 flex justify-center items-center">
        <h1 className="text-3xl font-semibold">Room {room?.label}</h1>
      </Card>

      <Card>
        <Table>
          <TableBody>
            <TableRow>
              <TableCell>Type</TableCell>
              <TableCell>{roomType?.name}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell>Price Tag</TableCell>
              <TableCell className="flex flex-wrap">
                <Badge icon={HiCash} color="info" className="mt-2">
                  Rp. {roomType?.price}
                </Badge>
              </TableCell>
            </TableRow>
            <TableRow>
              <TableCell>Availability</TableCell>
              <TableCell>
                <div className="flex flex-wrap gap-4">
                  {room.status === 0 && (
                    <>
                      <Badge icon={HiCheck} color="success" className="mt-2">
                        Available
                      </Badge>
                    </>
                  )}

                  {room.status === 1 && (
                    <Badge
                      icon={BsDoorClosedFill}
                      color="gray"
                      className="mt-2"
                    >
                      Occupied
                    </Badge>
                  )}

                  {room.status === 2 && (
                    <Badge
                      icon={HiExclamationCircle}
                      color="warning"
                      className="mt-2"
                    >
                      Unavailable
                    </Badge>
                  )}
                </div>
              </TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </Card>
      <hr />

      {room.status === 0 && (
        <Button
          className="py-4 w-full"
          onClick={() => router.push(`/reception/checkin?room=${room.id}`)}
        >
          <BsDoorClosedFill className="mr-2 w-5 h-5" />
          Check In
        </Button>
      )}
    </div>
  );
}
