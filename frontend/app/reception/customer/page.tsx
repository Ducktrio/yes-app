"use client";

import CustomerService from "@/api/CustomerService";
import useFetchCustomers from "@/hooks/useFetchCustomers";
import { toast } from "@/lib/toast";
import { CreateCustomerPayload, Customer } from "@/types/Customer";
import { ValidationError, ValidationErrorResponse } from "@/types/Response";
import { useMutation } from "@tanstack/react-query";
import {
  Button,
  Checkbox,
  Label,
  Spinner,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeadCell,
  TableRow,
  TextInput,
} from "flowbite-react";
import { useSearchParams, useRouter } from "next/navigation";
import { useState } from "react";
export default function ReceptionCustomerPage() {
  const [form, setForm] = useState<CreateCustomerPayload>({
    age: 18,
    courtesy_title: "Mr.",
    full_name: "",
    phone_number: "",
  });
  const [errors, setErrors] = useState<ValidationErrorResponse[]>([]);
  const params = useSearchParams();
  const router = useRouter();

  const mutation = useMutation({
    mutationFn: CustomerService.createCustomer,

    onSuccess: (data: Customer) => {
      toast("success", "New customer has been registered");
      let url = "";

      if (params.has("redirect_to")) url = params.get("redirect_to") as string;
      if (url.includes("?")) url = `${url}&customer=${data.id}`;
      else url = `${url}?customer=${data.id}`;
      router.push(url);

      customers.refetch();
    },

    onError: (error) => {
      if (error instanceof ValidationError) {
        setErrors(error.errors);
      }
      toast("error", error.message);
    },
  });

  const customers = useFetchCustomers();

  return (
    <>
      <div className="flex flex-row gap-8">
        <div className="flex-2/3 flex-col gap-8">
          <div className="overflow-y-scroll max-h-[40vh]">
            <Table>
              <TableHead>
                <TableRow>
                  <TableHeadCell>Full Name</TableHeadCell>
                  <TableHeadCell>Phone Number</TableHeadCell>
                  <TableHeadCell>Created At</TableHeadCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {customers.isLoading && (
                  <TableRow>
                    <Spinner className="mr-2 w-5 h-5"></Spinner>Loading...
                  </TableRow>
                )}

                {customers.data?.map((customer) => (
                  <TableRow key={customer.id}>
                    <TableCell>
                      {customer.courtesy_title} {customer.full_name}
                    </TableCell>
                    <TableCell>{customer.phone_number}</TableCell>

                    <TableCell>{customer.created_at}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </div>

        <div className="flex-1/3 flex-col gap-8">
          <form
            className="flex max-w-md flex-col gap-4"
            onSubmit={(e) => {
              e.preventDefault();
              mutation.mutate(form);
            }}
          >
            <div>
              <div className="mb-2 block">
                <Label>Courtesy Title</Label>
              </div>
              <TextInput
                value={form.courtesy_title}
                onChange={(e) =>
                  setForm({ ...form, courtesy_title: e.target.value })
                }
                required
              />
            </div>

            <div>
              <div className="mb-2 block">
                <Label>Full Name</Label>
              </div>
              <TextInput
                value={form.full_name}
                onChange={(e) =>
                  setForm({ ...form, full_name: e.target.value })
                }
                required
              />
            </div>

            <div>
              <div className="mb-2 block">
                <Label>Age</Label>
              </div>
              <TextInput
                type="number"
                value={form.age}
                onChange={(e) =>
                  setForm({ ...form, age: e.target.valueAsNumber })
                }
                required
              />
            </div>

            <div>
              <div className="mb-2 block">
                <Label>Phone Number</Label>
              </div>
              <TextInput
                value={form.phone_number}
                onChange={(e) =>
                  setForm({ ...form, phone_number: e.target.value })
                }
                required
              />
            </div>

            <Button type="submit">Submit</Button>
          </form>
        </div>
      </div>
    </>
  );
}
