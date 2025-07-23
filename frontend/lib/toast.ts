type ToastLevel = "info" | "success" | "error" | "warning";

type ToastMessage = {
  id: string;
  level: ToastLevel;
  message: string;
};

type ToastHandler = (msg: ToastMessage) => void;

let notify: ToastHandler | null = null;

export function registerToastHandler(fn: ToastHandler) {
  notify = fn;
}

export function toast(level: ToastLevel, message: string) {
  if (notify) {
    notify({ id: "adkosad", level, message });
  } else {
    console.warn("[toastLog] No toast system registered");
  }
}
