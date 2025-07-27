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
import { HiUser, HiViewBoards } from "react-icons/hi";
import { BsDoorOpenFill } from "react-icons/bs";
import { useAuth } from "@/contexts/AuthContext";

const navLinks = [
  [
    { name: "Dashboard", href: "/manager", icon: HiViewBoards },
    { name: "Users", href: "/manager/users", icon: HiUser },
  ],
  [{ name: "Rooms", href: "/manager/rooms", icon: BsDoorOpenFill }],
  [
    {
      name: "Tickets",
      href: "/manager/tickets",
      icon: HiViewBoards,
    },
  ],
];

export default function ManagerSidebar() {
  const pathname = usePathname();
  const auth = useAuth();

  return (
    <Sidebar aria-label="Manager panel sidebar">
      <SidebarItems>
        <SidebarItemGroup>
          <div className="mb-8">
            <b className="text-2xl">{process.env.NEXT_PUBLIC_APP_NAME}</b>
            <br />
            <i>Manager</i>
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
                {link.name}
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
