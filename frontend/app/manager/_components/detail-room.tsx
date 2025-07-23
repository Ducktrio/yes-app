"use client";
import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import { Room, UpdateRoomPayload } from "@/types/Room";
import { FormEvent, useState } from "react";
import {
  Alert,
  Button,
  Label,
  Modal,
  ModalBody,
  ModalHeader,
  Spinner,
} from "flowbite-react";
import { useMutation } from "@tanstack/react-query";
import RoomService from "@/api/RoomService";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import {
  HiInformationCircle,
  HiOutlineExclamationCircle,
} from "react-icons/hi";

export default function DetailRoom({
  room,
  openModal,
  closeModal,
  onSuccess,
}: {
  room: Room;
  openModal: boolean;
  closeModal: () => void;
  onSuccess: () => void;
}) {
  const [form, setForm] = useState<UpdateRoomPayload>({
    status: room?.status,
  });

  const [validationError, setValidationError] = useState<
    ValidationErrorResponse[]
  >([]);

  const mutate = useMutation({
    mutationKey: ["updateRoom", room?.id],
    mutationFn: (payload: UpdateRoomPayload) => {
      return RoomService.updateRoom(room?.id, payload);
    },
    onError: (error, variables, context) => {
      if (error instanceof ValidationError) {
        setValidationError(error.errors);
      }
    },
    onSuccess: (data) => {
      onSuccess?.();
      closeModal();
      setForm({
        status: room?.status ?? "",
      });
      setValidationError([]);
    },
  });

  const deleteMutation = useMutation({
    mutationKey: ["deleteRoom", room?.id],
    mutationFn: RoomService.deleteRoom,
    onError(error, variables, context) {
      if (error instanceof ValidationError) {
        setValidationError(error.errors);
      }
    },
    onSuccess: () => {
      onSuccess?.();
      closeModal();
      setValidationError([]);
    },
  });

  const [deleteModal, setDeleteModal] = useState(false);

  const handleDelete = () => {
    deleteMutation.mutate(room.id);
    setDeleteModal(false);
  };
  const handleEdit = (e: FormEvent) => {
    e.preventDefault();
    mutate.mutate(form);
  };

  return (
    <>
      <Modal show={openModal} onClose={closeModal} size="md" popup>
        <ModalHeader />
        <ModalBody>
          <form onSubmit={handleEdit}>
            <div className="space-y-6">
              <h3 className="text-xl font-medium text-gray-900">
                Room {room?.label}
              </h3>

              {mutate.isError && (
                <div className="mb-12 block">
                  <Alert color="failure" icon={HiInformationCircle}>
                    <span className="font-medium">Error</span>
                    {mutate.error.message}
                    <ul>
                      {validationError.map((error) => (
                        <li key={error.resourceName}>{error.errorMessage}</li>
                      ))}
                    </ul>
                  </Alert>
                </div>
              )}

              <div className="mb-12 block">
                <Label htmlFor="description">Edit Room Status</Label>
                <div className="flex gap-4">
                  <Button
                    color={form.status === 0 ? "gray" : "alternative"}
                    onClick={() => setForm({ ...form, status: 0 })}
                  >
                    Available
                  </Button>

                  <Button
                    color={form.status === 2 ? "gray" : "alternative"}
                    onClick={() => setForm({ ...form, status: 2 })}
                  >
                    Unavailable
                  </Button>
                </div>
              </div>

              <div className="w-full block space-y-4">
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
                  <Button type="submit" outline className="w-full">
                    Submit
                  </Button>
                )}
                <Button
                  outline
                  color="red"
                  className="w-full"
                  onClick={() => setDeleteModal(true)}
                >
                  Delete
                </Button>
              </div>
            </div>
          </form>
        </ModalBody>
      </Modal>

      <Modal
        show={deleteModal}
        size="md"
        onClose={() => setDeleteModal(false)}
        popup
      >
        <ModalHeader />
        <ModalBody>
          <div className="text-center">
            <HiOutlineExclamationCircle className="mx-auto mb-4 h-14 w-14 text-gray-400 dark:text-gray-200" />
            <h3 className="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">
              Are you sure you want to delete this Room?
            </h3>
            <div className="flex justify-center gap-4">
              <Button color="red" onClick={handleDelete}>
                Yes, I&apos;m sure
              </Button>
              <Button color="alternative" onClick={() => setDeleteModal(false)}>
                No, cancel
              </Button>
            </div>
          </div>
        </ModalBody>
      </Modal>
    </>
  );
}
