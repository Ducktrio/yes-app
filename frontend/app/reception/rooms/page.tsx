"use client";
import RoomCard from "@/app/_components/RoomCard";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import { RoomType } from "@/types/RoomType";
import {
  Tabs,
  TabItem,
  Badge,
  Spinner,
  Drawer,
  DrawerHeader,
  DrawerItems,
  TextInput,
} from "flowbite-react";
import { useCallback, useMemo, useState } from "react";
import { BsDoorClosedFill, BsDoorOpenFill, BsXSquare } from "react-icons/bs";
import ReceptionRoomDetail from "../_components/room-detail";
import { Room } from "@/types/Room";

export default function ReceptionRoomsPage() {
  const [filterStatus, setFilterStatus] = useState(0);
  const [showDetail, setShowDetail] = useState(false);
  const [detailRoom, setDetailRoom] = useState<Room | null>(null);
  const [query, setQuery] = useState("");

  const rawRooms = useFetchRooms();
  const rooms = useFetchRooms({
    status: filterStatus,
    label: query ? query : null,
  });
  const roomTypes = useFetchRoomTypes();

  return (
    <>
      <h1 className="text-4xl">Room List</h1>

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

      <TextInput
        type="search"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        className="mt-4 mb-4"
        placeholder="Search by room label"
      />

      <div className="flex flex-wrap gap-8 p-8 bg-gray-100 rounded-xl">
        {rooms.isLoading && (
          <>
            <Spinner></Spinner> Loading...
          </>
        )}

        {roomTypes.data &&
          rooms.data?.map((room) => (
            <RoomCard
              key={room.id}
              room={room}
              roomType={
                roomTypes.data?.find(
                  (x) => x.id === room.roomType_id,
                ) as RoomType
              }
              onUpdate={() => {}}
              onClick={() => {
                setShowDetail(true);
                setDetailRoom(room);
              }}
            />
          ))}
      </div>

      <Drawer
        open={showDetail && detailRoom !== null}
        onClose={() => {
          setShowDetail(false);
          setDetailRoom(null);
        }}
        position="right"
        backdrop
        className="w-[32rem]"
      >
        <DrawerHeader></DrawerHeader>
        <DrawerItems>
          {showDetail && (
            <ReceptionRoomDetail
              room={detailRoom!}
              roomType={
                roomTypes.data?.find(
                  (x) => x.id == detailRoom?.roomType_id,
                ) as RoomType
              }
            />
          )}
        </DrawerItems>
      </Drawer>
    </>
  );
}
