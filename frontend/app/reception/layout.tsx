"use client";

import { ReactNode } from "react";
import ReceptionSidebar from "./_components/sidebar";

export default function ReceptionLayout({ children }: { children: ReactNode }) {
  return (
    <div className="min-h-lvh flex flex-row">
      <div>
        <ReceptionSidebar />
      </div>
      <div className="flex-1 p-12 space-y-8 ">{children}</div>
    </div>
  );
}
