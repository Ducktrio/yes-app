"use client";

import { ReactNode } from "react";
import ManagerSidebar from "./_components/sidebar";

export default function ManagerLayout({ children }: { children: ReactNode }) {
  return (
    <div className="min-h-lvh flex flex-row">
      <div>
        <ManagerSidebar />
      </div>
      <div className="flex-1 p-12">{children}</div>
    </div>
  );
}
