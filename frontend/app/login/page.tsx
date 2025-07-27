"use client";

import Anima from "../_components/anima";
import {
  Card,
  Label,
  TextInput,
  Checkbox,
  Button,
  Spinner,
} from "flowbite-react";
import { LoginPayload } from "@/types/User";
import { FormEvent, FormEventHandler, useEffect, useState } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { useRouter } from "next/navigation";
import { toast } from "@/lib/toast";

export default function LoginPage() {
  const anima = Anima();
  const [form, setForm] = useState<LoginPayload>({
    username: "",
    password: "",
  });
  const auth = useAuth();
  const router = useRouter();

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    console.log("Submitting login form", form);
    auth.login(form);
  };
  return (
    <>
      <div
        ref={anima}
        className="min-h-lvh flex flex-col justify-center items-center"
      >
        <Card className="max-w-lg">
          <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
            <div>
              <div className="mb-2 block">
                <Label htmlFor="username">Username</Label>
              </div>
              <TextInput
                id="username"
                type="username"
                value={form.username}
                onChange={(e) => setForm({ ...form, username: e.target.value })}
                placeholder=""
                required
              />
            </div>
            <div>
              <div className="mb-2 block">
                <Label htmlFor="password">Password</Label>
              </div>
              <TextInput
                id="password"
                type="password"
                required
                value={form.password}
                onChange={(e) => setForm({ ...form, password: e.target.value })}
              />
            </div>
            <div className="flex items-center gap-2">
              <Checkbox id="remember" />
              <Label htmlFor="remember">Remember me</Label>
            </div>
            <Button type="submit" disabled={auth.loading}>
              {auth.loading && <Spinner className="mr-2 w-5 h-5" />}
              {auth.loading ? "Logging in..." : "Login"}
            </Button>
          </form>
        </Card>
      </div>
    </>
  );
}
