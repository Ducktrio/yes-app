
import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import { createTheme, DarkThemeToggle, ThemeConfig, ThemeModeScript, ThemeProvider } from "flowbite-react";
import { ToastProvider } from "./_components/providers/toast-provider";
import { AuthProvider } from "@/contexts/AuthContext";
import { Providers } from "./_components/providers/query-provider";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: process.env.APP_NAME,
  description: "Hotel Management Software",
};

const theme = createTheme({
  button: {
    color: {
      primary: "bg-blue-600 text-white hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 dark:focus:ring-blue-800",
      secondary: "bg-gray-600 text-white hover:bg-gray-700 focus:ring-4 focus:ring-gray-300 dark:focus:ring-gray-800",
      success: "bg-green-600 text-white hover:bg-green-700 focus:ring-4 focus:ring-green-300 dark:focus:ring-green-800",
      danger: "bg-red-600 text-white hover:bg-red-700 focus:ring-4 focus:ring-red-300 dark:focus:ring-red-800",
      warning: "bg-yellow-600 text-white hover:bg-yellow-700 focus:ring-4 focus:ring-yellow-300 dark:focus:ring-yellow-800",
      info: "bg-blue-500 text-white hover:bg-blue-600 focus:ring-4 focus:ring-blue-300 dark:focus:ring-blue-800"

    }
  }
})

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {


  return (
    <html lang="en" suppressHydrationWarning>

      <head>
        <ThemeModeScript />
        <ThemeConfig dark={false} />
      </head>

      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased bg-gray-50 text-gray-900`}>
        <ThemeProvider theme={theme} />
        <Providers>
          <AuthProvider>
            <ToastProvider>{children}</ToastProvider>
          </AuthProvider>
        </Providers>
      </body>
    </html>
  );
}
