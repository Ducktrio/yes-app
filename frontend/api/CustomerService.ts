import api from "@/lib/api";
import { CreateCustomerPayload, Customer } from "@/types/Customer";

export interface GetCustomerQuery {
  id?: string;
  full_name?: string;
  roomTicketId?: string;
  serviceTicketId?: string;
}

export default class CustomerService {
  static async getCustomers(query?: GetCustomerQuery): Promise<Customer[]> {
    return await api
      .get<Customer[]>("/Customers", { params: query })
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async createCustomer(customer: CreateCustomerPayload) {
    return await api
      .post<Customer>("/Customers", customer)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async updateCustomer(
    id: string,
    customer: Partial<CreateCustomerPayload>,
  ) {
    return await api
      .put<Customer>(`/Customers?id=${id}`, customer)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }

  static async deleteCustomer(id: string) {
    return await api
      .delete(`/Customers?id=${id}`)
      .then((res) => {
        return res.data;
      })
      .catch((err) => {
        if (err.response) {
          throw new Error(err.response.data.title);
        }
        throw err;
      });
  }
}

