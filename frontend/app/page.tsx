"use client";
import { Button, Card } from "flowbite-react";
import Anima from "./_components/anima";

export default function Home() {
  const anima = Anima();

  return (
    <>
      <div
        ref={anima}
        className="min-h-lvh flex flex-col justify-center items-center"
      >
        <Card className="max-w-sm text-center">
          <h1 className="text-4xl">{process.env.NEXT_PUBLIC_APP_NAME}</h1>

          <small>Hotel Management System</small>
          <small>{process.env.NEXT_PUBLIC_APP_COMPANY}</small>
          <Button href="login">Login</Button>
        </Card>
      </div>
    </>
  );
}
