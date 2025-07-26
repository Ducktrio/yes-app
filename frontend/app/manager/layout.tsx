"use client";

import { ReactNode } from "react";
import ManagerSidebar from "./_components/sidebar";
import { useAuth } from "@/contexts/AuthContext";
import Unauthorized from "../unauthorized/page";

export default function ManagerLayout({ children }: { children: ReactNode }) {
  const auth = useAuth();

  if (auth.role?.title !== "Manager") return <Unauthorized />;

  return (
    <div className="min-h-lvh flex flex-row">
      <div>
        <ManagerSidebar />
      </div>
      <div className="flex-1 p-12">{children}</div>
    </div>
  );
}
