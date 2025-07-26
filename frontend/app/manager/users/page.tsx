"use client";

import useFetchRoles from "@/hooks/useFetchRoles";
import useFetchUsers from "@/hooks/useFetchUsers";

import { Spinner, Button } from "flowbite-react";
import { UserCard } from "../_components/usercard";
import { GoGitCommit } from "react-icons/go";
import { useState } from "react";
import CreateUser from "../_components/create-user";
import { toast } from "@/lib/toast";
import { Role } from "@/types/Role";

export default function ManageUserPage() {
  const users = useFetchUsers();
  const roles = useFetchRoles();

  const handleUserChange = () => {
    users.refetch();
  };
  const onCreatedUser = () => {
    toast("success", "User created successfully!");
    users.refetch();
  };

  const [createModal, setCreateModal] = useState(false);

  return (
    <>
      <h1 className="text-4xl font-bold mb-12">Manage Users</h1>
      <div className="flex flex-row items-center justify-between mt-4">
        <p className="text-gray-600">
          This page will allow you to manage users.
        </p>
        <Button
          className="bg-green-600 hover:bg-green-800 hover:shadow"
          onClick={() => setCreateModal(true)}
        >
          <GoGitCommit className="mr-2 h-5 w-5"></GoGitCommit>
          Create User
        </Button>
      </div>

      <div className="flex flex-wrap gap-4 mt-4">
        {users.isLoading && <Spinner className="mx-auto" />}
        {users.isError && (
          <p className="text-red-500">
            Error loading users: {users.error.message}
          </p>
        )}
        {users.data && users.data.length === 0 && <p>No users found.</p>}

        {users.data &&
          users.data.map((user) => (
            <UserCard
              key={user.id}
              user={user}
              role={
                roles.data?.find((role) => role.id === user.role_id) as Role
              }
              onChange={handleUserChange}
            />
          ))}
      </div>

      <div className="flex flex-row space-x-4 mt-4 space-y-4"></div>

      <CreateUser
        openModal={createModal}
        closeModal={() => setCreateModal(false)}
        onSuccess={onCreatedUser}
      />
    </>
  );
}
