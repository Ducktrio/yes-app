"use client";
import {
  Avatar,
  Dropdown,
  DropdownItem,
  Sidebar,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
  SidebarLogo,
} from "flowbite-react";
import { usePathname } from "next/navigation";
import { HiBookOpen, HiUser, HiViewBoards } from "react-icons/hi";
import { BsDoorClosed, BsDoorOpenFill, BsTicketDetailed } from "react-icons/bs";
import { useAuth } from "@/contexts/AuthContext";
import useFetchRoomTickets from "@/hooks/useFetchRoomTickets";
import useFetchServices from "@/hooks/useFetchServices";
import useFetchServiceTickets from "@/hooks/useFetchServiceTickets";
import { useEffect, useState } from "react";

export default function StaffSidebar({
  selected,
}: {
  selected: (id: string) => void;
}) {
  const auth = useAuth();
  const services = useFetchServices();
  const rawTickets = useFetchServiceTickets();
  const [selectService, setSelectService] = useState<string>("");

  useEffect(() => {
    selected(selectService);
  }, [selectService, selected]);

  return (
    <Sidebar aria-label="Reception panel sidebar">
      <SidebarItems>
        <SidebarItemGroup>
          <div className="mb-8">
            <b className="text-2xl">{process.env.NEXT_PUBLIC_APP_NAME}</b>
            <br />
            <i>Staff</i>
          </div>
        </SidebarItemGroup>
        <SidebarItemGroup>
          {services.data?.map((service) => (
            <SidebarItem
              key={service.id}
              icon={BsTicketDetailed}
              active={selectService === service.id}
              onClick={() => {
                setSelectService(service.id);
              }}
            >
              <div className="flex items-center justify-between">
                <span>{service.name}</span>
                {(rawTickets.data?.filter(
                  (x) => x.service_id === service.id && x.status == 0,
                ).length as number) > 0 && (
                  <span className="relative flex size-3">
                    <span className="absolute inline-flex h-full w-full animate-ping rounded-full bg-yellow-400 opacity-75"></span>{" "}
                    <span className="relative inline-flex size-3 rounded-full bg-yellow-500"></span>
                  </span>
                )}
              </div>
            </SidebarItem>
          ))}
        </SidebarItemGroup>

        <SidebarItemGroup>
          <SidebarItem className="flex items-start justify-start">
            <Dropdown
              inline
              label={
                <Avatar placeholderInitials={auth.user?.username[0]} rounded>
                  <div className="space-y-1 font-medium">
                    <div>{auth.user?.username}</div>
                  </div>
                </Avatar>
              }
            >
              <DropdownItem onClick={auth.logout}>Logout</DropdownItem>
            </Dropdown>
          </SidebarItem>
        </SidebarItemGroup>
      </SidebarItems>
    </Sidebar>
  );
}
