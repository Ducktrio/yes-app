"use client";
import { createContext, useContext, useEffect, useState } from "react";
import { registerToastHandler } from "@/lib/toast";
import clsx from "clsx";
import { Toast, ToastToggle } from "flowbite-react";
import {
  GoAlertFill,
  GoCheckCircleFill,
  GoInfo,
  GoXCircleFill,
} from "react-icons/go";

type ToastItem = {
  id: string;
  level: "info" | "success" | "error" | "warning";
  message: string;
};

const ToastContext = createContext({});

export function ToastProvider({ children }: { children: React.ReactNode }) {
  const [toasts, setToasts] = useState<ToastItem[]>([]);

  useEffect(() => {
    registerToastHandler((toast) => {
      setToasts((prev) => [...prev, toast]);
      setTimeout(() => {
        setToasts((prev) => prev.filter((t) => t.id !== toast.id));
      }, 3000);
    });
  }, []);

  return (
    <ToastContext.Provider value={{}}>
      {children}
      <div className="fixed top-4 right-4 space-y-2 z-50">
        {toasts.map((toast, index) => (
          <Toast key={index}>
            <div
              key={toast.id}
              className={clsx(
                "inline-flex h-8 w-8 shrink-0 items-center justify-center rounded-lg animate-slide-in",
                {
                  'bg-green-100 text-green-500 dark:bg-green-800 dark:text-green-200"':
                    toast.level === "success",

                  "bg-yellow-100 text-yellow-500 dark:bg-yellow-800 dark:text-yellow-200":
                    toast.level === "warning",
                  "bg-red-100 text-red-500 dark:bg-red-800 dark:text-red-200":
                    toast.level === "error",
                  "bg-blue-100 text-blue-500 dark:bg-blue-800 dark:text-blue-200":
                    toast.level === "info",
                },
              )}
            >
              {toast.level === "success" && (
                <GoCheckCircleFill className="h-5 w-5" />
              )}
              {toast.level === "warning" && <GoAlertFill className="h-5 w-5" />}

              {toast.level === "error" && <GoXCircleFill className="h-5 w-5" />}

              {toast.level === "info" && <GoInfo className="h-5 w-5" />}
            </div>
            <div className="ml-3 text-sm font-normal">{toast.message}</div>
            <ToastToggle />
          </Toast>
        ))}
      </div>
    </ToastContext.Provider>
  );
}
