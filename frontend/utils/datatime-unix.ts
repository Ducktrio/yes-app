export function convertUnixToDate(unixTimestampStr: string) {
  if (!unixTimestampStr) {
    throw new Error("Invalid UNIX timestamp string");
  }

  // Convert to number and multiply by 1000 because JS Date expects milliseconds
  const timestampInMs = parseInt(unixTimestampStr, 10) * 1000;

  return new Date(timestampInMs);
}
