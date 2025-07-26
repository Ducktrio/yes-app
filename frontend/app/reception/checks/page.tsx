"use client";

import CheckCard from "@/app/_components/CheckCard";
import useFetchCustomers from "@/hooks/useFetchCustomers";
import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchRoomTickets from "@/hooks/useFetchRoomTickets";
import { Customer } from "@/types/Customer";
import { Room } from "@/types/Room";
import { Badge, Spinner, TabItem, Tabs, TextInput } from "flowbite-react";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { BsDoorClosedFill, BsDoorOpenFill } from "react-icons/bs";

export default function ReceptionChecksPage() {
  const router = useRouter();
  const [filterStatus, setFilterStatus] = useState(0);

  const [query, setQuery] = useState("");

  const rawChecks = useFetchRoomTickets();

  const rooms = useFetchRooms({ label: query });

  const customers = useFetchCustomers();

  const checks = useFetchRoomTickets({
    checkedIn: filterStatus ? true : false,
    status: 0, // Exclude checked out tickets
    roomId: query ? rooms.data?.[0].id : undefined,
  });

  return (
    <>
      <h1 className="text-4xl">Room Checks</h1>

      <Tabs variant="underline" onActiveTabChange={(x) => setFilterStatus(x)}>
        <TabItem
          title={
            <>
              Pending Check In
              {rawChecks.data?.filter((raw) => raw.check_in_date == null)
                .length! > 0 && (
                <Badge className="ml-2 animate-bounce" color="failure">
                  {
                    rawChecks.data?.filter((raw) => raw.check_in_date == null)
                      .length
                  }
                </Badge>
              )}
            </>
          }
          icon={BsDoorOpenFill}
        />
        <TabItem
          title={
            <>
              Checked In
              <Badge className="ml-2" color="gray">
                {
                  rawChecks.data?.filter(
                    (raw) =>
                      raw.check_in_date != null && raw.check_out_date == null,
                  ).length
                }
              </Badge>
            </>
          }
          icon={BsDoorClosedFill}
        />
      </Tabs>

      <TextInput
        type="search"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        placeholder="Search by room label"
      />

      <div className="flex flex-wrap gap-8">
        {checks.isLoading && <Spinner />}
        {(checks.data?.length as number) <= 0 && (
          <>No room checks registered.</>
        )}
        {checks.data?.map((check) => (
          <CheckCard
            check={check}
            room={rooms.data?.find((room) => room.id === check.room_id) as Room}
            customer={
              customers.data?.find((x) => x.id == check.customer_id) as Customer
            }
            key={check.id}
            onClick={() => {
              router.push(`/reception/checks/${check.id}`);
            }}
          />
        ))}
      </div>
    </>
  );
}
