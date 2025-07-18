"use client";
import {
  Sidebar,
  SidebarItem,
  SidebarItemGroup,
  SidebarItems,
  SidebarLogo,
} from "flowbite-react";
import { usePathname } from "next/navigation";
import {
  HiArrowSmRight,
  HiChartPie,
  HiCog,
  HiInbox,
  HiPlus,
  HiShoppingBag,
  HiTable,
  HiUser,
  HiViewBoards,
} from "react-icons/hi";

const navLinks = [[
  { name: 'Dashboard', href: '/manager', icon: HiViewBoards },
  { name: 'Users', href: '/manager/users', icon: HiUser },
  { name: 'Register new user', href: '/manager/settings', icon: HiPlus },
],
[

]
];


export default function ManagerSidebar() {
  const pathname = usePathname();


  return (
    <Sidebar aria-label="Manager panel sidebar">
      <SidebarLogo href="#" img="/globe.svg" imgAlt="YES">
        {process.env.NEXT_PUBLIC_APP_NAME}
      </SidebarLogo>
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
