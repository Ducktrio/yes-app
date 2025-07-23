"use client";
import {
  Sidebar,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
  SidebarLogo,
} from "flowbite-react";
import { usePathname } from "next/navigation";
import { HiUser, HiViewBoards } from "react-icons/hi";
import { BsDoorOpenFill } from "react-icons/bs";

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

  return (
    <Sidebar aria-label="Manager panel sidebar">
      {process.env.NEXT_PUBLIC_APP_NAME}
      <SidebarItems>
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
      </SidebarItems>
    </Sidebar>
  );
}
