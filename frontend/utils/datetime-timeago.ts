export function timeAgo(date: Date | string): string {
  const now = new Date();
  const inputDate = typeof date === "string" ? new Date(date) : date;
  const diffSeconds = Math.floor((now.getTime() - inputDate.getTime()) / 1000);

  const rtf = new Intl.RelativeTimeFormat("en", { numeric: "auto" });

  const units: [Intl.RelativeTimeFormatUnit, number][] = [
    ["year", 60 * 60 * 24 * 365],
    ["month", 60 * 60 * 24 * 30],
    ["week", 60 * 60 * 24 * 7],
    ["day", 60 * 60 * 24],
    ["hour", 60 * 60],
    ["minute", 60],
    ["second", 1],
  ];

  for (const [unit, secondsInUnit] of units) {
    const value = Math.floor(diffSeconds / secondsInUnit);
    if (Math.abs(value) >= 1) {
      return rtf.format(-value, unit);
    }
  }

  return "just now";
}
