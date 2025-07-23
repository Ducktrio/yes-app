"use client";
import UserService from "@/api/UserService";
import { toast } from "@/lib/toast";
import { Role } from "@/types/Role";
import { User } from "@/types/User";
import { useMutation } from "@tanstack/react-query";
import {
  Card,
  Dropdown,
  DropdownItem,
  Button,
  Modal,
  ModalBody,
  ModalHeader,
} from "flowbite-react";
import { useState } from "react";
import { GoPeople, GoPerson } from "react-icons/go";
import {
  HiOutlineCheckCircle,
  HiOutlineExclamationCircle,
} from "react-icons/hi";

interface UserCardProps {
  user: User;
  role: Role;
  onChange: () => void;
}
export function UserCard({ user, role, onChange }: UserCardProps) {
  const [deleteModal, setDeleteModal] = useState(false);
  const [editModal, setEditModal] = useState(false);

  const mutateUser = useMutation({
    mutationKey: ["deleteUser"],
    mutationFn: UserService.deleteUser,
    onSuccess: () => {
      toast("success", `User ${user.username} deleted successfully`);
      onChange();
    },
  });

  return (
    <>
      <Card className="w-full max-w-sm bg-white shadow-md animate-slide-in duration-300">
        <div className="flex justify-end px-4 pt-4">
          <Dropdown inline label="">
            <DropdownItem>
              <a
                href="#"
                className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
              >
                Edit
              </a>
            </DropdownItem>

            <DropdownItem>
              <a
                onClick={() => setDeleteModal(true)}
                className="block px-4 py-2 text-sm text-red-600 hover:bg-gray-100"
              >
                Delete
              </a>
            </DropdownItem>
          </Dropdown>
        </div>
        <div className="flex flex-col items-center pb-10">
          <GoPerson className="mb-3 h-16 w-16 text-gray-400" />
          <h5 className="mb-1 text-xl font-medium text-gray-900 ">
            {user?.username}
          </h5>
          <span className="text-sm text-gray-500 ">{role?.title}</span>
          <div className="mt-4 flex space-x-3 lg:mt-6">
            <p>{user.description}</p>
          </div>
        </div>
      </Card>

      <Modal
        show={deleteModal}
        size="md"
        onClose={() => setDeleteModal(false)}
        popup
      >
        <ModalHeader />
        <ModalBody>
          <div className="text-center">
            {mutateUser.isPending && <p>Deleting user...</p>}
            {mutateUser.isError && (
              <p className="text-red-500">
                Error deleting user: {mutateUser.error.message}
              </p>
            )}
            {mutateUser.isSuccess && (
              <>
                <HiOutlineCheckCircle className="mx-auto mb-4 h-14 w-14 text-green-500" />
                <h3 className="text-green-500">User deleted successfully</h3>
              </>
            )}

            {mutateUser.isIdle && (
              <>
                <HiOutlineExclamationCircle className="mx-auto mb-4 h-14 w-14 text-gray-400" />
                <h3 className="mb-5 text-lg font-normal text-gray-500">
                  Are you sure you want to delete user{" "}
                  <span className="font-semibold">{user?.username}</span>?
                </h3>
                <div className="flex justify-center gap-4">
                  <Button
                    color="red"
                    onClick={() => mutateUser.mutate(user?.id)}
                  >
                    Yes, I&apos;m sure
                  </Button>
                  <Button
                    color="alternative"
                    onClick={() => setDeleteModal(false)}
                  >
                    No, cancel
                  </Button>
                </div>
              </>
            )}
          </div>
        </ModalBody>
      </Modal>
    </>
  );
}
