// app/_components/providers/route-provider.tsx
"use client";

import type { PropsWithChildren } from "react";
import { useRouter } from "next/navigation";
import { RouterProvider } from "react-aria-components";

declare module "react-aria-components" {
  interface RouterConfig {
    routerOptions: NonNullable<
      Parameters<ReturnType<typeof useRouter>["push"]>[1]
    >;
  }
}

export const RouteProvider = ({ children }: PropsWithChildren) => {
  let router = useRouter();

  return <RouterProvider navigate={router.push}>{children}</RouterProvider>;
};
