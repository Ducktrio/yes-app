"use client"

import { Card } from "flowbite-react";

export default function ManagerPage() {
  return <>
    <h1 className="text-2xl font-bold">Manager Dashboard</h1>
    <div className="flex flex-col space-y-4 mt-4">

      <div className="flex flex-row items-center space-x-4">

        <Card className="flex flex-col space-y-2 items-center justify-center p-8 bg-blue-100 text-blue-900 text-center">
          <h2 className="text-lg font-semibold">Total Users</h2>
            <span className="text-4xl">5</span>
        </Card>

        <Card className="flex flex-col space-y-2 items-center justify-center p-8 bg-green-100 text-green-900 text-center">
          <h2 className="text-lg font-semibold">Total Rooms</h2>
            <span className="text-4xl">3</span>
        </Card>

        <Card className="flex flex-col space-y-2 items-center justify-center p-8 bg-yellow-100 text-yellow-900 text-center">
          <h2 className="text-lg font-semibold">Total Bookings</h2>
            <span className="text-4xl">10</span>
        </Card>
      </div>

    </div>


  </>;
}
