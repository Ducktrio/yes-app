// app/_components/providers/theme-provider.tsx
"use client";

import { ThemeProvider as NextThemeProvider } from "next-themes";
import { type PropsWithChildren } from "react";

export function ThemeProvider({ children }: PropsWithChildren) {
  return (
    <NextThemeProvider
      attribute="class"
      value={{ light: "light-mode", dark: "dark-mode" }}
    >
      {children}
    </NextThemeProvider>
  );
}
