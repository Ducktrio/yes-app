"use client";
import RoomCard from "@/app/_components/RoomCard";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import { Alert, Spinner, Button, Badge, Tabs, TabItem } from "flowbite-react";
import { GoIssueOpened } from "react-icons/go";
import CreateRoom from "../_components/create-room";
import { useEffect, useState } from "react";
import { toast } from "@/lib/toast";
import { RoomType } from "@/types/RoomType";
import { BsDoorClosedFill, BsDoorOpenFill, BsXSquare } from "react-icons/bs";

export default function ManagerRoomsPage() {
  const roomTypes = useFetchRoomTypes();
  const [createModal, setCreateModal] = useState(false);

  const [filterType, setFilterType] = useState<RoomType | null>(null);
  const [filterStatus, setFilterStatus] = useState<number | null>(null);
  const rooms = useFetchRooms({
    roomTypeId: filterType?.id,
    status: filterStatus,
  });

  useEffect(() => {
    rawRooms.refetch();
  }, [rooms.data, rooms.isRefetching]);

  useEffect(() => {
    rooms.refetch();
  }, [filterType, filterStatus]);

  const rawRooms = useFetchRooms();

  const handleCreated = () => {
    toast("success", "Room created successfully!");
    rooms.refetch();
  };

  return (
    <>
      <h1 className="text-4xl font-bold mb-12">Manage Rooms</h1>

      <div className="flex flex-row justify-between items-center space-x-4 mt-4">
        <Tabs variant="underline" onActiveTabChange={(x) => setFilterStatus(x)}>
          <TabItem
            title={
              <>
                Available
                <Badge className="ml-2" color="light">
                  {rawRooms.data?.filter((raw) => raw.status === 0).length}
                </Badge>
              </>
            }
            icon={BsDoorOpenFill}
          />
          <TabItem
            title={
              <>
                Occupied
                <Badge className="ml-2" color="light">
                  {rawRooms.data?.filter((raw) => raw.status === 1).length}
                </Badge>
              </>
            }
            icon={BsDoorClosedFill}
          />
          <TabItem
            title={
              <>
                Unavailable
                <Badge className="ml-2" color="light">
                  {rawRooms.data?.filter((raw) => raw.status === 2).length}
                </Badge>
              </>
            }
            icon={BsXSquare}
          />
        </Tabs>
        <Button
          onClick={() => setCreateModal(true)}
          className="bg-green-600 hover:bg-green-800 hover:shadow"
        >
          <GoIssueOpened className="mr-2 w-5 h-5" /> Issue new room
        </Button>
      </div>

      <div className="flex flex-wrap space-x-4 mb-4">
        {roomTypes.data?.map((type) => (
          <Button
            color={type.id === filterType?.id ? "default" : "alternative"}
            onClick={() => setFilterType(type)}
            key={type.id}
          >
            {type.name}
          </Button>
        ))}
      </div>

      <hr className="mt-4 mb-4" />

      {rooms.isFetching && (
        <>
          <Spinner></Spinner>
        </>
      )}
      {rooms.isError && (
        <>
          <Alert color="failure">{rooms.error.message}</Alert>
        </>
      )}
      <div className="flex flex-wrap space-x-4 space-y-4 mt-4">
        {!rooms.data && <p>Empty here</p>}
        {rooms.data?.map((room) => (
          <RoomCard
            onUpdate={() => {
              rooms.refetch();
            }}
            room={room}
            roomType={
              roomTypes.data?.find(
                (type) => type.id === room.roomType_id,
              ) as RoomType
            }
            key={room.id}
          />
        ))}
      </div>

      <CreateRoom
        openModal={createModal}
        closeModal={() => setCreateModal(false)}
        onSuccess={handleCreated}
      />
    </>
  );
}
