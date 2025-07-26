"use client";
import useFetchRooms from "@/hooks/useFetchRooms";
import { Spinner } from "flowbite-react";
import { useEffect, useState } from "react";
import DrawerListAvailables from "./_components/drawer-list-availables";
import DrawerListUnavailables from "./_components/drawer-list-unavailables";
import {
  BsDoorOpen,
  BsTicketDetailed,
  BsTicketDetailedFill,
} from "react-icons/bs";
import { useRouter } from "next/navigation";

export default function ReceptionPage() {
  const router = useRouter();
  const [showAvailables, setShowAvailables] = useState(false);
  const [showUnavailable, setShowUnavailable] = useState(false);
  const availableRooms = useFetchRooms({ status: 0 });
  const unavailableRooms = useFetchRooms({ status: 2 });

  useEffect(() => {
    if (showAvailables && showUnavailable) {
      setShowAvailables(false);
    }
  }, [showUnavailable]);

  useEffect(() => {
    if (showUnavailable && showAvailables) {
      setShowUnavailable(false);
    }
  }, [showAvailables]);

  return (
    <>
      <div className="flex flex-row items-stretch space-y-4 space-x-4 mt-4">
        <div
          className="flex flex-col justify-start p-12 shadow bg-green-200 h-full hover:shadow-xl"
          onClick={() => setShowAvailables(!showAvailables)}
        >
          <h4>Available Rooms</h4>
          <div className="flex-1 flex justify-center items-center">
            {availableRooms.isLoading && <Spinner className="h-10 w-10" />}
            {availableRooms.data && (
              <span className="font-semibold text-4xl text-green-800">
                {availableRooms.data?.length}
              </span>
            )}
          </div>
        </div>

        <div
          className="flex flex-col p-12 shadow bg-yellow-200 h-full hover:shadow-xl"
          onClick={() => setShowUnavailable(!showUnavailable)}
        >
          <h4>Unavailable Rooms</h4>
          <div className="flex-1 flex justify-center items-center">
            {unavailableRooms.isLoading && <Spinner className="h-10 w-10" />}
            {unavailableRooms.data && (
              <span className="font-semibold text-4xl text-yellow-800">
                {unavailableRooms.data?.length}
              </span>
            )}
          </div>
        </div>
      </div>

      <hr className="my-8 text-gray-600" />

      <div className="flex flex-col gap-8">
        <h2 className="text-4xl mb-8">Quick Actions</h2>

        <div className="flex flex-wrap gap-8">
          <button
            className="p-8 rounded border text-black border-black hover:text-white hover:bg-black flex flex-col justify-center items-center text-2xl font-light gap-4"
            onClick={() => {
              router.push("/reception/checkin");
            }}
          >
            <BsDoorOpen />
            Check In
          </button>

          <button
            className="p-8 rounded border text-black border-black hover:text-white hover:bg-black flex flex-col justify-center items-center text-2xl font-light gap-4"
            onClick={() => {
              router.push("/reception/checkout");
            }}
          >
            <BsDoorOpen />
            Check Out
          </button>

          <button
            className="p-8 rounded border text-black border-black hover:text-white hover:bg-black flex flex-col justify-center items-center text-2xl font-light gap-4"
            onClick={() => {
              router.push("/reception/tickets");
            }}
          >
            <BsTicketDetailed />
            Open Ticket
          </button>
        </div>
      </div>

      <DrawerListAvailables open={showAvailables} />
      <DrawerListUnavailables open={showUnavailable} />
    </>
  );
}
