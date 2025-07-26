"use client";
import Checkin from "@/app/_components/checkin";
import useFetchCustomers from "@/hooks/useFetchCustomers";
import useFetchRooms from "@/hooks/useFetchRooms";
import { Spinner } from "flowbite-react";
import { useSearchParams } from "next/navigation";

export default function CheckinPage() {
  const handleSuccess = () => {};

  const params = useSearchParams();
  const room = useFetchRooms({ id: params.get("room") as string });

  const customer = useFetchCustomers({ id: params.get("customer") as string });

  return (
    <>
      <h1 className="text-4xl mb-8">Check-in</h1>

      {customer.isLoading || room.isLoading ? (
        <Spinner />
      ) : (
        <Checkin
          onSuccess={handleSuccess}
          room={params.has("room") ? room.data?.[0] : null}
          customer={params.has("customer") ? customer.data?.[0] : null}
        />
      )}
    </>
  );
}
