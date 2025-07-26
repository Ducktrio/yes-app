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

export default function ReceptionSidebar() {
  const pathname = usePathname();
  const auth = useAuth();
  const rawChecks = useFetchRoomTickets();

  const navLinks = [
    [{ name: "Dashboard", href: "/reception", icon: HiViewBoards }],
    [
      {
        name: "Checks",
        href: "/reception/checks",
        icon: HiBookOpen,
        notification:
          (rawChecks.data?.filter((raw) => raw.check_in_date == null)
            .length as number) > 0,
      },
      { name: "Checkin", href: "/reception/checkin", icon: BsDoorOpenFill },

      { name: "Checkout", href: "/reception/checkout", icon: BsDoorOpenFill },
    ],
    [{ name: "Rooms", href: "/reception/rooms", icon: BsDoorClosed }],
    [
      {
        name: "Tickets",
        href: "/reception/tickets",
        icon: BsTicketDetailed,
      },
    ],
  ];

  return (
    <Sidebar aria-label="Reception panel sidebar">
      <SidebarItems>
        <SidebarItemGroup>
          <div className="mb-8">
            <b className="text-2xl">{process.env.NEXT_PUBLIC_APP_NAME}</b>
            <br />
            <i>Reception</i>
          </div>
        </SidebarItemGroup>
        {navLinks.map((group, index) => (
          <SidebarItemGroup key={index}>
            {group.map((link) => (
              <SidebarItem
                key={link.name}
                href={link.href}
                icon={link.icon}
                active={pathname === link.href}
              >
                <div className="flex items-center justify-between">
                  <span>{link.name}</span>
                  {link.notification && (
                    <span className="relative flex size-3">
                      <span className="absolute inline-flex h-full w-full animate-ping rounded-full bg-yellow-400 opacity-75"></span>{" "}
                      <span className="relative inline-flex size-3 rounded-full bg-yellow-500"></span>
                    </span>
                  )}
                </div>
              </SidebarItem>
            ))}
          </SidebarItemGroup>
        ))}

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
