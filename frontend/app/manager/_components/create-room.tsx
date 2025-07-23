"use client";

import { FormEvent, useState } from "react";
import {
  Alert,
  Button,
  Label,
  Modal,
  ModalBody,
  ModalHeader,
  Select,
  Spinner,
  TextInput,
} from "flowbite-react";
import { useMutation } from "@tanstack/react-query";
import { HiInformationCircle } from "react-icons/hi";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import { CreateRoomPayload } from "@/types/Room";
import RoomService from "@/api/RoomService";

export default function CreateRoom({
  openModal,
  closeModal,
  onSuccess,
}: {
  openModal: boolean;
  closeModal: () => void;
  onSuccess?: () => void;
}) {
  const types = useFetchRoomTypes();
  const [form, setForm] = useState<CreateRoomPayload>({
    label: "",
    roomType_id: (types?.data?.[0].id as string) || "RT_001", // Default to the first room type if available
  });
  const [validationError, setValidationError] = useState<
    ValidationErrorResponse[]
  >([]);

  const mutate = useMutation({
    mutationKey: ["createRoom"],
    mutationFn: RoomService.createRoom,
    onError(error, variables, context) {
      if (error instanceof ValidationError) {
        setValidationError(error.errors);
      }
    },
    onSuccess: (data) => {
      onSuccess?.();
      closeModal();
      setForm({
        label: "",
        roomType_id: "",
      });
      setValidationError([]);
      types.refetch();
    },
  });

  const submit = (e: FormEvent) => {
    e.preventDefault();
    mutate.mutate(form);
  };

  return (
    <Modal show={openModal} onClose={closeModal} size="md" popup>
      <ModalHeader />
      <ModalBody>
        <form onSubmit={submit}>
          <div className="space-y-6">
            <h3 className="text-xl font-medium text-gray-900">
              Register a new Room
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
                <Label htmlFor="label">Label</Label>
              </div>
              <TextInput
                id="label"
                placeholder="Room label"
                value={form.label}
                onChange={(event) =>
                  setForm({ ...form, label: event.target.value })
                }
                required
              />
            </div>
            <div className="mb-2 block">
              <Label htmlFor="roomType_id">Room type</Label>
              <Select
                required
                onChange={(event) =>
                  setForm({ ...form, roomType_id: event.target.value })
                }
                id="role_id"
              >
                {types.data?.map((type) => (
                  <option key={type.id} value={type.id}>
                    {type.name} | Rp {type.price}
                  </option>
                ))}
              </Select>
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
