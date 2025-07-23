import { Room } from "@/types/Room";
import { RoomType } from "@/types/RoomType";
import { Badge, Card } from "flowbite-react";
import { useState } from "react";
import { BsDoorClosedFill } from "react-icons/bs";
import { HiCash, HiCheck, HiExclamationCircle } from "react-icons/hi";
import DetailRoom from "../manager/_components/detail-room";
import { toast } from "@/lib/toast";

export default function RoomCard({
  room,
  roomType,
  onUpdate,
}: {
  room: Room;
  roomType: RoomType;
  onUpdate?: () => void;
}) {
  const [showDetail, setShowDetail] = useState(false);

  const handleUpdate = () => {
    setShowDetail(false);
    toast("success", "Room updated successfully!");
    onUpdate?.();
  };

  return (
    <>
      <Card
        className="w-full max-w-sm text-center space-y-4 bg-white shadow-md animate-slide-in duration-300"
        onClick={() => setShowDetail(true)}
      >
        <h5 className="text-2xl font-bold tracking-tight text-gray-900">
          {room.label}
        </h5>
        <Card>
          Type <b>{roomType?.name}</b>
        </Card>

        <div className="flex flex-wrap gap-4">
          {room.status === 0 && (
            <>
              <Badge icon={HiCheck} color="success" className="mt-2">
                Available
              </Badge>
              <Badge icon={HiCash} color="info" className="mt-2">
                Rp. {roomType?.price}
              </Badge>
            </>
          )}

          {room.status === 1 && (
            <Badge icon={BsDoorClosedFill} color="gray" className="mt-2">
              Occupied
            </Badge>
          )}

          {room.status === 2 && (
            <Badge icon={HiExclamationCircle} color="warning" className="mt-2">
              Unavailable
            </Badge>
          )}
        </div>
      </Card>

      <DetailRoom
        openModal={showDetail}
        closeModal={() => setShowDetail(false)}
        onSuccess={handleUpdate}
        room={room}
      />
    </>
  );
}
