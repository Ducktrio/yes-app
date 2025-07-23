"use client";

import { CreateUserPayload } from "@/types/User";
import { FormEvent, useState } from "react";
import {
  Alert,
  Button,
  Checkbox,
  Label,
  Modal,
  ModalBody,
  ModalHeader,
  Select,
  Spinner,
  Textarea,
  TextInput,
} from "flowbite-react";
import useFetchRoles from "@/hooks/useFetchRoles";
import { useMutation } from "@tanstack/react-query";
import UserService from "@/api/UserService";
import { HiInformationCircle } from "react-icons/hi";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";

export default function CreateUser({
  openModal,
  closeModal,
  onSuccess,
}: {
  openModal: boolean;
  closeModal: () => void;
  onSuccess?: () => void;
}) {
  const [form, setForm] = useState<CreateUserPayload>({
    username: "",
    description: "",
    password: "",
    role_id: "",
  });
  const roles = useFetchRoles();
  const [validationError, setValidationError] = useState<
    ValidationErrorResponse[]
  >([]);

  const mutate = useMutation({
    mutationKey: ["createUser"],
    mutationFn: UserService.createUser,
    onError(error, variables, context) {
      if (error instanceof ValidationError) {
        setValidationError(error.errors);
      }
    },
    onSuccess: (data) => {
      onSuccess?.();
      closeModal();
      setForm({
        username: "",
        description: "",
        password: "",
        role_id: "",
      });
      setValidationError([]);
      roles.refetch();
    },
  });

  const submit = (e: FormEvent) => {
    e.preventDefault();
    mutate.mutate(form);
  };

  return (
    <Modal show={openModal} onClose={closeModal} size="md" popup dismissible>
      <ModalHeader />
      <ModalBody>
        <form onSubmit={submit}>
          <div className="space-y-6">
            <h3 className="text-xl font-medium text-gray-900">
              Create a new user
            </h3>

            {mutate.isError && (
              <div className="mb-2 block">
                <Alert color="failure" icon={HiInformationCircle}>
                  <span className="font-medium">Error</span>
                  <ul>
                    {validationError.map((error) => (
                      <li key={error.resourceName}>{error.errorMessage}</li>
                    ))}
                  </ul>
                </Alert>
              </div>
            )}

            <div>
              <div className="mb-2 block">
                <Label htmlFor="username">Username</Label>
              </div>
              <TextInput
                id="username"
                placeholder="Username"
                value={form.username}
                onChange={(event) =>
                  setForm({ ...form, username: event.target.value })
                }
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
                onChange={(event) =>
                  setForm({ ...form, password: event.target.value })
                }
                required
              />
            </div>

            <div className="mb-2 block">
              <Label htmlFor="role_id">Role</Label>
              <Select
                required
                onChange={(event) =>
                  setForm({ ...form, role_id: event.target.value })
                }
                id="role_id"
              >
                {roles.data?.map((role) => (
                  <option key={role.id} value={role.id}>
                    {role.title}
                  </option>
                ))}
              </Select>
            </div>

            <div className="mb-2 block">
              <Label htmlFor="description">Description</Label>
              <Textarea
                id="description"
                onChange={(event) =>
                  setForm({ ...form, description: event.target.value })
                }
              />
            </div>

            <div className="w-full block">
              {(mutate.isPending && (
                <Button disabled className="w-full" color="alternative">
                  <Spinner
                    size="sm"
                    aria-label="Info spinner example"
                    className="me-3"
                    light
                  />
                  Loading...
                </Button>
              )) || (
                <Button type="submit" className="w-full">
                  Submit
                </Button>
              )}
            </div>
          </div>
        </form>
      </ModalBody>
    </Modal>
  );
}
