export interface Customer {
    id: string;
    courtesy_title: string;
    full_name: string;
    age: number;
    phone_number: string;
    contact_info: string;
}
export interface CreateCustomerPayload {
    courtesy_title: string;
    full_name: string;
    age: number;
    phone_number: string;
    contact_info: string;
}