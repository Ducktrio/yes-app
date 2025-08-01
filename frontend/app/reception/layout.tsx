"use client";

import { ReactNode, Suspense } from "react";
import ReceptionSidebar from "./_components/sidebar";

export default function ReceptionLayout({ children }: { children: ReactNode }) {
  return (
    <div className="min-h-lvh flex flex-row">
      <div className="sticky top-0 max-h-lvh">
        <ReceptionSidebar />
      </div>
      <Suspense>
        <div className="flex-1 p-12 space-y-8 ">{children}</div>
      </Suspense>
    </div>
  );
}
